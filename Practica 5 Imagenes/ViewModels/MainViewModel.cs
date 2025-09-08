using System.Collections.ObjectModel;
using System.Windows.Input;
using FFImageLoading;
using Microsoft.Maui.Controls;
using Practica_5_Imagenes.Models;
using Practica_5_Imagenes.Services;

namespace Practica_5_Imagenes.ViewModels;

/// <summary>
/// ViewModel principal que gestiona la galería de fotografías.
/// 
/// Responsabilidades:
/// - Cargar y mostrar fotografías desde el servicio
/// - Gestionar el estado de carga (IsBusy) y mensajes de error/info
/// - Manejar búsquedas de fotografías
/// - Coordinar operaciones de cache de imágenes
/// - Proporcionar comandos para las acciones de la UI
/// - Gestionar la cancelación de operaciones HTTP
/// 
/// Patrón MVVM aplicado:
/// - No contiene lógica de UI específica (colores, tamaños, etc.)
/// - Expone propiedades bindables con INotifyPropertyChanged
/// - Usa comandos en lugar de eventos para las acciones
/// - Delega la lógica de negocio al servicio (IPhotoService)
/// </summary>
public class MainViewModel : BaseViewModel
{
    #region Campos privados y servicios
    
    private readonly IPhotoService _photoService;
    private CancellationTokenSource? _cancellationTokenSource;

    #endregion

    #region Campos de estado de la aplicación
    
    private bool _isBusy;
    private bool _isRefreshing;
    private string _errorMessage = string.Empty;
    private string _infoMessage = string.Empty;
    
    #endregion

    #region Campos de configuración y parámetros
    
    private string _searchTerm = string.Empty;
    private int _photosPerPage = 20;
    private bool _isCacheEnabled = true;
    private string _cacheInfo = string.Empty;

    #endregion

    #region Propiedades públicas - Colecciones

    /// <summary>
    /// Colección observable de fotografías que se muestra en la UI.
    /// ObservableCollection notifica automáticamente los cambios (Add, Remove, Clear)
    /// </summary>
    public ObservableCollection<Photo> Photos { get; } = new();

    #endregion

    #region Propiedades públicas - Estado de la UI

