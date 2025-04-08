using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherTracker.Models;

namespace WeatherTracker.Services
{
    public static class DatabaseSQL
    {
        private static SQLiteAsyncConnection _db;

        static async Task Init()
        {
            if (_db != null) return;
            {
                var databasePath = Path.Combine(FileSystem.AppDataDirectory, "WeatherTracker.db");

                _db = new SQLiteAsyncConnection(databasePath);

                await _db.CreateTableAsync<FavoriteCity>();
            }
        }

        public static async Task AddCity(string cityName, string optionalNotes)
        {
            await Init();

            var favoriteCity = new FavoriteCity
            {
                CityName = cityName,
                OptionalNotes = optionalNotes
            };

            await _db.InsertAsync(favoriteCity);
        }

        public static async Task EditCity(int id, string cityName, string optionalNotes)
        {
            await Init();

            var editCity = new FavoriteCity
            {
                Id = id,
                CityName = cityName,
                OptionalNotes = optionalNotes
            };

            await _db.UpdateAsync(editCity);
        }

        public static async Task RemoveCity(int id)
        {
            await Init();

            await _db.DeleteAsync<FavoriteCity>(id);
        }

        public static async Task<IEnumerable<FavoriteCity>> GetFavoriteCities()
        {
            await Init();

            var cities = await _db.Table<FavoriteCity>().ToListAsync();

            return cities;
        }
    }
}
