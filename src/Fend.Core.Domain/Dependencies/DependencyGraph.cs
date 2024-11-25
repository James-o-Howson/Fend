using Fend.Core.Domain.Dependencies.ValueObjects.Ids;
using Fend.Core.SharedKernel.Base;

namespace Fend.Core.Domain.Dependencies;

public sealed class DependencyGraph : AggregateRoot<DependencyGraphId>
{
    private readonly Dictionary<DependencyId, Dependency> _nodesById = new();
    private readonly Dictionary<DependencyId, HashSet<Dependency>> _nodesByDependencyItemId = new();

    public override DependencyGraphId Id { get; protected set; } = DependencyGraphId.New;
    public Dependency Root { get; }
    
    public DependencyGraph(Dependency rootDependency)
    {
        Root = rootDependency;
        AddNodeToIndex(Root);
    }
    
    public void AddNodes(IReadOnlyCollection<Dependency> dependencies, Dependency parent)
    {
        if (dependencies.Count == 0) return;
        
        foreach (var dependency in dependencies)
        {
            AddNode(dependency, parent);
        }
    }

    public Dependency AddNode(Dependency dependency, Dependency parent)
    {
        parent.AddDependency(dependency);
        AddNodeToIndex(dependency);
        
        return dependency;
    }

    public IEnumerable<Dependency> GetAllDependencyItems()
        => _nodesByDependencyItemId.Keys.Select(id => _nodesByDependencyItemId[id].First());

    public IEnumerable<Dependency> GetNonRootDependencies()
        => GetAllDependencyItems().Where(p => !Equals(p, Root));

    public IEnumerable<Dependency> GetNodesByDependencyItemId(DependencyId dependencyId)
        => _nodesByDependencyItemId.TryGetValue(dependencyId, out var nodes) 
            ? nodes 
            : Enumerable.Empty<Dependency>();

    public bool IsTransitive(DependencyId dependencyId)
    {
        var directDependencies = Root.Dependencies;
        return !GetNodesByDependencyItemId(dependencyId)
            .Any(node => directDependencies.Contains(node));
    }

    public IEnumerable<IList<Dependency>> FindPathsToRoot(DependencyId dependencyId, int? limit = null)
    {
        var paths = new List<IList<Dependency>>();
        foreach (var node in GetNodesByDependencyItemId(dependencyId))
        {
            var nodePaths = FindNodePathsToRoot(node);
            paths.AddRange(nodePaths);

            if (limit.HasValue && paths.Count >= limit.Value)
            {
                paths = paths.Take(limit.Value).ToList();
                break;
            }
        }

        return paths.OrderBy(p => p.Count);
    }
    
    private void AddNodeToIndex(Dependency dependency)
    {
        _nodesById[dependency.Id] = dependency;
        
        if (!_nodesByDependencyItemId.TryGetValue(dependency.Id, out var dependencies))
        {
            dependencies = [];
            _nodesByDependencyItemId[dependency.Id] = dependencies;
        }
        
        dependencies.Add(dependency);
    }

    private IEnumerable<IList<Dependency>> FindNodePathsToRoot(
        Dependency dependency,
        HashSet<Dependency>? visited = null)
    {
        visited ??= [];

        if (!visited.Add(dependency)) return [];

        if (dependency == Root) return [new List<Dependency> { dependency }];

        var paths = new List<IList<Dependency>>();

        foreach (var dependent in dependency.Dependents)
        {
            var dependentPaths = FindNodePathsToRoot(dependent, [..visited]);
            
            foreach (var path in dependentPaths)
            {
                var newPath = new List<Dependency> { dependency };
                newPath.AddRange(path);
                paths.Add(newPath);
            }
        }

        return paths;
    }
}