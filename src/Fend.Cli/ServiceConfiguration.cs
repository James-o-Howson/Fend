using Fend.Application;
using Fend.Application.Behaviours;
using Fend.Commands;
using Fend.Infrastructure;
using Fend.Scanner.Infrastructure;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Fend.Cli;

internal static class ServiceConfiguration
{
    public static void AddCliServices(this IServiceCollection services)
    {
        services.AddApplication();
        services.AddInfrastructure();
        services.AddDependencyGraph();
        
        services.AddMediatR(configurator =>
        {
            configurator.RegisterServicesFromAssemblies(CommandsAssembly.Assembly);

            configurator.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            configurator.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            configurator.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        });
    }
}