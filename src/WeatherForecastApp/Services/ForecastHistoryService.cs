using System.Text.Json;
using WeatherForecastApp.Models;

namespace WeatherForecastApp.Services;

public class ForecastHistoryService
{
    private readonly string _filePath =
        Path.Combine(AppContext.BaseDirectory, "forecast-history.json");

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true
    };

    public List<ForecastHistoryEntry> GetHistory()
    {
        if (!File.Exists(_filePath))
            return new List<ForecastHistoryEntry>();

        try
        {
            string json = File.ReadAllText(_filePath);

            var history = JsonSerializer.Deserialize<List<ForecastHistoryEntry>>(
                json, _jsonOptions);

            return history?
                .Where(entry => entry != null)
                .Select(entry =>
                {
                    entry.Results ??= new List<DayForecast>();
                    entry.RequestType ??= "";
                    return entry;
                })
                .ToList() ?? new List<ForecastHistoryEntry>();
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Ошибка чтения JSON: {ex.Message}");
            return new List<ForecastHistoryEntry>();
        }
    }

    public void AddEntry(
        string requestType,
        List<DayForecast> results,
        DateOnly? startDate = null,
        DateOnly? endDate = null)
    {
        var history = GetHistory();

        history.Add(new ForecastHistoryEntry
        {
            RequestedAt = DateTime.Now,
            RequestType = requestType,
            StartDate = startDate,
            EndDate = endDate,
            Results = results.ToList()
        });

        string json = JsonSerializer.Serialize(history, _jsonOptions);

        File.WriteAllText(_filePath, json);
    }
}