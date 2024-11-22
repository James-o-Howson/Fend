using System.Xml.Linq;
using Fend.Domain.Dependencies;
using Fend.Domain.Dependencies.ValueObjects;
using Fend.Domain.Dependencies.ValueObjects.Ids;

namespace Fend.Scanner.Infrastructure.Manifests.Nuget.CSharp;

internal sealed class NugetPackageReferenceAttributeBuilder : ICSharpProjectManifestBuilder
{
    public HashSet<Dependency> ParseAsync(string projectContent)
    {
        ArgumentException.ThrowIfNullOrEmpty(projectContent);
        
        return ParseProject(XDocument.Parse(projectContent));
    }

    private static HashSet<Dependency> ParseProject(XContainer projectContainer)
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
        
                var id = DependencyId.Create(dependencyName, versionValue);
                return Dependency.Create(id, DependencyType.NuGet);
            });
        
        return projectDependencies.ToHashSet();
    }
}