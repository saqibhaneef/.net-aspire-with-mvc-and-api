using AspireDemo.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

namespace AspireDemo.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{  
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly RabbitMQ.Client.IConnection connection;
    private readonly ForecastContext forecastContext;
    private readonly IDistributedCache distributedCache;
    public WeatherForecastController(ILogger<WeatherForecastController> logger, RabbitMQ.Client.IConnection connection, ForecastContext forecastContext, IDistributedCache distributedCache)
    {
        _logger = logger;
        this.connection = connection;
        this.forecastContext = forecastContext;
        this.distributedCache = distributedCache;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        var cachedForecast = await distributedCache.GetAsync("forecast");

        if (cachedForecast is null)
        {
            var forcasts =await this.forecastContext.WeatherForecasts.ToListAsync();

            await distributedCache.SetAsync("forecast", Encoding.UTF8.GetBytes(JsonSerializer.Serialize(forcasts)), new()
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(10)
            });

            CreateMessage(this.connection);

            return forcasts;
        }
                

        CreateMessage(this.connection);

        return JsonSerializer.Deserialize<IEnumerable<WeatherForecast>>(cachedForecast);
    }

    private void CreateMessage(RabbitMQ.Client.IConnection connection)
    {
        var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "forecastEvents",
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

        var body = Encoding.UTF8.GetBytes($"Getting all items in the forecast.{DateTime.Now}");

        channel.BasicPublish(exchange: string.Empty,
                             routingKey: "forecastEvents",
                              mandatory: false,
                             basicProperties: null,
                             body: body);
    }
}
