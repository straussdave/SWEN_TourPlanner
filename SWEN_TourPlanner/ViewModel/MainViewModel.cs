using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.Windows.Input;

namespace SWEN_TourPlanner.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        int counter;
        [ObservableProperty] //this auto generates code in Dependencies/net7.0-windows/CommunityToolkin.Mvvm.SourceGenerators/CoomunityToolkit.MvvmSourceGenerators.ObservablePropertyGenerator
        string text;

        [RelayCommand]
        void Clicked()
        {
            counter++;
            Text = counter.ToString();
        }
    }
}
