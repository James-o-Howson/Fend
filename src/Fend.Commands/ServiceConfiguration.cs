using Fend.Abstractions.Commands;
using Fend.Commands.Scans.RunDependencyScan;
using Fend.Contracts.DependencyGraphs;
using Fend.Contracts.Scans;
using Microsoft.Extensions.DependencyInjection;

namespace Fend.Commands;

public static class ServiceConfiguration
{
    public static void AddCommandHandlers(this IServiceCollection services)
    {
        services.AddTransient<ICommandHandler<RunDependencyScanCommand, ScanResultDto>, RunDependencyScanHandler>();
    }
}