//using System;
//using System.Threading.Tasks;
//using Microsoft.Maui.Controls;

//namespace TrainingDiary
//{
//    public partial class App : Application
//    {
//        public App()
//        {
//            InitializeComponent();
//        }

//        protected override Window CreateWindow(IActivationState? activationState)
//            => new Window(new AppShell());
//    }
//}

namespace TrainingDiary;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new ContentPage
        {
            Content = new Label { Text = "Test page" }
        };
    }
}