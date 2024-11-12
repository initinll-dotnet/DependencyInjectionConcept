using Microsoft.AspNetCore.Mvc;

using Weather.Api.Registrations;
using Weather.Api.Service;
using Weather.Api.Weather;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

builder.Services.AddCommonServices();
builder.Services.AddKeyedServices();
builder.Services.AddDescriptorServices();
builder.Services.AddOpenGenericServices();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("weather/{city}", async (string city, [FromServices] IEnumerable<IWeatherService> weatherServices) =>
{
    // ElementAt(0) - OpenWeatherService implementation
    // ElementAt(1) - InMemoryWeatherService implementation
    // ElementAt(2) - DummyWeatherService implementation
    var weatherService = weatherServices.ElementAt(1);

    var weather = await weatherService.GetCurrentWeatherAsync(city);

    if (weather == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(weather);
})
.WithName("GetWeather")
.WithOpenApi();

app.MapGet("lifetime", ([FromServices] IServiceProvider serviceProvider) =>
{
    var transientIdGenerator = serviceProvider.GetRequiredKeyedService<IdGenerator>("transient");
    var scopedIdGenerator = serviceProvider.GetRequiredKeyedService<IdGenerator>("scoped");
    var singletonIdGenerator = serviceProvider.GetRequiredKeyedService<IdGenerator>("singleton");

    var transientId = transientIdGenerator.Id;
    var scopedId = scopedIdGenerator.Id;
    var singletonId = singletonIdGenerator.Id;

    var log = $"------In Endpoint------\nTransient Id: {transientId}\nScoped Id:{scopedId}\nSingleton Id:{singletonId}";
    app.Logger.LogInformation(log);

    return Results.Ok();
})
.AddEndpointFilter(async (efiContext, next) =>
{
    var transientIdGenerator = efiContext
        .HttpContext
        .RequestServices
        .GetRequiredKeyedService<IdGenerator>("transient");

    var scopedIdGenerator = efiContext
        .HttpContext
        .RequestServices
        .GetRequiredKeyedService<IdGenerator>("scoped");

    var singletonIdGenerator = efiContext
        .HttpContext
        .RequestServices
        .GetRequiredKeyedService<IdGenerator>("singleton");

    // execute before lifetime endpoint
    var transientId = transientIdGenerator.Id;
    var scopedId = scopedIdGenerator.Id;
    var singletonId = singletonIdGenerator.Id;

    var log = $"------Before Endpoint------\nTransient Id: {transientId}\nScoped Id:{scopedId}\nSingleton Id:{singletonId}";
    app.Logger.LogInformation(log);

    // execute lifetime endpoint
    var result = await next(efiContext);

    // execute after lifetime endpoint

    return result;
});

app.Run();