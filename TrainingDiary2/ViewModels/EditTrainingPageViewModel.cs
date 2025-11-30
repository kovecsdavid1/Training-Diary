using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TrainingDiary2.Models;

namespace TrainingDiary2.ViewModels;

[QueryProperty(nameof(EditedTraining), "Training")]
public partial class EditTrainingPageViewModel:ObservableObject
{
    [ObservableProperty]
    Training editedTraining;

    [ObservableProperty]
    Training draft;

    public ObservableCollection<string> TrainedRegions { get; } = new()
    {
        "Chest", "Back", "Legs", "Arms", "Shoulders", "Core"
    };

    public ObservableCollection<string> TrainingTypes { get; } = new()
    {
        "Strength", "Cardio", "Flexibility", "Balance"
    };

    public ObservableCollection<string> Intensivities { get; } = new()
    {
        "easy", "medium", "hard", "progressive"
    };

    public void InitDraft()
    {
        Draft = EditedTraining.GetCopy();
    }
    
    [RelayCommand]
    public async Task SaveTraining()
    {
        var param = new ShellNavigationQueryParameters
        {
            { "EditedTraining", Draft }
        };
        await Shell.Current.GoToAsync("..", param);
    }

    [RelayCommand]
    public async Task CancelEdit()
    {
        await Shell.Current.GoToAsync("..");
    }
}