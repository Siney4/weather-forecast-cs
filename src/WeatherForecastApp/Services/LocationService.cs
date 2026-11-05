using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using WeatherForecastApp.Interfaces;
using WeatherForecastApp.Models;
using WeatherForecastApp.Options;

namespace WeatherForecastApp.Services;

public class LocationService : ILocationService
{
    private const int MaxResults = 5;

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AccuWeatherOptions _options;

    public LocationService(
        IHttpClientFactory httpClientFactory,
        IOptions<AccuWeatherOptions> options)
    {
        _httpClientFactory = httpClientFactory;
        _options = options.Value;
    }

    public async Task<List<LocationInfo>> SearchAsync(string cityName)
    {
        var client = _httpClientFactory.CreateClient("AccuWeather");
        
        var url = $"locations/v1/cities/search" +
                  $"?apikey={_options.ApiKey}" +
                  $"&q={Uri.EscapeDataString(cityName)}" +
                  $"&language=ru-ru";
        
        var dtos = await client.GetFromJsonAsync<List<LocationDto>>(url);

        if (dtos == null)
            return new List<LocationInfo>();

        return dtos
            .Take(MaxResults)
            .Select(d => new LocationInfo
            {
                Key = d.Key,
                Name = BuildName(d)
            })
            .ToList();
    }

    private static string BuildName(LocationDto dto)
    {
        var parts = new[]
        {
            dto.LocalizedName,
            dto.AdministrativeArea.LocalizedName,
            dto.Country.LocalizedName
        };
        
        return string.Join(", ", parts.Where(p => !string.IsNullOrWhiteSpace(p)));
    }
}