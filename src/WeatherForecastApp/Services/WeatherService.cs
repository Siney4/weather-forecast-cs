using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using WeatherForecastApp.Interfaces;
using WeatherForecastApp.Models;
using WeatherForecastApp.Options;

namespace WeatherForecastApp.Services;

public class WeatherService : IWeatherService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AccuWeatherOptions _options;

    public WeatherService(
        IHttpClientFactory httpClientFactory,
        IOptions<AccuWeatherOptions> options)
    {
        _httpClientFactory = httpClientFactory;
        _options = options.Value;
    }

    public async Task<List<DayForecast>> GetForecastAsync(string locationKey)
    {
        var client = _httpClientFactory.CreateClient("AccuWeather");
        
        var url = $"forecasts/v1/daily/15day/{locationKey}?apikey={_options.ApiKey}&metric=true&language=ru-ru";

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