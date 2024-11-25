namespace Fend.Core.Domain.Dependencies.Building;

public sealed class ManifestBuilderResult
{
    private readonly Dictionary<Dependency, HashSet<Dependency>> _dependenciesByParent = [];
    
    private ManifestBuilderResult() { }

    public IReadOnlyDictionary<Dependency, HashSet<Dependency>> DependenciesByParent => _dependenciesByParent;

    public int TotalDependencies => DependenciesByParent.Values.Aggregate(0, (total, dependency) => total + dependency.Count);

    public static ManifestBuilderResult Create() => new();

    public void AddDependencies(Dependency parent, IReadOnlyCollection<Dependency> dependencies)
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