using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TrainingDiary.Models;
using TrainingDiary.Services;

namespace TrainingDiary.ViewModels;

public partial class TrainingStaticsViewModel : ObservableObject
{
    private readonly TrainingDatabase _database;

    public ObservableCollection<Training> Trainings { get; } = new();

    public TrainingStaticsViewModel(TrainingDatabase database)
    {
        _database = database;
    }

    public async Task InitializeAsync()
    {
        var trainingsFromDb = await _database.GetTrainingsAsync();

        Trainings.Clear();
        foreach (var t in trainingsFromDb)
            Trainings.Add(t);

        OnPropertyChanged(nameof(TotalDuration));
        OnPropertyChanged(nameof(MostFrequentType));
        OnPropertyChanged(nameof(MostFrequentIntensity));
        OnPropertyChanged(nameof(MostTrainedRegion));
        OnPropertyChanged(nameof(AvgDuration));
    }

    public string TotalDuration =>
        Trainings.Count == 0 ? "0 hour(s)" :
        $"{(int)Trainings.Sum(t => (t.EndTime - t.StartTime).TotalHours)} hour(s) {(Trainings.Sum(t => (t.EndTime - t.StartTime).Minutes) % 60)} minute(s)";

    public string MostFrequentType =>
        Trainings.GroupBy(t => t.Type).OrderByDescending(g => g.Count()).FirstOrDefault()?.Key ?? "-";

    public string MostFrequentIntensity =>
        Trainings.GroupBy(t => t.Intensivity).OrderByDescending(g => g.Count()).FirstOrDefault()?.Key ?? "-";

    public string MostTrainedRegion =>
        Trainings.GroupBy(t => t.TrainedRegion).OrderByDescending(g => g.Count()).FirstOrDefault()?.Key ?? "-";

    public string AvgDuration =>
        Trainings.Count == 0 ? "0 minute(s)" :
        $"{(int)Trainings.Average(t => (t.EndTime - t.StartTime).TotalMinutes)} minute(s)";
}