    /// <summary>
    /// Indica si hay una operación en curso (cargando fotos, buscando, etc.)
    /// Se usa para mostrar indicadores de carga y deshabilitar botones
    /// </summary>
    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            if (SetProperty(ref _isBusy, value))
                UpdateCommandStates();
        }
    }

    /// <summary>
    /// Indica si se está ejecutando una operación de actualización (pull-to-refresh)
    /// </summary>
    public bool IsRefreshing
    {
        get => _isRefreshing;
        set => SetProperty(ref _isRefreshing, value);
    }

    /// <summary>
    /// Mensaje de error para mostrar al usuario cuando algo falla
    /// </summary>
    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    /// <summary>
    /// Mensaje informativo para mostrar al usuario (éxito, información general)
    /// </summary>
    public string InfoMessage
    {
        get => _infoMessage;
        set => SetProperty(ref _infoMessage, value);
    }

    #endregion

    #region Propiedades públicas - Configuración y parámetros

    /// <summary>
    /// Término de búsqueda actual
    /// </summary>
    public string SearchTerm
    {
        get => _searchTerm;
        set => SetProperty(ref _searchTerm, value);
    }

    /// <summary>
    /// Número de fotos por página (para paginación)
    /// </summary>
    public int PhotosPerPage
    {
        get => _photosPerPage;
        set => SetProperty(ref _photosPerPage, Math.Max(1, Math.Min(value, 100)));
    }

    /// <summary>
    /// Indica si el cache de imágenes está habilitado
    /// </summary>
    public bool IsCacheEnabled
    {
        get => _isCacheEnabled;
        set => SetProperty(ref _isCacheEnabled, value);
    }

    /// <summary>
    /// Información sobre el cache actual (tamaño, número de imágenes, etc.)
    /// </summary>
    public string CacheInfo
    {
        get => _cacheInfo;
        set => SetProperty(ref _cacheInfo, value);
    }

    #endregion

    #region Comandos públicos

    /// <summary>
    /// Comando para cargar fotografías
    /// </summary>
    public ICommand LoadPhotosCommand { get; private set; } = null!;

    /// <summary>
    /// Comando para buscar fotografías por término
    /// </summary>
    public ICommand SearchPhotosCommand { get; private set; } = null!;

    /// <summary>
    /// Comando para actualizar la lista (pull-to-refresh)
    /// </summary>
    public ICommand RefreshCommand { get; private set; } = null!;

    /// <summary>
    /// Comando para cancelar la operación actual
    /// </summary>
    public ICommand CancelCommand { get; private set; } = null!;

    /// <summary>
    /// Comando para limpiar el cache de imágenes
    /// </summary>
    public ICommand ClearCacheCommand { get; private set; } = null!;

    /// <summary>
    /// Comando para alternar el estado del cache
    /// </summary>
    public ICommand ToggleCacheCommand { get; private set; } = null!;

    /// <summary>
    /// Comando para obtener información del cache
    /// </summary>
    public ICommand GetCacheInfoCommand { get; private set; } = null!;

    #endregion

    #region Constructor

    public MainViewModel(IPhotoService photoService)
    {
        _photoService = photoService ?? throw new ArgumentNullException(nameof(photoService));

        InitializeCommands();
        
        // Cargar fotos iniciales de forma asíncrona
        _ = ExecuteLoadPhotosAsync();
    }

    /// <summary>
    /// Inicializa todos los comandos con sus respectivos métodos y CanExecute
    /// </summary>
    private void InitializeCommands()
    {
        LoadPhotosCommand = new Command(async () => await ExecuteLoadPhotosAsync(), () => !IsBusy);
        SearchPhotosCommand = new Command(async () => await ExecuteSearchPhotosAsync(), () => !IsBusy && !string.IsNullOrWhiteSpace(SearchTerm));
        RefreshCommand = new Command(async () => await ExecuteRefreshAsync());
        CancelCommand = new Command(ExecuteCancel, () => IsBusy);
        ClearCacheCommand = new Command(async () => await ExecuteClearCacheAsync(), () => !IsBusy);
        ToggleCacheCommand = new Command(ExecuteToggleCache);
        GetCacheInfoCommand = new Command(async () => await ExecuteGetCacheInfoAsync());
    }

    #endregion

    #region Métodos de comandos - Carga de datos

    /// <summary>
    /// Carga fotografías desde el servicio
    /// </summary>
    private async Task ExecuteLoadPhotosAsync()
    {
        await ExecuteWithErrorHandlingAsync(async (ct) =>
        {
            var photos = await _photoService.GetPhotosAsync(PhotosPerPage, ct);
            
            Photos.Clear();
            foreach (var photo in photos)
            {
                Photos.Add(photo);
            }

            InfoMessage = $"Se cargaron {photos.Count} fotografías";
        });
    }

    /// <summary>
    /// Busca fotografías por término
    /// </summary>
    private async Task ExecuteSearchPhotosAsync()
    {
        if (string.IsNullOrWhiteSpace(SearchTerm))
            return;

        await ExecuteWithErrorHandlingAsync(async (ct) =>
        {
            var photos = await _photoService.SearchPhotosAsync(SearchTerm, PhotosPerPage, ct);
            
            Photos.Clear();
            foreach (var photo in photos)
            {
                Photos.Add(photo);
            }

            InfoMessage = $"Se encontraron {photos.Count} fotografías para '{SearchTerm}'";
        });
    }

    /// <summary>
    /// Actualiza la lista de fotografías (pull-to-refresh)
    /// </summary>
    private async Task ExecuteRefreshAsync()
    {
        IsRefreshing = true;
        try
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                await ExecuteLoadPhotosAsync();
            }
            else
            {
                await ExecuteSearchPhotosAsync();
            }
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    #endregion

    #region Métodos de comandos - Control de operaciones

    /// <summary>
    /// Cancela la operación HTTP actual
    /// </summary>
    private void ExecuteCancel()
    {
        _cancellationTokenSource?.Cancel();
        InfoMessage = "Operación cancelada por el usuario";
    }

    #endregion

    #region Métodos de comandos - Gestión de cache

    /// <summary>
    /// Limpia el cache de imágenes
    /// </summary>
    private async Task ExecuteClearCacheAsync()
    {
        try
        {
            IsBusy = true;
            
            // Limpiar cache de FFImageLoading
            await ImageService.Instance.InvalidateCacheAsync(FFImageLoading.Cache.CacheType.All);
            
            InfoMessage = "Cache de imágenes limpiado correctamente";
            await ExecuteGetCacheInfoAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error al limpiar el cache: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// Alterna el estado del cache de imágenes
    /// </summary>
    private void ExecuteToggleCache()
    {
        IsCacheEnabled = !IsCacheEnabled;
        InfoMessage = IsCacheEnabled ? "Cache habilitado" : "Cache deshabilitado";
        
        // Actualizar información del cache
        _ = ExecuteGetCacheInfoAsync();
    }

    /// <summary>
    /// Obtiene información sobre el cache actual
    /// </summary>
    private async Task ExecuteGetCacheInfoAsync()
    {
        try
        {
            // Información básica del cache y estado de la aplicación
            var cacheStatus = IsCacheEnabled ? "Habilitado" : "Deshabilitado";
            var photosLoaded = Photos.Count;
            
            CacheInfo = $"Estado: {cacheStatus}\n" +
                       $"Fotos cargadas: {photosLoaded}\n" +
                       $"FFImageLoading: Configurado";
            
            await Task.Delay(100); // Simular trabajo
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error al obtener información del cache: {ex.Message}";
        }
    }

    #endregion

    #region Métodos auxiliares - Manejo de errores y estado

    /// <summary>
    /// Ejecuta una operación asíncrona con manejo estándar de errores y cancelación
    /// </summary>
    /// <param name="operation">Operación a ejecutar que puede ser cancelada</param>
    private async Task ExecuteWithErrorHandlingAsync(Func<CancellationToken, Task> operation)
    {
        // Limpiar mensajes previos
        ErrorMessage = string.Empty;
        InfoMessage = string.Empty;
        
        // Preparar cancelación
        _cancellationTokenSource = new CancellationTokenSource();
        IsBusy = true;

        try
        {
            await operation(_cancellationTokenSource.Token);
        }
        catch (OperationCanceledException)
        {
            InfoMessage = "Operación cancelada por el usuario";
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }
    }

    /// <summary>
    /// Actualiza el estado CanExecute de todos los comandos
    /// </summary>
    private void UpdateCommandStates()
    {
        (LoadPhotosCommand as Command)?.ChangeCanExecute();
        (SearchPhotosCommand as Command)?.ChangeCanExecute();
        (CancelCommand as Command)?.ChangeCanExecute();
        (ClearCacheCommand as Command)?.ChangeCanExecute();
    }

    #endregion
}