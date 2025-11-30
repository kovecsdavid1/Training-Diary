using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using TrainingDiary2.Models;
using TrainingDiary2.Services;

namespace TrainingDiary2.ViewModels;

[QueryProperty(nameof(EditedTraining), "EditedTraining")]
public partial class HomePageViewModel:ObservableObject
{
    private ITrainingDatabase database;
    public ObservableCollection<Training> Trainings { get; set; }

    [ObservableProperty]
    Training selectedTraining;

    [ObservableProperty]
    private Training editedTraining;
  
    //azért kell ez, mert async nem hívható setterből
    //az [ObservableProperty] generálja az alapját, amúgy ilyen metódus nem lenne
    async partial void OnEditedTrainingChanged(Training value)
    {
        if (value != null)
        {
            if (Trainings.Any(t => t.Id == value.Id))
            {
                await database.UpdateTrainingAsync(value);
            }
            else
            {
                await database.CreateTrainingAsync(value);
            }
            await InitializeAsync();
        }
    }

    
    public HomePageViewModel(ITrainingDatabase database) { 
        this.database = database;
        Trainings= new ObservableCollection<Training>();
    }

    public async Task InitializeAsync()
    {
        var trainingList = await database.GetTrainingsAsync();
        Trainings.Clear();
        trainingList.ForEach(p=>Trainings.Add(p));
    }
    
    [RelayCommand]
    public async Task ShowStatistics()
    {
        await Shell.Current.GoToAsync("statistics");
    }

    [RelayCommand]
    public async Task ShowDetails()
    {
        if (SelectedTraining != null)
        {
            var param = new ShellNavigationQueryParameters
            {
                { "Training", SelectedTraining }
            };
            await Shell.Current.GoToAsync("trainingdetails",param);
        }
        else
        {
            WeakReferenceMessenger.Default.Send("Select a training to view details.");
        }
    }

    [RelayCommand]
    public async Task NewTrainingAsync()
    {
        SelectedTraining = null;
        var param = new ShellNavigationQueryParameters
        {
             { "Training", new Training(){Date = DateTime.Today, StartTime = DateTime.Now.TimeOfDay, EndTime = DateTime.Now.AddHours(1).TimeOfDay} }
        };
        await Shell.Current.GoToAsync("edittraining", param);
    }
    
    [RelayCommand]
    public async Task EditTrainingAsync()
    {
        if (SelectedTraining != null)
        {
            var param = new ShellNavigationQueryParameters
            {
                { "Training", SelectedTraining.GetCopy() }
            };
            await Shell.Current.GoToAsync("edittraining", param);
        }
        else
        {
            WeakReferenceMessenger.Default.Send("Select a training to edit.");
        }
    }
    
    [RelayCommand]
    public void DeleteTraining()
    {
        if (SelectedTraining != null)
        {
            database.DeleteTrainingAsync(SelectedTraining);
            Trainings.Remove(SelectedTraining);
            SelectedTraining = null;
        }
        else
        {
            WeakReferenceMessenger.Default.Send("Select a training to delete.");
        }
        
    }

    [RelayCommand]
    public async Task ShareTraining()
    {
        if (SelectedTraining != null)
        {
            if (!string.IsNullOrWhiteSpace(SelectedTraining.ImagePath) && File.Exists(SelectedTraining.ImagePath))
            {
                await Share.Default.RequestAsync(new ShareFileRequest
                {
                    Title = SelectedTraining.ToString(),
                    File = new ShareFile(SelectedTraining.ImagePath)
                });
            }
            else
            {
                await Share.Default.RequestAsync(new ShareTextRequest
                {
                    Title = "Share Training",
                    Text = SelectedTraining.ToString()
                });
            }
        }
        else
        {
            WeakReferenceMessenger.Default.Send("Select a training to share.");
        }
    }

    [RelayCommand]
    public void SelectTraining(Training training)
    {
        SelectedTraining = training;
    }
}