
using System.Globalization;
using WeatherForecastApp.Models;

namespace WeatherForecastApp.Services;

public class MenuService
{
    private const string DateFormat = "dd-MM-yyyy";
    
    private readonly ForecastHistoryService _historyService;
    
    public MenuService(ForecastHistoryService historyService)
    {
        _historyService = historyService;
    }

    private static bool TryReadDate(string prompt, out DateOnly date)
    {
        Console.Write(prompt);

        var input = Console.ReadLine();

        if (DateOnly.TryParseExact(
                input,
                DateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out date))
        {
            return true;
        }

        Console.WriteLine("Неверная дата. Используйте формат дд-мм-гггг, например 30-10-2026.");
        return false;
    }

    private static void PrintForecast(DayForecast forecast)
    {
        Console.WriteLine(
            $"{forecast.Date.ToString(DateFormat, CultureInfo.InvariantCulture)}: " +
            $"{forecast.MinimumTemperature}..." +
            $"{forecast.MaximumTemperature} °C, " +
            $"{forecast.SkyConditions}");
    }

    public void ShowWeeklyForecast(List<DayForecast> forecasts)
    {
        Console.WriteLine("Прогноз на неделю:");

        foreach (var forecast in forecasts.Take(7))
        {
            PrintForecast(forecast);
        }
    }

    public void ShowNextDayForecast(List<DayForecast> forecasts)
    {
        Console.WriteLine("Прогноз на завтра:");

        var forecast = forecasts
            .OrderBy(f => f.Date)
            .Skip(1)
            .FirstOrDefault();

        if (forecast == null)
        {
            Console.WriteLine("Прогноз на завтра недоступен.");
            return;
        }

        PrintForecast(forecast);
    }

    public void ShowForecastForDay(List<DayForecast> forecasts)
    {
        if (!TryReadDate("Введите дату (дд-мм-гггг): ", out var date))
            return;

        var forecast = forecasts.FirstOrDefault(f => f.Date == date);

        if (forecast == null)
        {
            Console.WriteLine("Прогноз на эту дату недоступен.");
            Console.WriteLine("Проверьте, входит ли дата в полученный от API диапазон.");
            return;
        }

        Console.WriteLine($"\nПрогноз на {date.ToString(DateFormat, CultureInfo.InvariantCulture)}:");
        PrintForecast(forecast);
    }

    public void ShowForecastForPeriod(List<DayForecast> forecasts)
    {
        if (!TryReadDate("Введите начальную дату (дд-мм-гггг): ", out var startDate))
            return;

        if (!TryReadDate("Введите конечную дату (дд-мм-гггг): ", out var endDate))
            return;

        if (startDate > endDate)
        {
            Console.WriteLine("Начальная дата не может быть позже конечной.");
            return;
        }

        var availableForecasts = forecasts
            .Where(f => f.Date >= startDate && f.Date <= endDate)
            .OrderBy(f => f.Date)
            .ToList();

        if (availableForecasts.Count == 0)
        {
            Console.WriteLine("На указанный период прогноз недоступен.");
            Console.WriteLine("Проверьте даты: API возвращает прогноз максимум на 15 дней.");
            return;
        }

        Console.WriteLine(
            $"\nПрогноз с {startDate.ToString(DateFormat, CultureInfo.InvariantCulture)} " +
            $"по {endDate.ToString(DateFormat, CultureInfo.InvariantCulture)}:");

        foreach (var forecast in availableForecasts)
        {
            PrintForecast(forecast);
        }

        var requestedDays = endDate.DayNumber - startDate.DayNumber + 1;
        var missingDays = requestedDays - availableForecasts.Count;

        if (missingDays > 0)
        {
            Console.WriteLine(
                $"\nДля {missingDays} дн. прогноз отсутствует в полученном списке.");
        }
    }
    
    public void ShowHistory()
    {
        var history = _historyService.GetHistory();

        if (history == null || history.Count == 0)
        {
            Console.WriteLine("История запросов пока пуста.");
            return;
        }

        Console.WriteLine("\n=== История запросов ===");

        foreach (var entry in history.OrderByDescending(e => e.RequestedAt))
        {
            Console.WriteLine(
                $"\nДата запроса: {entry.RequestedAt:dd-MM-yyyy HH:mm:ss}");

            Console.WriteLine($"Тип: {entry.RequestType ?? "Неизвестно"}");

            if (entry.StartDate.HasValue)
                Console.WriteLine($"Начало периода: {entry.StartDate:dd-MM-yyyy}");

            if (entry.EndDate.HasValue && entry.EndDate != entry.StartDate)
                Console.WriteLine($"Конец периода: {entry.EndDate:dd-MM-yyyy}");

            if (entry.Results == null || entry.Results.Count == 0)
            {
                Console.WriteLine("Результаты отсутствуют.");
                continue;
            }

            foreach (var forecast in entry.Results)
            {
                if (forecast == null)
                    continue;

                PrintForecast(forecast);
            }
        }
    }
}
