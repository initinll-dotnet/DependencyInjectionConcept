using Weather.Api.Logging;

namespace Weather.Api.Registrations;

public static class OpenGenericApproach
{
    public static IServiceCollection AddOpenGenericServices(this IServiceCollection services)
    {
        // registering open generics
        services.AddTransient(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));

        return services;
    }
}
