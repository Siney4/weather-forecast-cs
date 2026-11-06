namespace WeatherForecastApp.Options;

public class AccuWeatherOptions
{
    public const string SectionName = "AccuWeather";

    public string BaseUrl { get; set; } = "";
    public string ApiKey { get; set; } = "";
}