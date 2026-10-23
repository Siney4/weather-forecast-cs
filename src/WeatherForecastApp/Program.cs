using WeatherForecastApp.Interfaces;
using WeatherForecastApp.Services;

using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddHttpClient<IWeatherService, WeatherService>();

var provider = services.BuildServiceProvider();

var weather = provider.GetRequiredService<IWeatherService>();

var forecasts = await weather.GetForecastAsync();
foreach (var f in forecasts)
    Console.WriteLine($"{f.Date}: {f.MinTemp}..{f.MaxTemp}, {f.Description}");
