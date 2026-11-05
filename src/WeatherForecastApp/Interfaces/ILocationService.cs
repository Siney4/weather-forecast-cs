using WeatherForecastApp.Models;
 
namespace WeatherForecastApp.Interfaces;
 
public interface ILocationService
{
    public Task<List<LocationInfo>> SearchAsync(string cityName);
}