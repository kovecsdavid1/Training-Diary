using TrainingDiary2.ViewModels;

namespace TrainingDiary2.Views;


public partial class EditTrainingPage : ContentPage
{
    private EditTrainingPageViewModel VM;
    public EditTrainingPage(EditTrainingPageViewModel VM)
    {
        InitializeComponent();
        this.VM = VM;
        BindingContext=VM;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    { 
       base.OnNavigatedTo(args);
       VM.InitDraft();
    }


}