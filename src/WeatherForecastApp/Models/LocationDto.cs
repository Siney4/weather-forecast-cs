namespace WeatherForecastApp.Models;

public class LocationDto
{
    public string Key { get; set; } = "";
    public string LocalizedName { get; set; } = "";
    public AreaDto Country { get; set; } = new();
    public AreaDto AdministrativeArea { get; set; } = new();
}

public class AreaDto
{
    public string LocalizedName { get; set; } = "";
}