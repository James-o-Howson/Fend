using Fend.DependencyGraph.Manifests.Npm;
using Fend.Domain.DependencyGraphs.Builders;
using Microsoft.Extensions.DependencyInjection;
using NugetDependencyBuilder = Fend.DependencyGraph.Manifests.Nuget.NugetDependencyBuilder;

namespace Fend.DependencyGraph;

public static class ServiceConfiguration
{
    public static void AddDependencyGraph(this IServiceCollection services)
    {
        services.AddTransient<IDependencyGraphBuilder, DependencyGraphBuilder>();
        
        services.AddTransient<IManifestDependencyBuilder, NpmDependencyBuilder>();
        services.AddTransient<IManifestDependencyBuilder, NugetDependencyBuilder>();
    }
}