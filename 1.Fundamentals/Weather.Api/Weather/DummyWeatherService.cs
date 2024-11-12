
namespace Weather.Api.Weather;

public class DummyWeatherService : IWeatherService
{
    public Task<WeatherResponse?> GetCurrentWeatherAsync(string city)
    {
        throw new NotImplementedException();
    }
}
