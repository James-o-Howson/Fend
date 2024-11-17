using System.Text;
using System.Text.Json;
using Fend.Domain.DependencyGraphs.Building;
using Fend.Domain.DependencyGraphs.ValueObjects;

namespace Fend.DependencyGraph.Building.Manifests.Npm;

internal sealed class NpmDependencyBuilder : IManifestDependencyBuilder
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };
    
    private const string JsonFileSearchPattern = "*.json";
    private const string PackageJson = "package.json";
    private const string AngularJson = "angular.json";

    public bool IsRootDirectory(DirectoryInfo potentialProject)
    {
        var jsonFilePaths = GetJsonFilePathsInDirectory(potentialProject);

        return ContainsNodeManifest(jsonFilePaths) && ContainsAngularManifest(jsonFilePaths);
    }

    public bool IsManifest(string potentialProjectPath) =>
        !string.IsNullOrWhiteSpace(potentialProjectPath) && IsNodeManifest(potentialProjectPath);

    public async Task<ManifestBuilderResult?> BuildAsync(FileInfo projectFile, IBuilderContext context)
    {
        if (context.IsAlreadyComplete(projectFile.FullName)) return null;
        
        var content = await File.ReadAllTextAsync(projectFile.FullName);
        var nodeManifest = await JsonSerializer.DeserializeAsync<NodeManifest>(
            new MemoryStream(Encoding.UTF8.GetBytes(content)),
            JsonSerializerOptions);
        
        if (nodeManifest is null) throw new ArgumentException($"Cannot deserialize {nameof(content)} into ${typeof(NodeManifest)}", content);
        var relativePath = projectFile.RelativeTo(context.ProjectRootDirectory);
        
        // Todo: Get Version from Angular manifest, not Node manifest
        var project = DependencyItem.Create(
            DependencyItemId.Create(nodeManifest.Name, nodeManifest.Version), 
            DependencyType.Project,
            new Dictionary<string, string> { { "Path", relativePath } });

        context.MarkAsComplete(projectFile.FullName, projectFile);

        var result = ManifestBuilderResult.Create();
        result.AddDependencies(project, GetAllNodeDependencies(nodeManifest));
        
        return result;
    }
    
    private static List<DependencyItem> GetAllNodeDependencies(NodeManifest nodeManifest)
    {
        var dependencies = nodeManifest.Dependencies
            .Select(entry => CreateDependency(entry.Key, entry.Value, "production"))
            .ToList();

        var devDependencies = nodeManifest.DevDependencies
            .Select(entry => CreateDependency(entry.Key, entry.Value, "development"));
        
        dependencies.AddRange(devDependencies);
        
        return dependencies;
    }

    private static DependencyItem CreateDependency(string dependencyName, string? version, string environment)
    {
        if (dependencyName.Length > 0 && dependencyName[0] == '@')
        {
            dependencyName = dependencyName[1..];
        }

        var packageInfo = DependencyItem.Create(
            DependencyItemId.Create(dependencyName, version ?? string.Empty), 
            DependencyType.Npm,
            new Dictionary<string, string> { { "Environment", environment } });
        
        return packageInfo;
    }

    private static string[] GetJsonFilePathsInDirectory(DirectoryInfo potentialProject) => 
        Directory.GetFiles(potentialProject.FullName, JsonFileSearchPattern, SearchOption.TopDirectoryOnly);
    
    private static bool ContainsNodeManifest(string[] filePaths) => filePaths.Any(IsNodeManifest);
    private static bool IsNodeManifest(string filePath) => filePath.EndsWith(PackageJson);
    private static bool ContainsAngularManifest(string[] filePaths) => filePaths.Any(IsAngularManifest);
    private static bool IsAngularManifest(string filePath) => filePath.EndsWith(AngularJson);
}