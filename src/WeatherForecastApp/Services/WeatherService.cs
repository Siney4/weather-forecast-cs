using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using WeatherForecastApp.Interfaces;
using WeatherForecastApp.Models;

namespace WeatherForecastApp.Services;

public class WeatherService : IWeatherService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public WeatherService(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<List<DayForecast>> GetForecastAsync()
    {
        var client = _httpClientFactory.CreateClient("AccuWeather");
        
        var apiKey = _configuration["AccuWeather:ApiKey"];

        var url = $"forecasts/v1/daily/15day/349727?apikey={apiKey}&metric=true&language=ru-ru";

        try
        {
            var dto = await client.GetFromJsonAsync<ForecastDto>(url);

            if (dto == null || dto.DailyForecasts == null)
            {
                throw new Exception("Failed to get weather forecast.");
            }

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
        catch (Exception ex)
        {
            throw new Exception("Failed to get weather forecast.", ex);
        }
    }
}