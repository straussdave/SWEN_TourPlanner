using SWEN_TourPlanner.ViewModel;

namespace SWEN_TourPlanner;

public partial class App : Application
{
	public App(MainPage page)
	{
		InitializeComponent();

		MainPage = page;
	}
}
