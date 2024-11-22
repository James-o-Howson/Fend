using Fend.Domain.Dependencies;
using Fend.Domain.Dependencies.Building;
using Fend.Scanner.Infrastructure.Manifests.Npm;
using Fend.Scanner.Infrastructure.Manifests.Nuget.CSharp;
using Microsoft.Extensions.DependencyInjection;
using LocalReferenceAttributeBuilder = Fend.Scanner.Infrastructure.Manifests.Nuget.CSharp.LocalReferenceAttributeBuilder;
using NugetDependencyBuilder = Fend.Scanner.Infrastructure.Manifests.Nuget.NugetDependencyBuilder;

namespace Fend.Scanner.Infrastructure;

public static class ServiceConfiguration
{
    public static void AddDependencyGraph(this IServiceCollection services)
    {
        services.AddTransient<IDependencyGraphBuilder, DependencyGraphBuilder>();
        
        // Npm
        services.AddTransient<IManifestDependencyBuilder, NpmDependencyBuilder>();
        
        // Nuget
        services.AddTransient<IManifestDependencyBuilder, NugetDependencyBuilder>();
        services.AddTransient<ICSharpProjectManifestBuilder, NugetPackageReferenceAttributeBuilder>();
        services.AddTransient<ICSharpProjectManifestBuilder, LocalReferenceAttributeBuilder>();
    }
}