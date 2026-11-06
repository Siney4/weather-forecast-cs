using Microsoft.Extensions.Options;

namespace WeatherForecastApp.Options;

public class AccuWeatherOptionsValidator : IValidateOptions<AccuWeatherOptions>
{
    public ValidateOptionsResult Validate(string? name, AccuWeatherOptions options)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(options.ApiKey))
            errors.Add(":ApiKey не заполнен.");

        if (!Uri.TryCreate(options.BaseUrl, UriKind.Absolute, out _))
            errors.Add("введите BaseUrl");

        return errors.Count > 0
            ? ValidateOptionsResult.Fail(errors)
            : ValidateOptionsResult.Success;
    }
}