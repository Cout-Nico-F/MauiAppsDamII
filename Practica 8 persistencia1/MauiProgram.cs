?using Microsoft.Extensions.Logging;
using Practica_8_persistencia1.Services;

namespace Practica_8_persistencia1
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
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "people.db3");
            builder.Services.AddSingleton(s => new DatabaseService(dbPath));


            return builder.Build();
        }
    }
}