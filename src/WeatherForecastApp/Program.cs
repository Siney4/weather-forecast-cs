using WeatherForecastApp.Interfaces;
using WeatherForecastApp.Services;
using WeatherForecastApp.UI;
using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.DependencyInjection;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var services = new ServiceCollection();

services.AddSingleton<IConfiguration>(configuration);


services.AddHttpClient("AccuWeather", client =>
{
    client.BaseAddress = new Uri("https://dataservice.accuweather.com/");
});
services.AddTransient<IWeatherService, WeatherService>();
services.AddTransient<MenuService>();
services.AddTransient<ConsoleMenu>();
services.AddSingleton<ForecastHistoryService>();

var provider = services.BuildServiceProvider();

var weather = provider.GetRequiredService<IWeatherService>();

var forecasts = await weather.GetForecastAsync();

var menu = provider.GetRequiredService<ConsoleMenu>();

await menu.RunAsync(forecasts);