using System.Net;

using Weather.Api.Logging;

namespace Weather.Api.Weather;

public class OpenWeatherService : IWeatherService
{
    private const string OpenWeatherApiKey = "f539ebbe9ad5228403f6c267b7b7743c";
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILoggerAdapter<OpenWeatherService> loggerAdapter;

    public OpenWeatherService(IHttpClientFactory httpClientFactory, ILoggerAdapter<OpenWeatherService> loggerAdapter)
    {
        _httpClientFactory = httpClientFactory;
        this.loggerAdapter = loggerAdapter;
    }

    public async Task<WeatherResponse?> GetCurrentWeatherAsync(string city)
    {
        this.loggerAdapter.LogInformation("Logging via generic logger adaptor");

        var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={OpenWeatherApiKey}&units=metric";
        var httpClient = _httpClientFactory.CreateClient();

        var weatherResponse = await httpClient.GetAsync(url);
        if (weatherResponse.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
        var weather = await weatherResponse.Content.ReadFromJsonAsync<WeatherResponse>();
        return weather;
    }
}