﻿using Fend.Domain.Dependencies;
using Fend.Domain.Dependencies.Building;
using Fend.Domain.Dependencies.ValueObjects;
using Fend.Domain.Dependencies.ValueObjects.Ids;
using DepGraph = Fend.Domain.Dependencies.DependencyGraph;

namespace Fend.Scanner.Infrastructure;

internal sealed class DependencyGraphBuilder : IDependencyGraphBuilder
{
    private const string AllFilesSearchPattern = "*.*";

    private readonly IEnumerable<IManifestDependencyBuilder> _projectBuilders;

    public DependencyGraphBuilder(IEnumerable<IManifestDependencyBuilder> projectBuilders)
    {
        _projectBuilders = projectBuilders;
    }

    public async Task<DepGraph> BuildAsync(DirectoryInfo projectDirectory,
        CancellationToken cancellationToken = default)
    {
        var dependencyGraph = InitDependencyGraph(projectDirectory);
        var context = BuilderContext.Create(projectDirectory, _projectBuilders);
        
        foreach (var filePath in GetAllProjectFiles(projectDirectory))
        {
            var matchingBuilders = context.GetBuildersForFile(filePath);
            if (matchingBuilders.Count == 0) continue;
            
            // Don't read content or make file info unless the file path matches a builder, it's expensive.
            var projectFileInfo = new FileInfo(filePath);  
            
            foreach (var builder in matchingBuilders)
            {
                var result = await builder.BuildAsync(projectFileInfo, context, cancellationToken);
                if(result is null) continue;
                
                UpdateDependencyGraph(dependencyGraph, result);
            }
        }

        return dependencyGraph;
    }

    private static void UpdateDependencyGraph(DepGraph dependencyGraph, ManifestBuilderResult result)
    {
        foreach (var (parent, dependencies) in result.DependenciesByParent)
        {
            var projectNode = dependencyGraph.AddNode(parent, dependencyGraph.Root);
            dependencyGraph.AddNodes(dependencies, projectNode);
        }
    }

    private static DepGraph InitDependencyGraph(DirectoryInfo projectDirectory)
    {
        var id = DependencyId.Create(projectDirectory.Name);
        var rootNode = Dependency.Create(id, DependencyType.Project);
        
        return new DepGraph(rootNode);
    }

    private static ParallelQuery<string> GetAllProjectFiles(DirectoryInfo projectDirectory) => 
        Directory.EnumerateFiles(projectDirectory.FullName, AllFilesSearchPattern, SearchOption.AllDirectories)
            .AsParallel()
            .WithDegreeOfParallelism(Environment.ProcessorCount);
}