using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SWEN_TourPlanner.Model;
using SWEN_TourPlanner.ViewModel;
using System.Diagnostics;
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
        

#if DEBUG
        builder.Logging.AddDebug();
#endif
        var app = builder.Build();
        Services = app.Services;

        return app;
	}

    public static IServiceProvider Services { get; private set; }
}
