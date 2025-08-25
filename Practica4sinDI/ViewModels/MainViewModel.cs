using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq; // Para consultas sobre colecciones
using Microsoft.Maui.Controls; // Para Command
using Practica4sinDI.Models;
using Practica4sinDI.Services;

namespace Practica4sinDI.ViewModels;

// ViewModel simple sin DI: el propio VM crea su dependencia (ApiService).
// Responsabilidades del ViewModel (MVVM):
// - Exponer estado bindable para la UI (propiedades y colecciones).
// - Coordinar acciones de la UI llamando a servicios (acceso HTTP) y procesando resultados.
// - No contiene elementos de UI (no usa tipos visuales), solo lógica de presentación.
//
// Este VM también mantiene un "overlay" local de cambios (upserts y borrados)
// para que se aprecie el efecto de POST/PUT/DELETE aunque la API pública de ejemplo
// (jsonplaceholder) no persiste los cambios. Esto se explica cuando comparamos la
// lista original del servidor con la lista fusionada (merged) que mostramos.
public class MainViewModel : BaseViewModel
{
    // Servicio HTTP (sin DI en esta práctica): el VM administra su propia instancia.
    private readonly ApiService _api = new(); // Se instancia directamente sin DI

    // Fuente de cancelación para cortar solicitudes HTTP de larga duración.
    // Regla: crearla al iniciar una operación, usar su Token y desecharla en finally.
    private CancellationTokenSource? _cts;

    // Cache local de cambios de esta sesión
    private readonly Dictionary<int, Post> _localUpserts = new(); // Creados/actualizados
    private readonly HashSet<int> _deletedIds = new(); // Eliminados

    // Colección observable: al agregar/quitar/reemplazar elementos, la UI se actualiza automáticamente.
    public ObservableCollection<Post> Posts { get; } = new();

    // Bandera de ocupación para deshabilitar botones mientras hay un request en curso
    private bool _isBusy;
    public bool IsBusy { get => _isBusy; set { if (SetProperty(ref _isBusy, value)) UpdateCanExecutes(); } }

    // Mensajes informativos y de error para la UI
    private string _errorMessage = string.Empty;
    public string ErrorMessage { get => _errorMessage; set => SetProperty(ref _errorMessage, value); }

    private string _infoMessage = string.Empty;
    public string InfoMessage { get => _infoMessage; set => SetProperty(ref _infoMessage, value); }

    // Ids separados por operación: mejora la UX al no reutilizar el mismo campo para todo
    private int _searchId = 1;
    public int SearchId { get => _searchId; set => SetProperty(ref _searchId, value); }

    private int _updateId = 1;
    public int UpdateId { get => _updateId; set { if (SetProperty(ref _updateId, value)) UpdateCanExecutes(); } }

    private int _deleteId = 1;
    public int DeleteId { get => _deleteId; set { if (SetProperty(ref _deleteId, value)) UpdateCanExecutes(); } }

    // Campos de entrada para crear/actualizar posts
    private string _createTitle = string.Empty;
    public string CreateTitle { get => _createTitle; set => SetProperty(ref _createTitle, value); }

    private string _createBody = string.Empty;
    public string CreateBody { get => _createBody; set => SetProperty(ref _createBody, value); }

    private string _updateTitle = string.Empty;
    public string UpdateTitle { get => _updateTitle; set => SetProperty(ref _updateTitle, value); }

    private string _updateBody = string.Empty;
    public string UpdateBody { get => _updateBody; set => SetProperty(ref _updateBody, value); }

    // Comandos que la UI puede invocar. CanExecute se liga a IsBusy para evitar dobles envíos.
    public ICommand LoadPostsCommand { get; }
    public ICommand LoadPostByIdCommand { get; }
    public ICommand CancelCommand { get; }

    public ICommand CreatePostCommand { get; }
    public ICommand UpdatePostCommand { get; }
    public ICommand DeletePostCommand { get; }

