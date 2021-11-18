using System.ComponentModel.DataAnnotations;

namespace MinimalApiDemo.Services
{
    public interface IWeatherService
    {
        WeatherForecast[] GetAllWeatherForecasts();
        WeatherForecast? GetWeatherForecasts(string id);
        void Add(string id);
        void Update(string id, int newTemperatureC);
        void Delete(string id);
    }

    public class WeatherService : IWeatherService
    {
        private readonly ApiDbContext _db;

        public WeatherService(ApiDbContext db)
        {
            _db = db;
        }

        public WeatherForecast[] GetAllWeatherForecasts()
        {
            return _db.WeatherForecasts.ToArray();
        }

        public WeatherForecast? GetWeatherForecasts(string id)
        {
            return _db.WeatherForecasts.FirstOrDefault(x => x.Summary == id);
        }

        public async void Add(string id)
        {
            _db.WeatherForecasts.Add(new WeatherForecast(DateTime.UtcNow, 0, id));
            await _db.SaveChangesAsync();
        }

        public async void Update(string id, int newTemperatureC)
        {
            var existingItem = _db.WeatherForecasts.FirstOrDefault(x => x.Summary == id);

            if (existingItem != null)
                existingItem.TemperatureC = newTemperatureC;
            await _db.SaveChangesAsync();

        }

        public async void Delete(string id)
        {
            var existingItem = _db.WeatherForecasts.FirstOrDefault(x => x.Summary == id);

            if (existingItem != null)
                _db.WeatherForecasts.Remove(existingItem);
            await _db.SaveChangesAsync();
        }
    }

    public class WeatherForecast
    {
        [Key]
        public string Summary { get; set; }
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public WeatherForecast(DateTime date, int temperatureC, string? summary)
        {
            Date = date;
            TemperatureC = temperatureC;
            Summary = summary;
        }
    }
}
