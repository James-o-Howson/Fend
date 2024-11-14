using Fend.Domain.DependencyGraphs.ValueObjects;
using Fend.SharedKernel.Abstractions;

namespace Fend.Domain.DependencyGraphs;

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
        if (WouldCreateCycle(node)) throw new DomainException("Adding this dependency would create a cycle");

        _dependencies.Add(node);
        node._dependents.Add(this);
    }

    private bool WouldCreateCycle(DependencyNode newDependency, HashSet<DependencyNode>? visited = null)
    {
        visited ??= [];

        if (!visited.Add(this)) return true;
        if (this == newDependency) return true;

        return _dependencies.Any(dependency => dependency.WouldCreateCycle(newDependency, visited));
    }
}