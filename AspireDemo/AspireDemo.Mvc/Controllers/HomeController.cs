using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AspireDemo.Mvc.Models;

namespace AspireDemo.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly HttpClient _httpClient;

    public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient("ApiClient");
    }

    public async Task<List<WeatherForecast>> GetWeatherAsync(int maxItems = 10, CancellationToken cancellationToken = default)
    {
        List<WeatherForecast>? forecasts = new List<WeatherForecast>();


        var response = await _httpClient.GetAsync("/weatherforecast", cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            var forecastList = await response.Content.ReadFromJsonAsync<List<WeatherForecast>>(cancellationToken);

            return forecastList;
        }

        return forecasts;
    }

    public async Task<IActionResult> Index()
    {
        var forecasts = await GetWeatherAsync();
        return View(forecasts);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
