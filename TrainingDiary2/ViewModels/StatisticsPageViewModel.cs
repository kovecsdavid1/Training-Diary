using CommunityToolkit.Mvvm.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using TrainingDiary2.Models;
using TrainingDiary2.Services;

namespace TrainingDiary2.ViewModels
{
    public partial class StatisticsPageViewModel : ObservableObject
    {
        private readonly ITrainingDatabase _database;

        [ObservableProperty]
        private string averageDuration;

        [ObservableProperty]
        private string mostFrequentRegion;

        [ObservableProperty]
        private string mostFrequentType;

        [ObservableProperty]
        private string mostFrequentIntensivity;

        [ObservableProperty]
        private Training longestTraining;

        public StatisticsPageViewModel(ITrainingDatabase database)
        {
            _database = database;
        }

        public async Task CalculateStatistics()
        {
            var trainings = await _database.GetTrainingsAsync();

            if (trainings == null || !trainings.Any())
            {
                AverageDuration = "No data";
                MostFrequentRegion = "No data";
                MostFrequentType = "No data";
                MostFrequentIntensivity = "No data";
                LongestTraining = null;
                return;
            }

            var totalDuration = trainings.Aggregate(TimeSpan.Zero, (total, training) => total + training.Duration);
            AverageDuration = (totalDuration / trainings.Count).ToString(@"hh\:mm");

            MostFrequentRegion = trainings.GroupBy(t => t.TrainedRegion)
                                          .OrderByDescending(g => g.Count())
                                          .Select(g => g.Key)
                                          .FirstOrDefault();

            MostFrequentType = trainings.GroupBy(t => t.Type)
                                        .OrderByDescending(g => g.Count())
                                        .Select(g => g.Key)
                                        .FirstOrDefault();

            MostFrequentIntensivity = trainings.GroupBy(t => t.Intensivity)
                                               .OrderByDescending(g => g.Count())
                                               .Select(g => g.Key)
                                               .FirstOrDefault();

            LongestTraining = trainings.OrderByDescending(t => t.Duration).FirstOrDefault();
        }
    }
}
