using WeatherForecastApp.Models;

namespace WeatherForecastApp.Interfaces;

public interface IWeatherService
{
    public Task<List<DayForecast>> GetForecastAsync();
}