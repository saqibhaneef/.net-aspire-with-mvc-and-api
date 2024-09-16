using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace AspireDemo.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly RabbitMQ.Client.IConnection connection;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, RabbitMQ.Client.IConnection connection)
    {
        _logger = logger;
        this.connection = connection;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        var forcasts= Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();

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
