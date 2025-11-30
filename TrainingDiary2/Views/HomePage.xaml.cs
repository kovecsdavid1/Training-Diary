using CommunityToolkit.Mvvm.Messaging;
using TrainingDiary2.ViewModels;

namespace TrainingDiary2.Views;

public partial class HomePage : ContentPage
{
    private HomePageViewModel viewModel;
    public HomePage(HomePageViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        BindingContext=viewModel;
        WeakReferenceMessenger.Default.Register<string>(this, async (r,m) =>
        {
            await DisplayAlert("Warning", m, "OK");
        });
    }

    private async void HomePage_OnLoaded(object? sender, EventArgs e)
    {
        await viewModel.InitializeAsync();
    }
}