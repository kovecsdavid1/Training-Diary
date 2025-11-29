using TrainingDiary.ViewModels;

namespace TrainingDiary.Views;

public partial class HomePage : ContentPage
{
    public HomePage(HomePageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    //private async void HomePage_OnLoaded(object? sender, EventArgs e)
    //{
    //    await viewModel.InitializeAsync();
    //}
}