using System.Text.Json;
using WeatherForecastApp.Interfaces;
using WeatherForecastApp.Models;

namespace WeatherForecastApp.Services;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _client;

    public WeatherService(HttpClient client)
    {
        _client = client;
    }

    public async Task<List<DayForecast>> GetForecastAsync()
    {
        
        
        var url = "https://dataservice.accuweather.com/forecasts/v1/daily/15day/349727?apikey=&metric=true&language=ru-ru";

        var json = await _client.GetStringAsync(url);          
        var dto = JsonSerializer.Deserialize<ForecastDto>(json)!; 

        var result = new List<DayForecast>();                  
        foreach (var d in dto.DailyForecasts)
        {
            result.Add(new DayForecast(
                DateOnly.FromDateTime(d.Date),
                d.Temperature.Minimum.Value,
                d.Temperature.Maximum.Value,
                d.Day.IconPhrase));
        }

        return result;
    }
}