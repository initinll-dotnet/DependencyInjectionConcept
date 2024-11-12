using Microsoft.Extensions.DependencyInjection.Extensions;

using Weather.Api.Logging;
using Weather.Api.Weather;

namespace Weather.Api.Registrations;

public static class DescriptorApproach
{
    public static IServiceCollection AddDescriptorServices(this IServiceCollection services)
    {

        var weatherServiceDescriptor1 =
            new ServiceDescriptor(
                typeof(IWeatherService),
                typeof(DummyWeatherService),
                ServiceLifetime.Transient);


        var weatherServiceDescriptor2 =
            new ServiceDescriptor(
                typeof(IWeatherService),
                new DummyWeatherService(),
                ServiceLifetime.Transient);


        var weatherServiceDescriptor3 =
            new ServiceDescriptor(
                typeof(IWeatherService),
                provider =>
                {
                    return new DummyWeatherService();
                },
                ServiceLifetime.Transient);

        var weatherServiceDescriptor4 =
            new ServiceDescriptor(
                typeof(IWeatherService),
                provider =>
                {
                    var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
                    var logger = provider.GetRequiredService<ILoggerAdapter<OpenWeatherService>>();

                    return new OpenWeatherService(httpClientFactory, logger);
                },
                ServiceLifetime.Transient);

        // Add vs TryAdd Method

        // using Add method DI container can register multiple implementations of same IWeatherService service

        services.Add(weatherServiceDescriptor1);
        services.Add(weatherServiceDescriptor2);
        services.Add(weatherServiceDescriptor3);

        // using TryAdd DI method DI container skips multiple implementations IWeatherService service and only adds if there is not one
        // prevents accidental multiple registration of same type
        services.TryAdd(weatherServiceDescriptor4); // this will be skipped as above 3 are already added

        // TryAddEnumerable

        // TryAddEnumerable behaves differently than TryAdd method
        // TryAdd checks only for only uniqueness of interface type registration
        // TryAddEnumerable checks of uniqueness of (interface, implementation) type registration

        services.TryAddEnumerable(weatherServiceDescriptor1); // gets added 
        services.TryAddEnumerable(weatherServiceDescriptor1); // gets skipped
        services.TryAddEnumerable(weatherServiceDescriptor2); // get added

        // or

        ServiceDescriptor[] descriptors =
            [
            weatherServiceDescriptor1,
            weatherServiceDescriptor1,
            weatherServiceDescriptor2,
            ];

        services.TryAddEnumerable(descriptors);

        return services;
    }
}
