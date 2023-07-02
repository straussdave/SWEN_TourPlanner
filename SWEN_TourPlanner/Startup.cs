using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SWEN_TourPlanner.Model;
using System;
using System.Reflection;

public partial class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        var a = Assembly.GetExecutingAssembly();
        using var stream = a.GetManifestResourceStream("SWEN_TourPlanner.appsettings.json");
        var config = new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .Build();

        services.AddDbContext<TourPlannerDbContext>(options => options.UseNpgsql(config.GetConnectionString("cs")));
    }
}
