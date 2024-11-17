using System.Collections.Concurrent;
using Fend.Domain.DependencyGraphs.Building;

namespace Fend.DependencyGraph.Building;

internal class BuilderContext : IBuilderContext
{
    private readonly ConcurrentDictionary<string, FileInfo?> _completedFiles = new();
    
    private BuilderContext() { }
    
    public required ConcurrentQueue<IManifestDependencyBuilder> Builders { get; init; }
    public required DirectoryInfo ProjectRootDirectory { get; init; }
    public required CancellationToken CancellationToken { get; init; }

    public List<IManifestDependencyBuilder> GetBuildersForFile(string filePath) => 
        string.IsNullOrWhiteSpace(filePath) ? 
            [] : Builders.Where(p => p.IsManifest(filePath)).ToList();
    
    public void MarkAsComplete(string path, FileInfo? fileInfo) => 
        _completedFiles.TryAdd(Path.GetFullPath(path).ToLower(), fileInfo);

    public bool IsAlreadyComplete(string path) => 
        _completedFiles.ContainsKey(Path.GetFullPath(path).ToLower());

    public static IBuilderContext Create(DirectoryInfo projectDirectory,
        IEnumerable<IManifestDependencyBuilder> projectParsers,
        CancellationToken cancellationToken = default) =>
        new BuilderContext
        {
            ProjectRootDirectory = projectDirectory,
            Builders = new ConcurrentQueue<IManifestDependencyBuilder>(projectParsers),
            CancellationToken = cancellationToken
        };
}