namespace Fend.Scanner.Domain.Graphs;

public interface IDependencyGraphBuilder
{
    Task<DependencyGraph> BuildAsync(DirectoryInfo projectDirectory,
        CancellationToken cancellationToken = default);
}