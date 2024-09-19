namespace AspireDemo.Api.Data.Seeder
{
    public static class SeedData
    {
        public static IEnumerable<WeatherForecast> GetProducts()
        {
            return new List<WeatherForecast>
            {
                new WeatherForecast()
                {
                    Date=DateOnly.MaxValue,
                    Summary="Summary 1",
                    TemperatureC=32,                    
                },
                new WeatherForecast()
                {
                    Date=DateOnly.MaxValue,
                    Summary="Summary 2",
                    TemperatureC=30,
                },
                new WeatherForecast()
                {
                    Date=DateOnly.MaxValue,
                    Summary="Summary 3",
                    TemperatureC=38,
                },
                new WeatherForecast()
                {
                    Date=DateOnly.MaxValue,
                    Summary="Summary 4",
                    TemperatureC=25,
                }
            };
        }        
    }
}
