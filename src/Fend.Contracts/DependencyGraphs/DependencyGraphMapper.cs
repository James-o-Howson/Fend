using Fend.Domain.DependencyGraphs;

namespace Fend.Contracts.DependencyGraphs;

public static class DependencyGraphMapper
{
    public static DependencyGraphDto ToDto(this DependencyGraph dependencyGraph)
    {
        var rootDependency = GetDependencyDto(dependencyGraph.RootNode);
        return new DependencyGraphDto(rootDependency);
    }

    private static List<DependencyDto> GetDependencies(DependencyNode dependencyNode) =>
        dependencyNode.Dependencies
            .Select(GetDependencyDto)
            .ToList();

    private static DependencyDto GetDependencyDto(DependencyNode d) => 
        new(GetName(d), GetVersion(d), GetDependencies(d));

    private static string GetName(DependencyNode dependencyNode) => 
        dependencyNode.DependencyItem.Id.Name;
    
    private static string GetVersion(DependencyNode dependencyNode) => 
        dependencyNode.DependencyItem.Id.Version;
}