namespace WeatherForecastApp.UI;

using WeatherForecastApp.Services;
using WeatherForecastApp.Models;

public class ConsoleMenu
{
    private readonly MenuService _menuService;

    public ConsoleMenu(MenuService menuService)
    {
        _menuService = menuService;
    }
    
    public async Task RunAsync(List<DayForecast> forecasts)
    {
        while (true)
        {
            ShowMenu();

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    _menuService.ShowWeeklyForecast(forecasts);
                    break;
                case "2":
                    _menuService.ShowNextDayForecast(forecasts);
                    break;
                case "3":
                    _menuService.ShowForecastForDay(forecasts);
                    break;
                case "4":
                    _menuService.ShowForecastForPeriod(forecasts);
                    break;
                case "5":
                    _menuService.ShowHistory();
                    break;
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

    private static void ShowMenu()
    {
        Console.Clear();
        Console.WriteLine("Прогноз погоды");
        Console.WriteLine("1) Прогноз на неделю");
        Console.WriteLine("2) Прогноз на следующий день");
        Console.WriteLine("3) Прогноз на конкретную дату");
        Console.WriteLine("4) Прогноз на период");
        Console.WriteLine("5) Настройки (локация, история прогнозов)");
        Console.WriteLine("6) Выход");
        Console.Write("> ");
    }
}