using Microsoft.Extensions.Logging;
using Practica6Shell.Services;
using Practica6Shell.ViewModels;
using Practica6Shell.Views;

namespace Practica6Shell
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

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Registrar servicios
            builder.Services.AddSingleton<IProductoService, ProductoService>();

            // Registrar ViewModels
            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<ProductosViewModel>();
            builder.Services.AddTransient<DetalleProductoViewModel>();

            // Registrar Pages
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<ProductosPage>();
            builder.Services.AddTransient<DetalleProductoPage>();
            builder.Services.AddTransient<CategoriasPage>();
            builder.Services.AddTransient<ConfiguracionPage>();
            builder.Services.AddTransient<AcercaPage>();
            builder.Services.AddTransient<EditarProductoPage>();

            return builder.Build();
        }
    }
}