namespace Fend.DependencyGraph.Building.Manifests.Npm;

internal sealed class NodeManifest
{
    public string Name { get; init; } = string.Empty;
    public string Version { get; init; } = string.Empty;
    public Dictionary<string, string> Scripts { get; init; } = new();
    public bool Private { get; init; }
    public Dictionary<string, string?> Dependencies { get; init; } = new();
    public Dictionary<string, string> DevDependencies { get; init; } = new();
}