using Practica_5_Imagenes.ViewModels;

namespace Practica_5_Imagenes;

/// <summary>
/// Página principal de la aplicación de galería de fotografías.
/// 
/// En MVVM, el code-behind debe mantenerse mínimo y contener solo:
/// - Lógica de UI específica que no se puede hacer con bindings
/// - Manejo de eventos de ciclo de vida de la página
/// - Interacciones complejas con controles (animaciones, focus, etc.)
/// 
/// Toda la lógica de negocio y estado está en el MainViewModel.
/// </summary>
public partial class MainPage : ContentPage
{
    /// <summary>
    /// Constructor que recibe el ViewModel por inyección de dependencias.
    /// 
    /// Al recibir el ViewModel por DI:
    /// - Se garantiza que todas las dependencias del VM estén resueltas
    /// - Facilita testing al poder inyectar mocks
    /// - Sigue el principio de inversión de dependencias
    /// </summary>
    /// <param name="viewModel">ViewModel que maneja la lógica de esta página</param>
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        
        // Asignar el ViewModel como BindingContext
        // Esto conecta la UI con el ViewModel para data binding
        BindingContext = viewModel;
    }

    /// <summary>
    /// Se ejecuta cuando la página aparece en pantalla.
    /// Útil para refrescar datos o reiniciar estados.
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();
        
        // Aquí podríamos agregar lógica específica cuando la página aparece,
        // como actualizar datos o mostrar animaciones de entrada.
        // En nuestro caso, el ViewModel ya carga los datos automáticamente
        // en su constructor, pero podrías agregar:
        // 
        // if (BindingContext is MainViewModel viewModel)
        // {
        //     _ = viewModel.RefreshCommand.Execute(null);
        // }
    }

    /// <summary>
    /// Se ejecuta cuando la página desaparece de la pantalla.
    /// Útil para limpiar recursos o pausar operaciones.
    /// </summary>
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        
        // Aquí podríamos cancelar operaciones en curso para ahorrar recursos:
        // 
        // if (BindingContext is MainViewModel viewModel)
        // {
        //     viewModel.CancelCommand.Execute(null);
        // }
    }
}
