namespace MinimalApiDemo.Services
{
    public class WeatherService
    {
        private List<string> _summaries = new() { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };

        public WeatherForecast[] GetWeatherForecasts()
        {
            var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    (
                        DateTime.Now.AddDays(index),
                        Random.Shared.Next(-20, 55),
                        _summaries[Random.Shared.Next(_summaries.Count)]
                    ))
                .ToArray();
            return forecast;
        }

        public WeatherForecast[] GetAllWeatherForecasts()
        {
            var forecast = Enumerable.Range(1, _summaries.Count).Select(index =>
                    new WeatherForecast
                    (
                        DateTime.Now.AddDays(index),
                        Random.Shared.Next(-20, 55),
                        _summaries[Random.Shared.Next(_summaries.Count)]
                    ))
                .ToArray();
            return forecast;
        }

        public string? GetWeatherForecasts(string id)
        {
            return _summaries.FirstOrDefault(x => x == id);
        }

        public void Add(string id)
        {
            _summaries.Add(id);
        }

        public void Update(string id, string newId)
        {
            int index = _summaries.FindIndex(s => s == id);

            if (index != -1)
                _summaries[index] = newId;
        }

        public void Delete(string id)
        {
            _summaries.RemoveAll(x => x == id);
        }
    }

    public record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}
