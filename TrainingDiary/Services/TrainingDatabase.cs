using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using TrainingDiary.Models;

namespace TrainingDiary.Services
{
    public class TrainingDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        public TrainingDatabase(string? dbPath = null)
        {
            if (string.IsNullOrWhiteSpace(dbPath))
            {
#if WINDOWS
        var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        dbPath = Path.Combine(folder, "trainings.db3");
#else
                dbPath = Path.Combine(FileSystem.AppDataDirectory, "trainings.db3");
#endif
            }

            var flags = SQLiteOpenFlags.ReadWrite |
                        SQLiteOpenFlags.Create |
                        SQLiteOpenFlags.SharedCache;

            _database = new SQLiteAsyncConnection(dbPath, flags);
            _database.CreateTableAsync<Training>().Wait();
        }

        public Task<List<Training>> GetTrainingsAsync()
        {
            return _database.Table<Training>().ToListAsync();
        }

        public Task<Training> GetTrainingAsync(int id)
        {
            return _database.Table<Training>()
                            .Where(t => t.Id == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveTrainingAsync(Training training)
        {
            if (training.Id != 0)
                return _database.UpdateAsync(training);
            else
                return _database.InsertAsync(training);
        }

        public Task<int> DeleteTrainingAsync(Training training)
        {
            return _database.DeleteAsync(training);
        }
    }
}