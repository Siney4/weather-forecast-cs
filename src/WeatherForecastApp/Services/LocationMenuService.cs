using System.Text.Json;
using WeatherForecastApp.Interfaces;
using WeatherForecastApp.Models;

namespace WeatherForecastApp.Services;


public class LocationMenuService
{
    private readonly ILocationService _locationService;

    public LocationMenuService(ILocationService locationService)
    {
        _locationService = locationService;
    }

    public async Task<LocationInfo?> ChooseLocationAsync()
    {
        Console.Write("Введите название города: ");

        var query = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(query))
        {
            Console.WriteLine("Название города не может быть пустым.");
            return null;
        }

        List<LocationInfo> locations;

        try
        {
            locations = await _locationService.SearchAsync(query.Trim());
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Ошибка запроса к AccuWeather: {ex.Message}");
            return null;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Не удалось разобрать ответ сервера: {ex.Message}");
            return null;
        }

        if (locations.Count == 0)
        {
            Console.WriteLine("Город не найден.");
            return null;
        }

        Console.WriteLine("\nНайдено:");

        for (var i = 0; i < locations.Count; i++)
        {
            Console.WriteLine($"{i + 1}) {locations[i].Name}");
        }

        Console.Write("Выберите номерEnter - отмена): ");

        var input = Console.ReadLine();

        if (int.TryParse(input, out var number) &&
            number >= 1 &&
            number <= locations.Count)
        {
            return locations[number - 1];
        }

        Console.WriteLine("Выбор отменён.");
        return null;
    }
}