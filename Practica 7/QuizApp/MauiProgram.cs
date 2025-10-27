using Microsoft.Extensions.Logging;
using QuizApp.Services;
using QuizApp.ViewModels;
using QuizApp.Views;

namespace QuizApp;

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

		// Register services
		builder.Services.AddSingleton<IQuizService, QuizService>();

		// Register ViewModels
		builder.Services.AddTransient<MainViewModel>();
		builder.Services.AddTransient<QuizViewModel>();
		builder.Services.AddTransient<ResultadoViewModel>();

		// Register Pages
		builder.Services.AddTransient<MainPage>();
		builder.Services.AddTransient<QuizPage>();
		builder.Services.AddTransient<ResultadoPage>();

		return builder.Build();
	}
}
