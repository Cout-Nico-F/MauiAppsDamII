using Microsoft.Extensions.Logging;
using Practica9.Pages;
using Practica9.ViewModels;

namespace Practica_9
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

            // Registrar ViewModels
            builder.Services.AddSingleton<CalculadoraViewModel>();
            builder.Services.AddSingleton<TiendaViewModel>();

            // Registrar Pages
            builder.Services.AddSingleton<CalculadoraPage>();
            builder.Services.AddSingleton<TiendaPage>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
