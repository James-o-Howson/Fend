using Fend.Core.Domain.Dependencies;

namespace Fend.Scanner.Infrastructure.Manifests.Nuget.CSharp;

internal interface ICSharpProjectManifestBuilder
{
    HashSet<Dependency> ParseAsync(string projectContent);
}