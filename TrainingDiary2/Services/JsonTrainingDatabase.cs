using System.Text.Json;
using CommunityToolkit.Mvvm.Messaging;
using TrainingDiary2.Models;

namespace TrainingDiary2.Services;

public class JsonTrainingDatabase:ITrainingDatabase
{
    string filePath=Path.Combine(FileSystem.Current.AppDataDirectory, "trainings.json");
    
    public async Task<List<Training>> GetTrainingsAsync()
    {
        if (File.Exists(filePath))
        {
            string jsonString = await File.ReadAllTextAsync(filePath);
                if (!string.IsNullOrEmpty(jsonString))
                {
                    return JsonSerializer.Deserialize<List<Training>>(jsonString);
                }
                else
                {
                    return new List<Training>();
                }
        }
        else return new List<Training>();
    }

    public async Task<Training> GetTrainingAsync(int id)
    {
        var trainings = await GetTrainingsAsync();
        return trainings.FirstOrDefault(p => p.Id == id);
    }

    public async Task CreateTrainingAsync(Training training)
    {
        var trainings = await GetTrainingsAsync();
        training.Id = trainings.Count==0 ? 1 : (trainings.Max(p => p.Id) + 1);
        trainings.Add(training);
        await WriteTrainingsAsync(trainings);
    }

    public async Task UpdateTrainingAsync(Training training)
    {
        var trainings = await GetTrainingsAsync();
        var index = trainings.FindIndex(p => p.Id == training.Id);
        if (index != -1)
        {
            trainings[index] = training;
            await WriteTrainingsAsync(trainings);
        }
    }

    public async Task DeleteTrainingAsync(Training training)
    {
        var trainings = await GetTrainingsAsync();
        trainings.RemoveAll(p=>p.Id==training.Id);
        await WriteTrainingsAsync(trainings);
    }
    
    private async Task WriteTrainingsAsync(List<Training> trainings)
    {
        try
        {
            string jsonString = JsonSerializer.Serialize(trainings);
            await File.WriteAllTextAsync(filePath, jsonString);
        }
        catch (Exception e)
        {
            WeakReferenceMessenger.Default.Send("File write error: "+ e.Message);
        }
    }
}