using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using log4net.Appender;
using SWEN_TourPlanner.Model;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace SWEN_TourPlanner.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        Logger logger = new();

        int counter;
        [ObservableProperty] //this auto generates code in Dependencies/net7.0-windows/CommunityToolkin.Mvvm.SourceGenerators/CoomunityToolkit.MvvmSourceGenerators.ObservablePropertyGenerator
        string text;
        
        DatabaseHandler handler = new();

        [RelayCommand] //example command
        void Clicked()
        {
            counter++;
            Text = counter.ToString();
        }

        public MainViewModel()
        {
            logger.log.Debug("MainViewModel created");
        }
    }
}
