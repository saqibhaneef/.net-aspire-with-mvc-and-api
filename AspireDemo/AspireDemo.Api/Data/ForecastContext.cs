using Microsoft.EntityFrameworkCore;
namespace AspireDemo.Api.Data
{    
    public class ForecastContext : DbContext
    {
        public ForecastContext(DbContextOptions<ForecastContext> options) : base(options)
        {
        }

        public DbSet<WeatherForecast> WeatherForecasts => Set<WeatherForecast>();
        

        protected override void OnModelCreating(ModelBuilder bldr)
        {
            base.OnModelCreating(bldr);            
        }


    }

}
