using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
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

        //example data for the datagrid

        static Random random = new();
        public ObservableCollection<Student> Items { get; } = new();
        public MainViewModel()
        {
            for (int i = 0; i < 10; i++)
            {
                Items.Add(new Student
                {
                    Id = i,
                    Name = "Person " + i,
                    Age = random.Next(14, 85),
                });
            }
        }
        public class Student
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
        }
    }
}
