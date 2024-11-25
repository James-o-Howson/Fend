using System.Text.RegularExpressions;
using System.Xml.Linq;
using Fend.Core.Domain.Dependencies;
using Fend.Core.Domain.Dependencies.Building;
using Fend.Core.Domain.Dependencies.ValueObjects;
using Fend.Core.Domain.Dependencies.ValueObjects.Ids;
using Fend.Scanner.Infrastructure.Manifests.Nuget.CSharp;
using Microsoft.Extensions.Logging;

namespace Fend.Scanner.Infrastructure.Manifests.Nuget;

internal sealed partial class NugetDependencyBuilder : IManifestDependencyBuilder
{
    private const string Pattern = """
                                   Project\("(.*?)"\)\s*=\s*"(.*?)"\s*,\s*"(.*?\.csproj)"\s*,\s*"(.*?)"
                                   """;
    [GeneratedRegex(Pattern, RegexOptions.Multiline)]
    private static partial Regex ProjectInfoRegex();
    
    private const string SolutionFileExtension = ".sln";
    private const string SolutionFileSearchPattern = $"*{SolutionFileExtension}";
    
    private readonly IEnumerable<ICSharpProjectManifestBuilder> _cSharpProjectBuilders;
    private readonly ILogger<NugetDependencyBuilder> _logger;

    public NugetDependencyBuilder(IEnumerable<ICSharpProjectManifestBuilder> cSharpProjectBuilders, ILogger<NugetDependencyBuilder> logger)
    {
        _cSharpProjectBuilders = cSharpProjectBuilders;
        _logger = logger;
    }

    public bool IsRootDirectory(DirectoryInfo potentialProject) => 
        GetSolutionFilePaths(potentialProject).Length != 0;

    public bool IsManifest(string potentialProjectPath) =>
        !string.IsNullOrEmpty(potentialProjectPath) && 
        potentialProjectPath.EndsWith(potentialProjectPath);

    public async Task<ManifestBuilderResult?> BuildAsync(FileInfo solutionFile, IBuilderContext context,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Parsing C# Solution {SolutionPath}", solutionFile.FullName);
        
        var content = await File.ReadAllTextAsync(solutionFile.FullName, cancellationToken);
        var projectDefinitions = ExtractProjectManifests(solutionFile, content);

        var manifestBuilderResult = await CreateCSharpSolutionDependencyGraphAsync(context, solutionFile, projectDefinitions, cancellationToken);
        
        _logger.LogInformation("Finished Parsing C# Solution {SolutionPath}. Dependencies found = {ProjectsCount}",
            solutionFile.FullName, manifestBuilderResult.TotalDependencies);
        
        return manifestBuilderResult;
    }
    
    private async Task<ManifestBuilderResult> CreateCSharpSolutionDependencyGraphAsync(IBuilderContext context,
        FileInfo solutionFileInfo,
        IEnumerable<DotNetProjectManifest> projectDefinitions, CancellationToken cancellationToken)
    {
        var result = ManifestBuilderResult.Create();
        
        foreach (var manifest in projectDefinitions)
        {
            if (context.IsAlreadyComplete(manifest.FilePath))
            {
                _logger.LogInformation("Skipping CSharp Project {CsprojFilePath}. Project has already been parsed", manifest.FilePath);
                continue;
            }
            
            if (!manifest.Exists)
            {
                context.MarkAsComplete(manifest.FilePath, null);
                _logger.LogInformation("Skipping CSharp Project {CsprojFilePath}. Project in Solution File but not present in Repository", manifest.FilePath);
                continue;
            }
            
            _logger.LogInformation("Parsing CSharp Project {CsprojFilePath}", manifest.FilePath);

            var projectDependencies = _cSharpProjectBuilders
                .SelectMany(p => p.ParseAsync(manifest.Content)).ToHashSet();

            var projectId = DependencyId.Create(manifest.Name, await GetDotNetVersionAsync(manifest.Content, cancellationToken));
            var project = Dependency.Create(projectId,
                DependencyType.Project,
                GetMetadata(manifest, solutionFileInfo)); 
        
            result.AddDependencies(project, projectDependencies);
            
            _logger.LogInformation("Finished Parsing C# Project {CsprojFilePath}. Dependencies found = {DependencyCount}", 
                manifest.FilePath, projectDependencies.Count);
            
            context.MarkAsComplete(manifest.FilePath, manifest.FileInfo);
        }
        
        return result;
    }
    
    private static IEnumerable<DotNetProjectManifest> ExtractProjectManifests(FileInfo solutionFile, string content)
    {
        var regex = ProjectInfoRegex();
        var matches = regex.Matches(content);

        List<DotNetProjectManifest> projectDefinitions = [];
        foreach (Match projectLine in matches)
        {
            var projectId = projectLine.Groups[4].Value;
            var projectTypeId = projectLine.Groups[1].Value;
            var projectFilePath = projectLine.Groups[3].Value;
            
            projectDefinitions.Add(new DotNetProjectManifest
            {
                ProjectId = projectId,
                Type = projectTypeId,
                FilePath = Path.Join(Path.GetDirectoryName(solutionFile.FullName), projectFilePath)
            });
        }

        return projectDefinitions;
    }
    
    private static async Task<string> GetDotNetVersionAsync(string csprojContent, CancellationToken cancellationToken)
    {
        var document = await XDocument.LoadAsync(new StringReader(csprojContent), LoadOptions.None, cancellationToken);

        string[] versionElements =
        [
            "TargetFramework",
            "TargetFrameworks",
            "TargetFrameworkIdentifier",
            "TargetFrameworkVersion",
            "TargetFrameworkMoniker",
            "TargetFrameworkProfile",
            "TargetFrameworkRootNamespace",
            "RuntimeIdentifier"
        ];

        var versions = new List<string>();
        foreach (var element in versionElements)
        {
            var value = GetElementValue(document, element);
            if (string.IsNullOrEmpty(value)) continue;
            
            versions.Add(value.Trim());
        }

        return string.Join(", ", versions);
    }

    private static string? GetElementValue(XContainer document, string elementName)
    {
        var element = document.Descendants().FirstOrDefault(e => e.Name.LocalName.Equals(elementName, StringComparison.OrdinalIgnoreCase));
        return element?.Value;
    }

    private static Dictionary<string, string> GetMetadata(DotNetProjectManifest manifest, FileInfo solutionFile)
    {
        return new Dictionary<string, string>
        {
            { "Solution Name", solutionFile.Name },
            { "Visual Studio Project Guid", manifest.ProjectId },
            { "Visual Studio Type Guid", manifest.Type }
        };
    }

    private static string[] GetSolutionFilePaths(DirectoryInfo potentialProject) => 
        Directory.GetFiles(potentialProject.FullName, SolutionFileSearchPattern, SearchOption.TopDirectoryOnly);
}