using System.Text.Json;
using WeatherForecastApp.Interfaces;
using WeatherForecastApp.Models;

namespace WeatherForecastApp.Services;

public class WeatherService : IWeatherService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public WeatherService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<DayForecast>> GetForecastAsync()
    {
        var client = _httpClientFactory.CreateClient("AccuWeather");
        
        var url = "forecasts/v1/daily/15day/349727?apikey=&metric=true&language=ru-ru";

        var json = await client.GetStringAsync(url);
        
        var dto = JsonSerializer.Deserialize<ForecastDto>(json)!; 

        var result = new List<DayForecast>();                  
        foreach (var d in dto.DailyForecasts)
        {
            result.Add(new DayForecast
            {
                Date = DateOnly.FromDateTime(d.Date),
                MinimumTemperature = d.Temperature.Minimum.Value,
                MaximumTemperature = d.Temperature.Maximum.Value,
                SkyConditions = d.Day.IconPhrase
            });
        }
        return result;
    }
}