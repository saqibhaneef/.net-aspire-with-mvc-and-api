using AspireDemo.Api.Data.Seeder;
using Microsoft.EntityFrameworkCore;
using EFCore.BulkExtensions;

namespace AspireDemo.Api.Data.Seeding
{
    public class Seeder(ForecastContext forecastContext)
    {
        public void Seed()
        {
            if (!forecastContext.WeatherForecasts.Any())
            {
                var strategy = forecastContext.Database.CreateExecutionStrategy();
                forecastContext.WeatherForecasts.AddRange(SeedData.GetProducts());

                strategy.Execute(() =>
                {
                    forecastContext.BulkSaveChanges();
                });
            }
        }
    }
}
