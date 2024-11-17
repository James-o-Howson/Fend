using Fend.Abstractions.Commands;
using Fend.Contracts.DependencyGraphs;
using Fend.Contracts.Scans;
using Fend.Domain.DependencyGraphs;
using Fend.Domain.DependencyGraphs.Building;
using Fend.Domain.DependencyGraphs.ValueObjects;

namespace Fend.Commands.Scans.RunDependencyScan;

internal sealed class RunDependencyScanHandler : ICommandHandler<RunDependencyScanCommand, ScanResultDto>
{
    private const string AllFilesSearchPattern = "*.*";
    private const string FileName = "vulns.json";

    private readonly IEnumerable<IManifestDependencyBuilder> _projectBuilders;

    public RunDependencyScanHandler(IEnumerable<IManifestDependencyBuilder> projectBuilders)
    {
        _projectBuilders = projectBuilders;
    }

    public async Task<ScanResultDto> HandleAsync(RunDependencyScanCommand command, CancellationToken cancellationToken = default)
    {
        var dependencyGraph = await BuildAsync(command.Target, cancellationToken);

        return new ScanResultDto(dependencyGraph.ToDto(), GetOutputPath(command));
    }
    
    private async Task<DependencyGraph> BuildAsync(DirectoryInfo projectDirectory,
        CancellationToken cancellationToken = default)
    {
        var dependencyGraph = InitDependencyGraph(projectDirectory);
        var context = BuilderContext.Create(projectDirectory, _projectBuilders, cancellationToken);
        
        foreach (var filePath in GetAllProjectFiles(projectDirectory))
        {
            var matchingBuilders = context.GetBuildersForFile(filePath);
            if (matchingBuilders.Count == 0) continue;
            
            // Don't read content or make file info unless the file path matches a builder, it's expensive.
            var projectFileInfo = new FileInfo(filePath);  
            
            foreach (var builder in matchingBuilders)
            {
                var result = await builder.BuildAsync(projectFileInfo, context);
                if(result is null) continue;
                
                UpdateDependencyGraph(dependencyGraph, result);
            }
        }

        return dependencyGraph;
    }

    private static void UpdateDependencyGraph(DependencyGraph dependencyGraph, ManifestBuilderResult result)
    {
        foreach (var (parent, dependencies) in result.DependenciesByParent)
        {
            var projectNode = dependencyGraph.AddNode(parent, dependencyGraph.RootNode);
            dependencyGraph.AddNodes(dependencies, projectNode);
        }
    }

    private static DependencyGraph InitDependencyGraph(DirectoryInfo projectDirectory)
    {
        var id = DependencyItemId.Create(projectDirectory.Name);
        var rootNode = DependencyItem.Create(id, DependencyType.Project);
        
        return new DependencyGraph(rootNode);
    }

    private static ParallelQuery<string> GetAllProjectFiles(DirectoryInfo projectDirectory) => 
        Directory.EnumerateFiles(projectDirectory.FullName, AllFilesSearchPattern, SearchOption.AllDirectories)
            .AsParallel()
            .WithDegreeOfParallelism(Environment.ProcessorCount);
    
    private static string GetOutputPath(RunDependencyScanCommand command) =>
        string.IsNullOrWhiteSpace(command.OutputPath) ? 
            Path.Join(command.Target.FullName, FileName) : 
            command.OutputPath;
}