using TrainingDiary.ViewModels;


namespace TrainingDiary.Views;


public partial class EditTrainingPage : ContentPage
{
    private readonly EditTrainingPageViewModel _vm;

    public EditTrainingPage(EditTrainingPageViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        _vm.InitDraft();
    }
}