    public MainViewModel()
    {
        // Importante: los CanExecute usan !IsBusy para evitar múltiples requests simultáneos.
        LoadPostsCommand = new Command(async () => await LoadPostsAsync(), () => !IsBusy);
        LoadPostByIdCommand = new Command(async () => await LoadPostByIdAsync(), () => !IsBusy);
        CancelCommand = new Command(CancelRequests, () => IsBusy);

        CreatePostCommand = new Command(async () => await CreatePostAsync(), () => !IsBusy);
        UpdatePostCommand = new Command(async () => await UpdatePostAsync(), () => !IsBusy && UpdateId > 0);
        DeletePostCommand = new Command(async () => await DeletePostAsync(), () => !IsBusy && DeleteId > 0);
    }

    // Cuando IsBusy cambia, avisamos a los comandos para que recalculen CanExecute y la UI actualice el estado de los botones
    private void UpdateCanExecutes()
    {
        (LoadPostsCommand as Command)?.ChangeCanExecute();
        (LoadPostByIdCommand as Command)?.ChangeCanExecute();
        (CancelCommand as Command)?.ChangeCanExecute();
        (CreatePostCommand as Command)?.ChangeCanExecute();
        (UpdatePostCommand as Command)?.ChangeCanExecute();
        (DeletePostCommand as Command)?.ChangeCanExecute();
    }

    // Solicitud de cancelación: se respeta por los métodos del servicio mediante CancellationToken
    private void CancelRequests()
    {
        if (_cts == null || !IsBusy) return;
        _cts.Cancel();
    }

    // Cargar todos los posts desde la API y fusionarlos con el overlay local.
    private async Task LoadPostsAsync()
    {
        ErrorMessage = string.Empty;
        InfoMessage = string.Empty;
        _cts = new CancellationTokenSource();
        IsBusy = true; // Patrón: fijar IsBusy antes del await
        try
        {
            // El servicio aplica try/catch y lanza mensajes pedagógicos si hay problemas de red.
            var items = await _api.GetPostsAsync(_cts.Token);

            // Fusión con overlay local:
            // - Aplicamos upserts locales
            // - Quitamos eliminados locales
            var map = items.ToDictionary(p => p.Id, p => p);
            foreach (var kv in _localUpserts)
                map[kv.Key] = kv.Value; // upsert
            foreach (var delId in _deletedIds)
                map.Remove(delId);

            var merged = map.Values.OrderBy(p => p.Id).ToList();

            // Reflejamos el resultado en la CollectionView
            Posts.Clear();
            foreach (var p in merged) Posts.Add(p);

            InfoMessage = $"Cargados {Posts.Count} elementos.";
        }
        catch (OperationCanceledException)
        {
            // Cancelación solicitada por el usuario: no es un error de red.
            ErrorMessage = "Operación cancelada por el usuario.";
        }
        catch (Exception ex)
        {
            // Mensaje ya “curado” en el servicio. Evitamos filtrar detalles técnicos a la UI.
            ErrorMessage = ex.Message;
        }
        finally
        {
            // Patrón: liberar recursos y restablecer estado sin importar el resultado
            IsBusy = false;
            _cts?.Dispose();
            _cts = null;
        }
    }

    // Cargar un post por Id, priorizando el overlay local si existe.
    private async Task LoadPostByIdAsync()
    {
        ErrorMessage = string.Empty;
        InfoMessage = string.Empty;
        _cts = new CancellationTokenSource();
        IsBusy = true;
        try
        {
            Posts.Clear();

            // Primero respetamos overlay local: si está eliminado, lo informamos; si hay upsert, lo mostramos.
            if (_deletedIds.Contains(SearchId))
            {
                ErrorMessage = "Este Id fue eliminado localmente en esta sesión.";
                return;
            }
            if (_localUpserts.TryGetValue(SearchId, out var local))
            {
                Posts.Add(local);
                return;
            }

            // Si no hay overlay, consultamos al servidor
            var post = await _api.GetPostByIdAsync(SearchId, _cts.Token);
            if (post != null)
            {
                Posts.Add(post);
                InfoMessage = "Elemento cargado.";
            }
            else ErrorMessage = $"No se encontró el post con Id {SearchId}.";
        }
        catch (OperationCanceledException)
        {
            ErrorMessage = "Operación cancelada por el usuario.";
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
            _cts?.Dispose();
            _cts = null;
        }
    }

