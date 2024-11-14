
using Consumer.ConsoleApp;

using NewDiFramework;

var services = new ServiceCollection();

//services.AddSingleton<IIdGenerator, IdGenerator>();
services.AddTransient<IIdGenerator, IdGenerator>();

//services.AddTransient<IdGenerator>();

var serviceProvider = services.BuildServiceProvider();

var service1 = serviceProvider.GetService<IIdGenerator>();
var service2 = serviceProvider.GetService<IIdGenerator>();

Console.WriteLine(((IdGenerator)service1).Id);
Console.WriteLine(((IdGenerator)service2).Id);