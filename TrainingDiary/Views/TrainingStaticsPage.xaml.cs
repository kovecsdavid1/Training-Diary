using TrainingDiary.ViewModels;


namespace TrainingDiary.Views;

public partial class TrainingStaticsPage : ContentPage
{
    private TrainingStaticsViewModel ViewModel;

    public TrainingStaticsPage(TrainingStaticsViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = new TrainingStaticsViewModel(App.Database);
        BindingContext = ViewModel;
    }

    private async void OnPageLoaded(object sender, EventArgs e)
    {
        await ViewModel.InitializeAsync();
    }
}