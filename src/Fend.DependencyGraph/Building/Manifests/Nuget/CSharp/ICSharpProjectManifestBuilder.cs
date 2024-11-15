using Fend.Domain.DependencyGraphs.ValueObjects;

namespace Fend.DependencyGraph.Building.Manifests.Nuget.CSharp;

internal interface ICSharpProjectManifestBuilder
{
    HashSet<DependencyItem> ParseAsync(string projectContent);
}