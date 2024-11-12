using Microsoft.Extensions.DependencyInjection.Extensions;

using Weather.Api.Logging;
using Weather.Api.Weather;

namespace Weather.Api.Registrations;

public static class CommonApproach
{
    public static IServiceCollection AddCommonServices(this IServiceCollection services)
    {
        services.AddTransient<IWeatherService, OpenWeatherService>();
        services.AddTransient<IWeatherService, InMemoryWeatherService>();

        // prevents accidental multiple registration of same type
        services.TryAddTransient<IWeatherService, DummyWeatherService>();

        // another way 1 - explicit approach
        services.AddTransient<IWeatherService>(provider =>
        {
            var clientFactory = provider.GetRequiredService<IHttpClientFactory>();
            var logger = provider.GetRequiredService<ILoggerAdapter<OpenWeatherService>>();

            return new OpenWeatherService(clientFactory, logger);
        });

        // another way 2 - explicit approach
        services.AddTransient(typeof(IWeatherService), provider =>
        {
            var clientFactory = provider.GetRequiredService<IHttpClientFactory>();
            var logger = provider.GetRequiredService<ILoggerAdapter<OpenWeatherService>>();

            return new OpenWeatherService(clientFactory, logger);
        });

        return services;
    }
}
