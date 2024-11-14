using Fend.DependencyGraph.Manifests.Npm;
using Fend.DependencyGraph.Manifests.Nuget.CSharp;
using Fend.Domain.DependencyGraphs.Builders;
using Microsoft.Extensions.DependencyInjection;
using LocalReferenceAttributeParser = Fend.DependencyGraph.Manifests.Nuget.CSharp.LocalReferenceAttributeParser;
using NugetDependencyBuilder = Fend.DependencyGraph.Manifests.Nuget.NugetDependencyBuilder;

namespace Fend.DependencyGraph;

public static class ServiceConfiguration
{
    public static void AddDependencyGraph(this IServiceCollection services)
    {
        services.AddTransient<IDependencyGraphBuilder, DependencyGraphBuilder>();
        
        // Npm
        services.AddTransient<IManifestDependencyBuilder, NpmDependencyBuilder>();
        
        // Nuget
        services.AddTransient<IManifestDependencyBuilder, NugetDependencyBuilder>();
        services.AddTransient<ICSharpProjectManifestParser, NugetPackageReferenceAttributeParser>();
        services.AddTransient<ICSharpProjectManifestParser, LocalReferenceAttributeParser>();
    }
}