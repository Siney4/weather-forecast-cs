using System.Text.Json;
using WeatherForecastApp.Models;

namespace WeatherForecastApp.Services;

public class LocationSettingsService
{
    private readonly string _filePath =
        Path.Combine(AppContext.BaseDirectory, "location.json");

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true
    };

    public LocationInfo GetLocation()
    {
        if (!File.Exists(_filePath))
            return CreateDefaultLocation();

        try
        {
            string json = File.ReadAllText(_filePath);

            var location = JsonSerializer.Deserialize<LocationInfo>(json, _jsonOptions);

            if (location == null || string.IsNullOrWhiteSpace(location.Key))
                return CreateDefaultLocation();

            return location;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Ошибка настроек локации: {ex.Message}");
            return CreateDefaultLocation();
        }
    }

    public void SaveLocation(LocationInfo location)
    {
        string json = JsonSerializer.Serialize(location, _jsonOptions);

        File.WriteAllText(_filePath, json);
    }
    
    private static LocationInfo CreateDefaultLocation()
    {
        return new LocationInfo
        {
            Key = "349727",
            Name = "Локация по умолчанию"
        };
    }
}