using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.IO;
using TrainingDiary2.Models;
using Microsoft.Maui.Storage;

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

    [RelayCommand]
    private async Task AddPhoto()
    {
        try
        {
            var action = await Application.Current.MainPage.DisplayActionSheet("Add Photo", "Cancel", null, "Take Photo", "Choose from Gallery");

            if (action == "Take Photo")
            {
                if (MediaPicker.Default.IsCaptureSupported)
                {
                    PermissionStatus status = await Permissions.RequestAsync<Permissions.Camera>();
                    if (status != PermissionStatus.Granted)
                    {
                        await Application.Current.MainPage.DisplayAlert("Permission Denied", "Camera permission is required to take a photo.", "OK");
                        return;
                    }

                    FileResult photo = await MediaPicker.Default.CapturePhotoAsync();
                    if (photo != null)
                    {
                        string localFilePath = await SaveFile(photo);
                        Draft.ImagePath = localFilePath;
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Not Supported", "Your device does not support taking photos.", "OK");
                }
            }
            else if (action == "Choose from Gallery")
            {
                PermissionStatus status = await Permissions.RequestAsync<Permissions.StorageRead>();
                if (status != PermissionStatus.Granted)
                {
                    await Application.Current.MainPage.DisplayAlert("Permission Denied", "Storage permission is required to choose a photo.", "OK");
                    return;
                }

                FileResult photo = await MediaPicker.Default.PickPhotoAsync();
                if (photo != null)
                {
                    string localFilePath = await SaveFile(photo);
                    Draft.ImagePath = localFilePath;
                }
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

    private async Task<string> SaveFile(FileResult fileResult)
    {
        string localFolderPath = Path.Combine(FileSystem.AppDataDirectory, "TrainingImages");
        if (!Directory.Exists(localFolderPath))
        {
            Directory.CreateDirectory(localFolderPath);
        }

        string localFilePath = Path.Combine(localFolderPath, $"{Guid.NewGuid()}{Path.GetExtension(fileResult.FileName)}");

        using (var sourceStream = await fileResult.OpenReadAsync())
        using (var localFileStream = File.OpenWrite(localFilePath))
        {
            await sourceStream.CopyToAsync(localFileStream);
        }
        
        return localFilePath;
    }
}