namespace WeatherForecastApp.Models;

public class ForecastDto
{
    public List<DailyDto> DailyForecasts { get; set; } = new();
}

public class DailyDto
{
    public DateTime Date { get; set; }
    public TemperatureDto Temperature { get; set; } = new();
    public SkyConditions Day { get; set; } = new();
}

public class TemperatureDto
{ 
    public TemperatureValueDto Minimum { get; set; } = new();
    public TemperatureValueDto Maximum { get; set; } = new();
}

public class TemperatureValueDto
{
    public double Value { get; set; }
}

public class SkyConditions
{
    public string IconPhrase { get; set; } = "";
}