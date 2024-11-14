using Fend.Domain.DependencyGraphs.ValueObjects;

namespace Fend.DependencyGraph.Manifests.Nuget.CSharp;

internal interface ICSharpProjectManifestParser
{
    HashSet<DependencyItem> ParseAsync(string projectContent);
}