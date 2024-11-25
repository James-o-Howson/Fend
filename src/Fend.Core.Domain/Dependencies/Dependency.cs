using System.Collections.Immutable;
using Fend.Core.Domain.Dependencies.ValueObjects;
using Fend.Core.Domain.Dependencies.ValueObjects.Ids;
using Fend.Core.SharedKernel;
using Fend.Core.SharedKernel.Abstractions;

namespace Fend.Core.Domain.Dependencies;

public sealed class Dependency : IEntity<DependencyId>
{
    private readonly HashSet<Dependency> _dependencies = [];
    private readonly HashSet<Dependency> _dependents = [];
    private readonly HashSet<Vulnerability> _vulnerabilities = [];

    public required DependencyId Id { get; init; }
    public required DependencyType Type { get; init; }
    public IDictionary<string, string> Metadata { get; init; } = ImmutableDictionary<string, string>.Empty;
    public IReadOnlySet<Dependency> Dependencies => _dependencies;
    public IReadOnlySet<Dependency> Dependents => _dependents;
    public IReadOnlySet<Vulnerability> Vulnerabilities => _vulnerabilities;

    private Dependency() { }
    
    public static Dependency Create(DependencyId id, DependencyType type) => new()
    {
        Id = id,
        Type = type
    };

    public static Dependency Create(DependencyId id, DependencyType type, IDictionary<string, string> metadata) => new()
    {
        Id = id,
        Type = type,
        Metadata = metadata.ToImmutableDictionary()
    };

    public void AddDependency(Dependency node)
    {
        if (WouldCreateCircularReference(node)) throw new DomainException("Adding this dependency would create a circular reference.");

        _dependencies.Add(node);
        node._dependents.Add(this);
    }

    private bool WouldCreateCircularReference(Dependency newDependency, HashSet<Dependency>? visited = null)
    {
        visited ??= [];

        if (!visited.Add(this)) return true;
        if (this == newDependency) return true;

        return _dependencies.Any(dependency => dependency.WouldCreateCircularReference(newDependency, visited));
    }
}