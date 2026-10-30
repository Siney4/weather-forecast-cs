namespace WeatherForecastApp.UI;

public class ConsoleMenu
{
    public async Task RunAsync()
    {
        while (true)
        {
            ShowMenu();

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Console.WriteLine("Прогноз на неделю");
                    break;
                case "2":
                    Console.WriteLine("Прогноз на следующий день");
                    break;
                case "3":
                    Console.WriteLine("Прогноз на конкретную дату");
                    break;
                case "4":
                    Console.WriteLine("Прогноз на период");
                    break;
                case "5":
                    Console.WriteLine("Настройки");
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