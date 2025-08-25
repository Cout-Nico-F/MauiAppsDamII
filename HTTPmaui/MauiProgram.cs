using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using HTTPmaui.Services;
using HTTPmaui.ViewModels;

namespace HTTPmaui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Registro de servicios en el contenedor de DI (Dependency Injection)
            // AddHttpClient usa IHttpClientFactory, que es la forma recomendada de usar HttpClient.
            builder.Services.AddHttpClient<IApiService, ApiService>(client =>
            {
                // Se configura la URL base para todas las solicitudes del ApiService
                client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
            });

            // Registro de ViewModels
            builder.Services.AddSingleton<MainViewModel>();

            // Registro de Pages. Registrar la página permite resolverla desde el contenedor si se necesita.
            builder.Services.AddSingleton<MainPage>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
