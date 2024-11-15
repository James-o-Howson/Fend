using Fend.DependencyGraph.Building;
using Fend.DependencyGraph.Building.Manifests.Npm;
using Fend.DependencyGraph.Building.Manifests.Nuget.CSharp;
using Fend.Domain.DependencyGraphs.Builders;
using Microsoft.Extensions.DependencyInjection;
using NugetDependencyBuilder = Fend.DependencyGraph.Building.Manifests.Nuget.NugetDependencyBuilder;

namespace Fend.DependencyGraph;

public static class ServiceConfiguration
{
    public static void AddDependencyGraph(this IServiceCollection services)
    {
        // Npm
        services.AddTransient<IManifestDependencyBuilder, NpmDependencyBuilder>();
        
        // Nuget
        services.AddTransient<IManifestDependencyBuilder, NugetDependencyBuilder>();
        services.AddTransient<ICSharpProjectManifestBuilder, NugetPackageReferenceAttributeBuilder>();
        services.AddTransient<ICSharpProjectManifestBuilder, LocalReferenceAttributeBuilder>();
    }
}