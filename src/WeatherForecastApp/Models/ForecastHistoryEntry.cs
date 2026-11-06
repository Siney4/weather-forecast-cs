using WeatherForecastApp.Models;

namespace WeatherForecastApp.Models;

public class ForecastHistoryEntry
{
    public DateTime RequestedAt { get; set; }

    public string RequestType { get; set; } = "";

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public List<DayForecast> Results { get; set; } = new();
}