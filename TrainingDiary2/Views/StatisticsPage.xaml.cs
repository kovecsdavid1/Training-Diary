using TrainingDiary2.ViewModels;

namespace TrainingDiary2.Views;

public partial class StatisticsPage : ContentPage
{
    private readonly StatisticsPageViewModel _viewModel;

    public StatisticsPage(StatisticsPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.CalculateStatistics();
    }
}
