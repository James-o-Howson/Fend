using System.Collections.Concurrent;

namespace Fend.Domain.DependencyGraphs.Builders;

public interface IBuilderContext
{
    ConcurrentQueue<IManifestDependencyBuilder> Builders { get; init; }
    DirectoryInfo ProjectRootDirectory { get; init; }
    void MarkAsComplete(string path, FileInfo? fileInfo);
    bool IsAlreadyComplete(string path);
    List<IManifestDependencyBuilder> GetBuildersForFile(string filePath);
}