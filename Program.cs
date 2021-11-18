using Microsoft.EntityFrameworkCore;
using MinimalApiDemo;
using MinimalApiDemo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IWeatherService, WeatherService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseSqlite(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/weatherforecast/{id}", (HttpContext _, IWeatherService weatherService, string id) =>
    {
        var result = weatherService.GetWeatherForecasts(id);
        return result != null ? Results.Ok(result) : Results.NotFound();
    })
    .WithName("GetWeatherForecastById");

app.MapGet("/allweatherforecast", (HttpContext _, IWeatherService weatherService) => weatherService.GetAllWeatherForecasts())
    .WithName("GetAllWeatherForecast");

app.MapPost("/weatherforecast", (HttpContext _, IWeatherService weatherService, string id) =>
{
    weatherService.Add(id);
    return Results.Created($"/weatherforecast/{id}", id);
}).WithName("CreateWeatherForecast");

app.MapPut("/weatherforecast/{id}", (HttpContext _, IWeatherService weatherService, string id, int newTemperature) =>
{
    if (weatherService.GetWeatherForecasts(id) == null)
    {
        return Results.NotFound();
    }

    weatherService.Update(id, newTemperature);
    return Results.Ok(newTemperature);
}).WithName("UpdateWeatherForecast");

app.MapDelete("/weatherforecast/{id}", (HttpContext _, IWeatherService weatherService, string id) =>
{
    if (weatherService.GetWeatherForecasts(id) == null)
    {
        return Results.NotFound();
    }

    weatherService.Delete(id);
    return Results.Ok(id);
}).WithName("DeleteWeatherForecast");

app.Run();