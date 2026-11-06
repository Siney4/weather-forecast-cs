using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WeatherForecastApp.Interfaces;
using WeatherForecastApp.Options;
using WeatherForecastApp.Services;
using WeatherForecastApp.UI;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var services = new ServiceCollection();

services.Configure<AccuWeatherOptions>(
    configuration.GetSection(AccuWeatherOptions.SectionName));
services.AddSingleton<IValidateOptions<AccuWeatherOptions>, AccuWeatherOptionsValidator>();

services.AddHttpClient("AccuWeather", (serviceProvider, client) =>
{
    var options = serviceProvider
        .GetRequiredService<IOptions<AccuWeatherOptions>>()
        .Value;

    client.BaseAddress = new Uri(options.BaseUrl);
});

services.AddTransient<IWeatherService, WeatherService>();
services.AddTransient<ILocationService, LocationService>();
services.AddTransient<LocationMenuService>();
services.AddTransient<MenuService>();
services.AddTransient<ConsoleMenu>();
services.AddSingleton<ForecastHistoryService>();
services.AddSingleton<LocationSettingsService>();

var provider = services.BuildServiceProvider();

try
{
    _ = provider.GetRequiredService<IOptions<AccuWeatherOptions>>().Value;
}
catch (OptionsValidationException ex)
{
    Console.WriteLine("Ошибка в настройках appsettings.json:");

    foreach (var failure in ex.Failures)
    {
        Console.WriteLine($"- {failure}");
    }

    return;
}

var menu = provider.GetRequiredService<ConsoleMenu>();

await menu.RunAsync();