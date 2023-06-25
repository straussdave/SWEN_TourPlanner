using SWEN_TourPlanner.ViewModel;

namespace SWEN_TourPlanner;

public partial class MainPage : ContentPage
{
    public MainPage()
	{
		InitializeComponent();
		BindingContext = new MainViewModel();
	}
}

