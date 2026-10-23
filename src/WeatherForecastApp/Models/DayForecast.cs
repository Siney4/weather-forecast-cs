namespace WeatherForecastApp.Models;

public class DayForecast
{
    public DateOnly Date { get; }
    public double MinTemp { get; }
    public double MaxTemp { get; }
    public string Description { get; }

    public DayForecast(DateOnly date, double minTemp, double maxTemp, string description)
    {
        Date = date;
        MinTemp = minTemp;
        MaxTemp = maxTemp;
        Description = description;
    }
}