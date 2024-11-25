using Fend.Core.Domain.Dependencies;

namespace Fend.Application.Contracts.Dependencies;

public static class DependencyGraphMapper
{
    public static DependencyDto ToDto(this DependencyGraph dependencyGraph) => 
        GetDependencyDto(dependencyGraph.Root);

    private static List<DependencyDto> GetDependencies(Dependency dependency) =>
        dependency.Dependencies
            .Select(GetDependencyDto)
            .ToList();

    private static DependencyDto GetDependencyDto(Dependency d) => 
        new(GetName(d), GetVersion(d), GetDependencies(d));

    private static string GetName(Dependency dependency) => 
        dependency.Id.Name;
    
    private static string GetVersion(Dependency dependency) => 
        dependency.Id.Version;
}