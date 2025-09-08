using Microsoft.Extensions.Logging;
using FFImageLoading.Maui;
using Practica_5_Imagenes.Services;
using Practica_5_Imagenes.ViewModels;

namespace Practica_5_Imagenes;
/// <summary>
/// Configuración principal de la aplicación MAUI.
/// 
/// En este archivo se registran:
/// - Servicios en el contenedor de DI (Dependency Injection)
/// - Configuración de FFImageLoading para cache de imágenes
/// - HttpClient con IHttpClientFactory para manejo eficiente de conexiones
/// - ViewModels y Pages para inyección de dependencias
/// - Configuración de logging para depuración
/// 
/// Diferencias con la práctica anterior (sin DI):
/// - Aquí usamos DI para desacoplar dependencias
/// - IHttpClientFactory maneja el pool de conexiones HTTP automáticamente
/// - Los ViewModels reciben sus dependencias por constructor
/// - Facilita testing y cambio de implementaciones
/// </summary>
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        
        builder
            .UseMauiApp<App>()
            .UseFFImageLoading() // Configurar FFImageLoading para cache avanzado de imágenes
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Configuración de servicios HTTP
        // AddHttpClient registra IHttpClientFactory y configura un HttpClient específico para PhotoService
        builder.Services.AddHttpClient<IPhotoService, PhotoService>(client =>
        {
            // Configuración específica para el cliente HTTP del PhotoService
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Practica5Imagenes-MAUI/1.0");
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        // Registro de ViewModels
        // AddTransient: nueva instancia cada vez que se solicite
        // AddSingleton: una sola instancia durante toda la vida de la app
        builder.Services.AddSingleton<MainViewModel>();

        // Registro de Pages
        // Las páginas también se pueden registrar para recibir dependencias
        builder.Services.AddSingleton<MainPage>();

#if DEBUG
        // Logging solo en modo debug para no impactar performance en release
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
