using Fend.Scanner.Domain.Graphs.ValueObjects;

namespace Fend.Scanner.Infrastructure.Manifests.Nuget.CSharp;

internal interface ICSharpProjectManifestBuilder
{
    HashSet<DependencyItem> ParseAsync(string projectContent);
}