using Fend.Scanner.Domain.Graphs.ValueObjects;

namespace Fend.Scanner.Domain.Graphs.Building;

public sealed class ManifestBuilderResult
{
    private readonly Dictionary<DependencyItem, HashSet<DependencyItem>> _dependenciesByParent = [];
    
    private ManifestBuilderResult() { }

    public IReadOnlyDictionary<DependencyItem, HashSet<DependencyItem>> DependenciesByParent => _dependenciesByParent;

    public int TotalDependencies => DependenciesByParent.Values.Aggregate(0, (total, dependency) => total + dependency.Count);

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