    // Crear un post: tras éxito, se actualiza el overlay y se refleja inmediatamente en la UI.
    private async Task CreatePostAsync()
    {
        ErrorMessage = string.Empty;
        InfoMessage = string.Empty;
        _cts = new CancellationTokenSource();
        IsBusy = true;
        try
        {
            var toCreate = new Post { Title = CreateTitle, Body = CreateBody, UserId = 1 };
            var created = await _api.CreatePostAsync(toCreate, _cts.Token);

            // Guardamos en overlay (upsert) y retiramos cualquier marca de borrado previa
            _localUpserts[created.Id] = created;
            _deletedIds.Remove(created.Id);

            // Sincronizamos campos de Id para facilitar pruebas rápidas desde la UI
            SearchId = UpdateId = DeleteId = created.Id;

            // Refrescamos la colección local sin depender de otra llamada a red
            UpsertInCollection(created);

            InfoMessage = "Creado localmente en esta sesión.";
        }
        catch (OperationCanceledException)
        {
            ErrorMessage = "Operación cancelada por el usuario.";
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
            _cts?.Dispose();
            _cts = null;
        }
    }

    // Actualizar un post existente por Id. También actualiza el overlay para que
    // la lista refleje los cambios aunque el servidor no los persista.
    private async Task UpdatePostAsync()
    {
        ErrorMessage = string.Empty;
        InfoMessage = string.Empty;
        if (UpdateId <= 0)
        {
            ErrorMessage = "Ingrese un Id válido para actualizar.";
            return;
        }

        _cts = new CancellationTokenSource();
        IsBusy = true;
        try
        {
            var update = new Post { Id = UpdateId, Title = UpdateTitle, Body = UpdateBody, UserId = 1 };
            var updated = await _api.UpdatePostAsync(UpdateId, update, _cts.Token);

            _localUpserts[UpdateId] = updated;
            _deletedIds.Remove(UpdateId);

            UpsertInCollection(updated);

            InfoMessage = "Actualizado localmente en esta sesión.";
        }
        catch (OperationCanceledException)
        {
            ErrorMessage = "Operación cancelada por el usuario.";
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
            _cts?.Dispose();
            _cts = null;
        }
    }

    // Eliminar un post. Registramos el Id en el conjunto de eliminados y
    // lo removemos de la colección para que la UI lo deje de mostrar.
    private async Task DeletePostAsync()
    {
        ErrorMessage = string.Empty;
        InfoMessage = string.Empty;
        if (DeleteId <= 0)
        {
            ErrorMessage = "Ingrese un Id válido para eliminar.";
            return;
        }

        _cts = new CancellationTokenSource();
        IsBusy = true;
        try
        {
            var ok = await _api.DeletePostAsync(DeleteId, _cts.Token);
            if (ok)
            {
                _deletedIds.Add(DeleteId);
                _localUpserts.Remove(DeleteId);

                var existing = Posts.FirstOrDefault(p => p.Id == DeleteId);
                if (existing != null) Posts.Remove(existing);
                InfoMessage = "Eliminado localmente en esta sesión.";
            }
        }
        catch (OperationCanceledException)
        {
            ErrorMessage = "Operación cancelada por el usuario.";
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
            _cts?.Dispose();
            _cts = null;
        }
    }

    // Inserta o reemplaza un Post en la CollectionView actual.
    // Observación: asignar por índice en ObservableCollection dispara un evento Replace,
    // lo que notifica a la UI que el elemento cambió y debe re-renderizarse.
    private void UpsertInCollection(Post post)
    {
        var idx = Posts.ToList().FindIndex(p => p.Id == post.Id);
        if (idx >= 0) Posts[idx] = post; else Posts.Add(post);
    }
}
