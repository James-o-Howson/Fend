using System.Text.RegularExpressions;
using Fend.Domain.DependencyGraphs.Builders;

namespace Fend.DependencyGraph.Manifests.Nuget;

internal sealed partial class NugetDependencyBuilder : IManifestDependencyBuilder
{
    private const string SolutionFileExtension = ".sln";
    private const string SolutionFileSearchPattern = $"*{SolutionFileExtension}";
    private const string Pattern = """
                                   Project\("(.*?)"\)\s*=\s*"(.*?)"\s*,\s*"(.*?\.csproj)"\s*,\s*"(.*?)"
                                   """;
    
    [GeneratedRegex(Pattern, RegexOptions.Multiline)]
    private static partial Regex ProjectInfoRegex();

    public bool IsRootDirectory(DirectoryInfo potentialProject) => 
        GetSolutionFilePaths(potentialProject).Length != 0;

    public bool IsManifest(string potentialProjectPath) =>
        !string.IsNullOrEmpty(potentialProjectPath) && 
        potentialProjectPath.EndsWith(SolutionFileExtension);

    public Task<ManifestBuilderResult?> BuildAsync(FileInfo projectFile, IBuilderContext context)
    {
        throw new NotImplementedException();
    }
    
    private static string[] GetSolutionFilePaths(DirectoryInfo potentialProject) => 
        Directory.GetFiles(potentialProject.FullName, SolutionFileSearchPattern, SearchOption.TopDirectoryOnly);
}