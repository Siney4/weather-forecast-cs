using WeatherForecastApp.Models;

namespace WeatherForecastApp.Interfaces;

public interface IWeatherService
{
    Task<List<DayForecast>> GetForecastAsync();
}