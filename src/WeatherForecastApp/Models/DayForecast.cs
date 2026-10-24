namespace WeatherForecastApp.Models;

public class DayForecast
{
    public DateOnly Date { get; init; }
    public double MinimumTemperature { get; init; }
    public double MaximumTemperature { get; init; }
    public string SkyConditions { get; init; } = "";
}