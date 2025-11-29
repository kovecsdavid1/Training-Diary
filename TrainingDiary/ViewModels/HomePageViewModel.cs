using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using TrainingDiary.Models;
using TrainingDiary.Services;

namespace TrainingDiary.ViewModels;

[QueryProperty(nameof(EditedTraining), "EditedTraining")]
public partial class HomePageViewModel : ObservableObject
{
    private readonly TrainingDatabase _database;

    public ObservableCollection<Training> Trainings { get; set; }

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

    [ObservableProperty]
    private Training selectedTraining;

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
    private DateTime startDate;

    [ObservableProperty]
    private TimeSpan startTime;

    [ObservableProperty]
    private DateTime endDate;

    [ObservableProperty]
    private TimeSpan endTime;

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

    private bool _isFullViewEnabled = true;

    public bool IsFullViewEnabled
    {
        get => _isFullViewEnabled;
        set
        {
            if (_isFullViewEnabled != value)
            {
                _isFullViewEnabled = value;
                OnPropertyChanged();
            }
        }
    }
    public HomePageViewModel(TrainingDatabase database)
    {
        _database = database;
        Trainings = new ObservableCollection<Training>();
    }

    public async Task InitializeAsync()
    {
        Trainings.Clear();
        var trainingList = await _database.GetTrainingsAsync();
        foreach (var t in trainingList)
            Trainings.Add(t);
    }

    [RelayCommand]
    public async Task ShowTrainingStaticsAsync()
    {
        await Shell.Current.GoToAsync("trainingstatics");
    }

    [RelayCommand]
    public async Task NewTrainingAsync()
    {
        await Shell.Current.GoToAsync("edittraining");
    }

    //[RelayCommand]
    //public async Task NewTrainingAsync()
    //{
    //    SelectedTraining = null;

    //    var param = new ShellNavigationQueryParameters
    //    {
    //        { "Training", new Training
    //            {
    //                StartTime = DateTime.Now,
    //                EndTime = DateTime.Now.AddHours(1)
    //            }
    //        }
    //    };

    //await Shell.Current.GoToAsync("edittraining", param);
    //}

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
            await _database.DeleteTrainingAsync(SelectedTraining);
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
        if (value == null)
            return;

        // ha szerkesztettünk egy meglévőt
        if (SelectedTraining != null)
        {
            Trainings.Remove(SelectedTraining);
            SelectedTraining = null;
        }

        // Save (insert vagy update az Id alapján)
        await _database.SaveTrainingAsync(value);

        // lista frissítése
        Trainings.Add(value);
    }
}
