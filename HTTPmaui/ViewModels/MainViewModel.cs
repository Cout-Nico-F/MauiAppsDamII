using System.Collections.ObjectModel;
using System.Windows.Input;
using HTTPmaui.Models;
using HTTPmaui.Services;

namespace HTTPmaui.ViewModels;

// ViewModel principal para la MainPage.
// Expone:
// - Lista observable de posts para mostrar en la UI.
// - Comandos para cargar datos y cancelar la carga.
// - Propiedades de estado (IsBusy, ErrorMessage) para dar feedback al usuario.
public class MainViewModel : BaseViewModel
{
    private readonly IApiService _api;
    private CancellationTokenSource? _cts; // Se usa para poder cancelar la llamada HTTP desde la UI.

    public ObservableCollection<Post> Posts { get; } = new();

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    private string _errorMessage = string.Empty;
    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    private int _postId = 1;
    public int PostId
    {
        // Propiedad bindable para que el usuario ingrese un Id y consultarlo.
        get => _postId;
        set => SetProperty(ref _postId, value);
    }

    // Comandos que la UI invocará (Command binding en XAML)
    public ICommand LoadPostsCommand { get; }
    public ICommand LoadPostByIdCommand { get; }
    public ICommand CancelCommand { get; }

    public MainViewModel(IApiService api)
    {
        _api = api;
        
        // Se definen los comandos y se enlazan a métodos locales async.
        LoadPostsCommand = new Command(async () => await LoadPostsAsync(), () => !IsBusy);
        LoadPostByIdCommand = new Command(async () => await LoadPostByIdAsync(), () => !IsBusy);
        CancelCommand = new Command(CancelRequests, () => IsBusy);
    }

    private void UpdateCanExecutes()
    {
        // Para refrescar el estado de los botones según IsBusy
        (LoadPostsCommand as Command)?.ChangeCanExecute();
        (LoadPostByIdCommand as Command)?.ChangeCanExecute();
        (CancelCommand as Command)?.ChangeCanExecute();
    }

    private void CancelRequests()
    {
        // Solo se intenta cancelar si hay una operación en curso
        if (_cts == null || !IsBusy) return;
        _cts.Cancel();
    }

    private async Task LoadPostsAsync()
    {
        ErrorMessage = string.Empty;
        Posts.Clear();

        _cts = new CancellationTokenSource();
        IsBusy = true; UpdateCanExecutes();
        try
        {
            var items = await _api.GetPostsAsync(_cts.Token);
            foreach (var p in items)
                Posts.Add(p);
        }
        catch (OperationCanceledException)
        {
            ErrorMessage = "Operación cancelada por el usuario.";
        }
        catch (Exception ex)
        {
            // Mostramos el mensaje pedagógico expuesto por el servicio.
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsBusy = false; UpdateCanExecutes();
            _cts?.Dispose();
            _cts = null;
        }
    }

    private async Task LoadPostByIdAsync()
    {
        ErrorMessage = string.Empty;
        Posts.Clear();

        _cts = new CancellationTokenSource();
        IsBusy = true; UpdateCanExecutes();
        try
        {
            var post = await _api.GetPostByIdAsync(PostId, _cts.Token);
            if (post != null)
                Posts.Add(post);
            else
                ErrorMessage = $"No se encontró el post con Id {PostId}.";
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
            IsBusy = false; UpdateCanExecutes();
            _cts?.Dispose();
            _cts = null;
        }
    }
}
