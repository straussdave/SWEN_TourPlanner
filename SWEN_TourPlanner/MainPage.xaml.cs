using Microsoft.Extensions.Configuration;
using SWEN_TourPlanner.ViewModel;

namespace SWEN_TourPlanner;

public partial class MainPage : ContentPage
{
    public IConfiguration configuration;

    public MainPage(IConfiguration config)
	{
		InitializeComponent();
		BindingContext = new MainViewModel();
        configuration = config;
    }
}

