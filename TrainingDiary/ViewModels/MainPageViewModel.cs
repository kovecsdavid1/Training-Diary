using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using TrainingDiary.Models;
using TrainingDiary.Services;

namespace TrainingDiary.ViewModels;

[QueryProperty(nameof(EditedTraining), "EditedTraining")]
public partial class MainPageViewModel : ObservableObject
{
    private ITrainingDatabase database;

    public ObservableCollection<Training> Trainings { get; set; }

    public ObservableCollection<string> TrainedRegions { get; } = new ObservableCollection<string>
{
    "Chest", "Back", "Legs", "Arms", "Shoulders", "Core"
};

    public ObservableCollection<string> TrainingTypes { get; } = new ObservableCollection<string>
{
    "Strength", "Cardio", "Flexibility", "Balance"
};

    public ObservableCollection<string> Intensivities { get; } = new ObservableCollection<string>
{
    "easy", "medium", "hard", "progressive"
};

    [ObservableProperty]
    Training selectedTraining;

    partial void OnSelectedTrainingChanged(Training value)
    {
        if (value != null)
        {
            StartDate = value.StartTime.Date;
            StartTime = value.StartTime.TimeOfDay;
            EndDate = value.EndTime.Date;
            EndTime = value.EndTime.TimeOfDay;
        }
        OnPropertyChanged(nameof(SelectedTrainingDuration));
    }

    [ObservableProperty]
    private Training editedTraining;

    [ObservableProperty]
    DateTime startDate;

    [ObservableProperty]
    TimeSpan startTime;

    [ObservableProperty]
    DateTime endDate;

    [ObservableProperty]
    TimeSpan endTime;

    public string SelectedTrainingDuration
    {
        get
        {
            if (SelectedTraining == null)
                return string.Empty;
            TimeSpan duration = SelectedTraining.EndTime - SelectedTraining.StartTime;
            if (duration.TotalSeconds < 0)
                return "0 hour(s) 0 minute(s)";
            return $"Duration: {(int)duration.TotalHours} hour(s) {duration.Minutes} minute(s)";
        }
    }

    public bool IsFullViewEnabled
    {
        get => Preferences.Default.Get("fullview", true);
        set
        {
            Preferences.Default.Set("fullview", value);
            OnPropertyChanged();
        }
    }

    public MainPageViewModel(ITrainingDatabase database)
    {
        this.database = database;
        Trainings = new ObservableCollection<Training>();
        Trainings.Add(new Training()
        {
            Name = "Morning Cardio",
            TrainedRegion = "Legs",
            Type = "Cardio",
            Intensivity = "medium",
            StartTime = new DateTime(2025, 11, 25, 7, 0, 0),
            EndTime = new DateTime(2025, 11, 25, 8, 0, 0)
        });


        Trainings.Add(new Training()
        {
            Name = "Evening Strength",
            TrainedRegion = "Arms",
            Type = "Strength",
            Intensivity = "hard",
            StartTime = new DateTime(2025, 11, 25, 18, 30, 0),
            EndTime = new DateTime(2025, 11, 25, 20, 0, 0)
        });

    }

    public async Task InitializeAsync()
    {
        var trainingList = await database.GetTrainingsAsync();
        trainingList.ForEach(p => Trainings.Add(p));
    }

    [RelayCommand]
    public async Task ShowTrainingStaticsAsync()
    {
        await Shell.Current.GoToAsync("///trainingstatics");
    }


    [RelayCommand]
    public async Task NewTrainingAsync()
    {
        SelectedTraining = null;
        var param = new ShellNavigationQueryParameters
    {
        { "Training", new Training(){ StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1)} }
    };
        await Shell.Current.GoToAsync("edittraining", param);
    }

    [RelayCommand]
    public async Task EditTrainingAsync()
    {
        if (SelectedTraining != null)
        {
            SelectedTraining.StartTime = StartDate.Date + StartTime;
            SelectedTraining.EndTime = EndDate.Date + EndTime;

            var param = new ShellNavigationQueryParameters
        {
            { "Training", SelectedTraining }
        };
            await Shell.Current.GoToAsync("edittraining", param);
        }
        else
        {
            WeakReferenceMessenger.Default.Send("Select a training to edit.");
        }
    }

    [RelayCommand]
    public async Task DeleteTrainingAsync()
    {
        if (SelectedTraining != null)
        {
            await database.DeleteTrainingAsync(SelectedTraining);
            Trainings.Remove(SelectedTraining);
            SelectedTraining = null;
        }
        else
        {
            WeakReferenceMessenger.Default.Send("Select a training to delete.");
        }
    }

    async partial void OnEditedTrainingChanged(Training value)
    {
        if (value != null)
        {
            if (SelectedTraining != null)
            {
                Trainings.Remove(SelectedTraining);
                SelectedTraining = null;
                await database.UpdateTrainingAsync(value);
            }
            else
            {
                await database.CreateTrainingAsync(value);
            }
            Trainings.Add(value);
        }
    }
}
