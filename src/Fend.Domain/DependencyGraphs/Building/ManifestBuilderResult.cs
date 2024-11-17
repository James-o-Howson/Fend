using Fend.Domain.DependencyGraphs.ValueObjects;

namespace Fend.Domain.DependencyGraphs.Building;

public sealed class ManifestBuilderResult
{
    private readonly Dictionary<DependencyItem, HashSet<DependencyItem>> _dependenciesByParent = [];
    
    private ManifestBuilderResult() { }

    public IReadOnlyDictionary<DependencyItem, HashSet<DependencyItem>> DependenciesByParent => _dependenciesByParent;

    public static ManifestBuilderResult Create() => new();

    public void AddDependencies(DependencyItem parent, IReadOnlyCollection<DependencyItem> dependencies)
    {
        if (!_dependenciesByParent.TryGetValue(parent, out var existing))
        {
            existing = [];
            _dependenciesByParent[parent] = existing;
        }

        foreach (var dependency in dependencies)
        {
            existing.Add(dependency);
        }
    }
}