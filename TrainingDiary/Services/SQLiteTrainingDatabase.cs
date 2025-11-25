using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using TrainingDiary.Models;

namespace TrainingDiary.Services
{
    public class SQLiteTrainingDatabase : ITrainingDatabase
    {
        SQLite.SQLiteOpenFlags Flags =
            SQLite.SQLiteOpenFlags.ReadWrite |
            SQLite.SQLiteOpenFlags.Create;

        string databasePath =
            Path.Combine(FileSystem.Current.AppDataDirectory, "trainings.db3");

        SQLiteAsyncConnection database;

        public SQLiteTrainingDatabase()
        {
            database = new SQLiteAsyncConnection(databasePath, Flags);
            database.CreateTableAsync<Training>().Wait();
        }

        public async Task<List<Training>> GetTrainingsAsync()
        {
            return await database.Table<Training>().ToListAsync();
        }

        public async Task<Training> GetTrainingAsync(int id)
        {
            return await database.Table<Training>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateTrainingAsync(Training training)
        {
            await database.InsertAsync(training);
        }

        public async Task UpdateTrainingAsync(Training training)
        {
            await database.UpdateAsync(training);
        }

        public async Task DeleteTrainingAsync(Training training)
        {
            await database.DeleteAsync(training);

        }
    }
}
