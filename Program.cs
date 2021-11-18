using MinimalApiDemo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(new WeatherService());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/weatherforecast", (HttpContext _, WeatherService weatherService) => weatherService.GetWeatherForecasts())
    .WithName("GetWeatherForecast");

app.MapGet("/weatherforecast/{id}", (HttpContext _, WeatherService weatherService, string id) =>
    {
        var result = weatherService.GetWeatherForecasts(id);
        return result != null ? Results.Ok(result) : Results.NotFound();
    })
    .WithName("GetWeatherForecastById");

app.MapGet("/allweatherforecast", (HttpContext _, WeatherService weatherService) => weatherService.GetAllWeatherForecasts())
    .WithName("GetAllWeatherForecast");

app.MapPost("/weatherforecast", (HttpContext _, WeatherService weatherService, string id) =>
{
    weatherService.Add(id);
    return Results.Created($"/weatherforecast/{id}", id);
}).WithName("CreateWeatherForecast");

app.MapPut("/weatherforecast/{id}", (HttpContext _, WeatherService weatherService, string id, string newId) =>
{
    if (weatherService.GetWeatherForecasts(id) == null)
    {
        return Results.NotFound();
    }

    weatherService.Update(id, newId);
    return Results.Ok(newId);
}).WithName("UpdateWeatherForecast");

app.MapDelete("/weatherforecast/{id}", (HttpContext _, WeatherService weatherService, string id) =>
{
    if (weatherService.GetWeatherForecasts(id) == null)
    {
        return Results.NotFound();
    }

    weatherService.Delete(id);
    return Results.Ok(id);
}).WithName("DeleteWeatherForecast");

app.Run();