using WeatherForecastApp.Interfaces;
using WeatherForecastApp.Services;

using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddHttpClient("AccuWeather", client =>
{
    client.BaseAddress = new Uri("https://dataservice.accuweather.com/");
});
services.AddTransient<IWeatherService, WeatherService>();

var provider = services.BuildServiceProvider();

var weather = provider.GetRequiredService<IWeatherService>();

var forecasts = await weather.GetForecastAsync();
foreach (var f in forecasts)
    Console.WriteLine($"{f.Date}: {f.MinimumTemperature}..{f.MaximumTemperature}, {f.SkyConditions}");
