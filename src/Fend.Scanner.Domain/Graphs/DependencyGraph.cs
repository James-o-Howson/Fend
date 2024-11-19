using Fend.Core.SharedKernel.Base;
using Fend.Scanner.Domain.Graphs.ValueObjects;

namespace Fend.Scanner.Domain.Graphs;

public sealed class DependencyGraph : AggregateRoot<DependencyGraphId>
{
    private readonly Dictionary<DependencyNodeId, DependencyNode> _nodesById = new();
    private readonly Dictionary<DependencyItemId, HashSet<DependencyNode>> _nodesByDependencyItemId = new();

    public DependencyNode RootNode { get; }
    
    public DependencyGraph(DependencyItem rootDependencyItem)
    {
        RootNode = DependencyNode.Create(rootDependencyItem);
        AddNodeToIndex(RootNode);
    }
    
    public void AddNodes(IReadOnlyCollection<DependencyItem> dependencies, DependencyNode parent)
    {
        if (dependencies.Count == 0) return;
        
        foreach (var dependency in dependencies)
        {
            AddNode(dependency, parent);
        }
    }

    public DependencyNode AddNode(DependencyItem dependency, DependencyNode parent)
    {
        var node = DependencyNode.Create(dependency);
        parent.AddDependency(node);
        AddNodeToIndex(node);
        
        return node;
    }

    public IEnumerable<DependencyItem> GetAllDependencyItems()
        => _nodesByDependencyItemId.Keys.Select(id => _nodesByDependencyItemId[id].First().DependencyItem);

    public IEnumerable<DependencyItem> GetNonRootDependencies()
        => GetAllDependencyItems().Where(p => !Equals(p, RootNode.DependencyItem));

    public IEnumerable<DependencyNode> GetNodesByDependencyItemId(DependencyItemId dependencyItemId)
        => _nodesByDependencyItemId.TryGetValue(dependencyItemId, out var nodes) 
            ? nodes 
            : Enumerable.Empty<DependencyNode>();

    public bool IsTransitive(DependencyItemId dependencyItemId)
    {
        var directDependencies = RootNode.Dependencies;
        return !GetNodesByDependencyItemId(dependencyItemId)
            .Any(node => directDependencies.Contains(node));
    }

    public IEnumerable<IList<DependencyItem>> FindPathsToRoot(DependencyItemId dependencyItemId, int? limit = null)
    {
        var paths = new List<IList<DependencyItem>>();
        foreach (var node in GetNodesByDependencyItemId(dependencyItemId))
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
    
    private void AddNodeToIndex(DependencyNode node)
    {
        _nodesById[node.Id] = node;
        
        if (!_nodesByDependencyItemId.TryGetValue(node.DependencyItem.Id, out var nodes))
        {
            nodes = [];
            _nodesByDependencyItemId[node.DependencyItem.Id] = nodes;
        }
        
        nodes.Add(node);
    }

    private IEnumerable<IList<DependencyItem>> FindNodePathsToRoot(
        DependencyNode node,
        HashSet<DependencyNode>? visited = null)
    {
        visited ??= [];

        if (!visited.Add(node)) return [];

        if (node == RootNode) return [new List<DependencyItem> { node.DependencyItem }];

        var paths = new List<IList<DependencyItem>>();

        foreach (var dependent in node.Dependents)
        {
            var dependentPaths = FindNodePathsToRoot(dependent, [..visited]);
            
            foreach (var path in dependentPaths)
            {
                var newPath = new List<DependencyItem> { node.DependencyItem };
                newPath.AddRange(path);
                paths.Add(newPath);
            }
        }

        return paths;
    }
}