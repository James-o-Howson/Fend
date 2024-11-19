using Fend.Commands;
using Fend.Core.Abstractions.Behaviours;
using MediatR;
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
            
            configurator.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            configurator.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            configurator.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
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