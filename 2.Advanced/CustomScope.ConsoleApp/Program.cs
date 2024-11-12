using CustomScope.ConsoleApp;

using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddScoped<ExampleService>();

var serviceProvider = services.BuildServiceProvider();

// here is the recommended approach

var serviceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();

using (var serviceScope = serviceScopeFactory.CreateScope())
{
    var exampleService1 = serviceScope.ServiceProvider.GetRequiredService<ExampleService>();
    Console.WriteLine(exampleService1.Id);
}

using (var serviceScope = serviceScopeFactory.CreateScope())
{
    var exampleService2 = serviceScope.ServiceProvider.GetRequiredService<ExampleService>();
    Console.WriteLine(exampleService2.Id);
}

// below approach is not recommended by doable

using (var serviceScope = serviceProvider.CreateScope())
{
    var exampleService1 = serviceScope.ServiceProvider.GetRequiredService<ExampleService>();
    Console.WriteLine(exampleService1.Id);
}

using (var serviceScope = serviceProvider.CreateScope())
{
    var exampleService2 = serviceScope.ServiceProvider.GetRequiredService<ExampleService>();
    Console.WriteLine(exampleService2.Id);
}