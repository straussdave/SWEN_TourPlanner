using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SWEN_TourPlanner.ViewModel;
using System.Reflection;

namespace SWEN_TourPlanner;

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
        builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<MainViewModel>();

        var a = Assembly.GetExecutingAssembly();
        using var stream = a.GetManifestResourceStream("SWEN_TourPlanner.appsettings.json");

        var config = new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .Build();


        builder.Configuration.AddConfiguration(config);
        builder.Services.AddTransient<MainPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif
        var app = builder.Build();
        Services = app.Services;

        return app;
	}

    public static IServiceProvider Services { get; private set; }
}
