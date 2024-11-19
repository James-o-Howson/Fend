using System.Text.RegularExpressions;
using System.Xml.Linq;
using Fend.Scanner.Domain.Graphs.ValueObjects;

namespace Fend.Scanner.Infrastructure.Manifests.Nuget.CSharp;

internal sealed partial class LocalReferenceAttributeBuilder : ICSharpProjectManifestBuilder
{
    [GeneratedRegex(@"\d+(?:\.\d+)+")]
    private static partial Regex VersionRegex();
    
    public HashSet<DependencyItem> ParseAsync(string projectContent)
    {
        ArgumentException.ThrowIfNullOrEmpty(projectContent);
        
        return ParseProject(XDocument.Parse(projectContent));
    }

    private static HashSet<DependencyItem> ParseProject(XContainer project) 
    {
        var dependencyViewModels = project.Descendants()
            .Where(e => e.Name.LocalName == "Reference")
            .Select(pr =>
            {
                var include = pr.Attribute("Include")?.Value.Trim() ?? string.Empty;
                var parts = include.Split(",");
        
                var dependencyName = string.Empty;
                var versionValue = string.Empty;
                
                switch (parts.Length)
                {
                    case > 1:
                        dependencyName = parts.First();
                        versionValue = parts[1].Trim()["Version=".Length..];
                        break;
                    case 1:
                    {
                        dependencyName = parts.First();
                        var hintPath = pr.Elements().FirstOrDefault(e => e.Name.LocalName == "HintPath");
                        if (hintPath is not null)
                        {
                            var matches = VersionRegex().Match(hintPath.Value.Trim());
                            versionValue = matches.Value;
                        }
        
                        break;
                    }
                }
                
                var id = DependencyItemId.Create(dependencyName, versionValue);
                return DependencyItem.Create(id, DependencyType.Local);
            });
        
        return dependencyViewModels.ToHashSet();
    }
}