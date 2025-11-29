using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TrainingDiary.Models;

namespace TrainingDiary.ViewModels
{
    [QueryProperty(nameof(EditedTraining), "Training")]
    public partial class EditTrainingPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private Training editedTraining;

        [ObservableProperty]
        private Training draft;

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
        private DateTime startDate;

        [ObservableProperty]
        private TimeSpan startTime;

        [ObservableProperty]
        private DateTime endDate;

        [ObservableProperty]
        private TimeSpan endTime;

        public void InitDraft()
        {
            if (EditedTraining != null)
            {
                Draft = EditedTraining.GetCopy();

                StartDate = Draft.StartTime.Date;
                StartTime = Draft.StartTime.TimeOfDay;
                EndDate = Draft.EndTime.Date;
                EndTime = Draft.EndTime.TimeOfDay;
            }
            else
            {
                Draft = new Training
                {
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now.AddHours(1)
                };

                StartDate = Draft.StartTime.Date;
                StartTime = Draft.StartTime.TimeOfDay;
                EndDate = Draft.EndTime.Date;
                EndTime = Draft.EndTime.TimeOfDay;
            }
        }

        partial void OnDraftChanged(Training value)
        {
            if (value != null)
            {
                StartDate = value.StartTime.Date;
                StartTime = value.StartTime.TimeOfDay;
                EndDate = value.EndTime.Date;
                EndTime = value.EndTime.TimeOfDay;
            }
        }

        [RelayCommand]
        public async Task SaveTraining()
        {
            // dátum + idő visszaírása Draftba
            Draft.StartTime = StartDate.Date + StartTime;
            Draft.EndTime = EndDate.Date + EndTime;

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
}
