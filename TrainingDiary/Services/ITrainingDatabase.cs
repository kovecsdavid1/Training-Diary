using TrainingDiary.Models;

namespace TrainingDiary.Services;

public interface ITrainingDatabase
{
    Task<List<Training>> GetTrainingsAsync();
    Task<Training> GetTrainingAsync(int id);
    Task CreateTrainingAsync(Training training);
    Task UpdateTrainingAsync(Training training);
    Task DeleteTrainingAsync(Training training);
}