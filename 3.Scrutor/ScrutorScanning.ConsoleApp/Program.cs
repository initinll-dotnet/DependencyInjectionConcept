using Microsoft.Extensions.DependencyInjection;

using Scrutor;

using ScrutorScanning.ConsoleApp.Attributes;

var services = new ServiceCollection();

//services.AddTransient<IExampleAService, ExampleAService>();

services.Scan(selector =>
{
    selector
        // .FromAssemblyOf<Program>()
        //     .AddClasses(f => f.AssignableTo<ISingletonService>())
        //         .AsImplementedInterfaces()
        //         .WithSingletonLifetime()
        //
        //     .AddClasses(f => f.AssignableTo<ITransientService>())
        //         .AsImplementedInterfaces()
        //         .WithTransientLifetime()
        //
        //     .AddClasses(f => f.AssignableTo<IScopedService>())
        //         .AsImplementedInterfaces()
        //         .WithScopedLifetime();

        .FromAssemblyOf<Program>()
            .AddClasses(f => f.WithAttribute<SingletonAttribute>())
                .AsImplementedInterfaces()
                .WithSingletonLifetime()

            .AddClasses(f => f.WithAttribute<TransientAttribute>())
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsImplementedInterfaces()
                .WithTransientLifetime()

            .AddClasses(f => f.WithAttribute<ScopedAttribute>())
                .AsImplementedInterfaces()
                .WithScopedLifetime();

    // .FromAssemblyOf<Program>()
    //     .AddClasses()
    //         .UsingAttributes();
});

PrintRegisteredService(services);
var serviceProvider = services.BuildServiceProvider();

void PrintRegisteredService(IServiceCollection serviceCollection)
{
    foreach (var service in serviceCollection)
    {
        Console.WriteLine($"{service.ServiceType.Name} -> {service.ImplementationType?.Name} as {service.Lifetime.ToString()}");
    }
}