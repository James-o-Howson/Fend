using Fend.Abstractions.Commands;
using Fend.Cli.Commands.Scan.RunDependencyScan;
using Microsoft.Extensions.DependencyInjection;

namespace Fend.Cli.Commands;

public static class ServiceConfiguration
{
    public static void AddCommandHandlers(this IServiceCollection services)
    {
        services.AddTransient<ICommandHandler<RunDependencyScanCommand>, RunDependencyScanHandler>();
    }
}