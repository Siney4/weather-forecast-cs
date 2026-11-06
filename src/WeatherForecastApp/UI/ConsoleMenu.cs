namespace WeatherForecastApp.UI;

using WeatherForecastApp.Interfaces;
using WeatherForecastApp.Models;
using WeatherForecastApp.Services;

public class ConsoleMenu
{
    private readonly MenuService _menuService;
    private readonly LocationMenuService _locationMenuService;
    private readonly LocationSettingsService _settingsService;
    private readonly IWeatherService _weatherService;
    
    private LocationInfo _location = new();
    private List<DayForecast> _forecasts = new();

    public ConsoleMenu(
        MenuService menuService,
        LocationMenuService locationMenuService,
        LocationSettingsService settingsService,
        IWeatherService weatherService)
    {
        _menuService = menuService;
        _locationMenuService = locationMenuService;
        _settingsService = settingsService;
        _weatherService = weatherService;
    }

    public async Task RunAsync()
    {
        _location = _settingsService.GetLocation();

        try
        {
            _forecasts = await _weatherService.GetForecastAsync(_location.Key);
        }
        catch (Exception ex)
        {
            ShowError("Не удалось получить прогноз", ex);
            return;
        }

        while (true)
        {
            ShowMenu();

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    _menuService.ShowWeeklyForecast(_forecasts);
                    break;
                case "2":
                    _menuService.ShowNextDayForecast(_forecasts);
                    break;
                case "3":
                    _menuService.ShowForecastForDay(_forecasts);
                    break;
                case "4":
                    _menuService.ShowForecastForPeriod(_forecasts);
                    break;
                case "5":
                    await ShowSettingsAsync();
                    continue;
                case "6":
                    return;
                default:
                    Console.WriteLine("Неверный пункт меню.");
                    break;
            }

            Console.WriteLine();
            Console.WriteLine("Нажмите любую клавишу");
            Console.ReadKey(true);
        }
    }

    private async Task ShowSettingsAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Настройки");
            Console.WriteLine($"Текущая локация: {_location.Name}");
            Console.WriteLine("1) Выбор локации");
            Console.WriteLine("2) История прогнозов");
            Console.WriteLine("3) Назад");
            Console.Write("> ");

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    await ChangeLocationAsync();
                    break;
                case "2":
                    _menuService.ShowHistory();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Неверный пункт меню.");
                    break;
            }

            Console.WriteLine();
            Console.WriteLine("Нажмите любую клавишу");
            Console.ReadKey(true);
        }
    }

    private async Task ChangeLocationAsync()
    {
        var newLocation = await _locationMenuService.ChooseLocationAsync();

        if (newLocation == null)
            return;

        try
        {
            _forecasts = await _weatherService.GetForecastAsync(newLocation.Key);
            _location = newLocation;
            _settingsService.SaveLocation(newLocation);

            Console.WriteLine($"Локация изменена: {_location.Name}");
        }
        catch (Exception ex)
        {
            ShowError("Не удалось получить прогноз для новой локации", ex);
            Console.WriteLine("Локация не изменена.");
        }
    }

    private static void ShowError(string message, Exception ex)
    {
        Console.WriteLine($"{message}.");
        
        Console.WriteLine(ex.InnerException?.Message ?? ex.Message);
    }

    private void ShowMenu()
    {
        Console.Clear();
        Console.WriteLine($"Прогноз погоды ({_location.Name})");
        Console.WriteLine("1) Прогноз на неделю");
        Console.WriteLine("2) Прогноз на следующий день");
        Console.WriteLine("3) Прогноз на конкретную дату");
        Console.WriteLine("4) Прогноз на период");
        Console.WriteLine("5) Настройки (локация, история прогнозов)");
        Console.WriteLine("6) Выход");
        Console.Write("> ");
    }
}