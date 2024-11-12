using Weather.Api.Service;

namespace Weather.Api.Registrations;

public static class KeyedApproach
{
    public static IServiceCollection AddKeyedServices(this IServiceCollection services)
    {
        services.AddKeyedTransient<IdGenerator>("transient");
        services.AddKeyedScoped<IdGenerator>("scoped");
        services.AddKeyedSingleton<IdGenerator>("singleton");

        return services;
    }
}
