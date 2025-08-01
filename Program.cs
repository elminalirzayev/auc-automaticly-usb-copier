using AUC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);

#if DEBUG
builder.ConfigureServices(services =>
{
    services.AddHostedService<Worker>();
});
#else
builder.UseWindowsService()
       .ConfigureServices(services =>
       {
           services.AddHostedService<Worker>();
       });
#endif

builder.Build().Run();
