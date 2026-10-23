namespace WeatherForecastApp.Models;

public class ForecastDto
{
    public List<DailyDto> DailyForecasts { get; set; } = new();
}

public class DailyDto
{
    public DateTime Date { get; set; }
    public TempDto Temperature { get; set; } = new();
    public PartDto Day { get; set; } = new();
}

public class TempDto
{
    public ValueDto Minimum { get; set; } = new();
    public ValueDto Maximum { get; set; } = new();
}

public class ValueDto
{
    public double Value { get; set; }
}

public class PartDto
{
    public string IconPhrase { get; set; } = "";
}