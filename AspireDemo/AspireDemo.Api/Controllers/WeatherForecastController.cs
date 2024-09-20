using AspireDemo.Api.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace AspireDemo.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{  
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly RabbitMQ.Client.IConnection connection;
    private readonly ForecastContext forecastContext;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, RabbitMQ.Client.IConnection connection, ForecastContext forecastContext)
    {
        _logger = logger;
        this.connection = connection;
        this.forecastContext = forecastContext;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        
        var forcasts=this.forecastContext.WeatherForecasts.ToList();

        CreateMessage(this.connection);

        return forcasts;
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
