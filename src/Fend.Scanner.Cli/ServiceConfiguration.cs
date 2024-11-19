using Fend.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Fend.Scanner.Cli;

internal static class ServiceConfiguration
{
    public static void AddCli(this IServiceCollection services)
    {
        services.AddMediatR(configurator =>
        {
            configurator.RegisterServicesFromAssemblies(CommandsAssembly.Assembly);
        });
    }
    
    public static void AddLogging(this IHostBuilder hostBuilder)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        hostBuilder.UseSerilog();
    }
}