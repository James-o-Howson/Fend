using System.Xml.Linq;
using Fend.Domain.DependencyGraphs.ValueObjects;

namespace Fend.DependencyGraph.Manifests.Nuget.CSharp;

internal sealed class NugetPackageReferenceAttributeParser : ICSharpProjectManifestParser
{
    public HashSet<DependencyItem> ParseAsync(string projectContent)
    {
        ArgumentException.ThrowIfNullOrEmpty(projectContent);
        
        return ParseProject(XDocument.Parse(projectContent));
    }

    private static HashSet<DependencyItem> ParseProject(XContainer projectContainer)
    {
        var projectDependencies = projectContainer.Descendants()
            .Where(e => e.Name.LocalName == "PackageReference")
            .Select(pr =>
            {
                var dependencyName = pr.Attribute("Include")?.Value.Trim() ?? string.Empty;
                var versionElement = pr.Elements().FirstOrDefault(e => e.Name.LocalName == "Version");
                var versionValue = versionElement?.Value.Trim() ?? string.Empty;
        
                if (string.IsNullOrEmpty(versionValue))
                {
                    versionValue = pr.Attribute("Version")?.Value.Trim() ?? string.Empty;
                }
        
                var id = DependencyItemId.Create(dependencyName, versionValue);
                return DependencyItem.Create(id, DependencyType.NuGet);
            });
        
        return projectDependencies.ToHashSet();
    }
}