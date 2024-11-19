using Fend.Core.SharedKernel;
using Fend.Core.SharedKernel.Abstractions;
using Fend.Scanner.Domain.Graphs.ValueObjects;

namespace Fend.Scanner.Domain.Graphs;

public sealed class DependencyNode : IEntity<DependencyNodeId>
{
    private readonly HashSet<DependencyNode> _dependencies = [];
    private readonly HashSet<DependencyNode> _dependents = [];

    public DependencyNodeId Id { get; init; }
    public required DependencyItem DependencyItem { get; init; }
    public IReadOnlySet<DependencyNode> Dependencies => _dependencies;
    public IReadOnlySet<DependencyNode> Dependents => _dependents;

    private DependencyNode() { }

    public static DependencyNode Create(DependencyItem dependencyItem) => new()
    {
        DependencyItem = dependencyItem
    };

    public void AddDependency(DependencyNode node)
    {
        if (WouldCreateCircularReference(node)) throw new DomainException("Adding this dependency would create a circular reference.");

        _dependencies.Add(node);
        node._dependents.Add(this);
    }

    private bool WouldCreateCircularReference(DependencyNode newDependency, HashSet<DependencyNode>? visited = null)
    {
        visited ??= [];

        if (!visited.Add(this)) return true;
        if (this == newDependency) return true;

        return _dependencies.Any(dependency => dependency.WouldCreateCircularReference(newDependency, visited));
    }
}