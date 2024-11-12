namespace Weather.ConsoleApp.Weather;

internal interface IWeatherService
{
    Task<WeatherResponse?> GetCurrentWeatherAsync(string city);
}
