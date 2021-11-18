using Microsoft.EntityFrameworkCore;
using MinimalApiDemo.Services;

namespace MinimalApiDemo
{
    public class ApiDbContext : DbContext
    {
        public virtual DbSet<WeatherForecast> WeatherForecasts { get; set; }

        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }
    }
}
