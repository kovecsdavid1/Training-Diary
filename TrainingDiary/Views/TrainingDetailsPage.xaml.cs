using TrainingDiary.Models;

namespace TrainingDiary.Views;

[QueryProperty(nameof(Training), "Training")]
public partial class TrainingDetailsPage : ContentPage
{
    Training training;
    public Training Training
    {
        get => training;
        set
        {
            training = value;
            BindingContext = training;
            OnPropertyChanged();
        }
    }
    public TrainingDetailsPage()
    {
        InitializeComponent();
    }
}