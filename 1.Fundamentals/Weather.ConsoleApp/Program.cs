using Microsoft.Extensions.DependencyInjection;

using Weather.ConsoleApp;
using Weather.ConsoleApp.Weather;

if (args.Length == 0)
{
    args = new string[1];
    args[0] = "London";
}

var services = new ServiceCollection();

services.AddSingleton<IWeatherService, OpenWeatherService>();
services.AddSingleton<Application>();

// services.AddScoped(_ => new Application(new OpenWeatherService()));

var serviceProvider = services.BuildServiceProvider();

// GetService - returns null on runtime if not added in service collection
var app = serviceProvider.GetService<Application>();

// GetRequiredService - returns exception on runtime if not added in service collection (recommended)
var application = serviceProvider.GetRequiredService<Application>();

await application.RunAsync(args);