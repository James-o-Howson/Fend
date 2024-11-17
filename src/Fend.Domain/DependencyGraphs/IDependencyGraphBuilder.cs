namespace Fend.Domain.DependencyGraphs;

public interface IDependencyGraphBuilder
{
    Task<DependencyGraph> BuildAsync(DirectoryInfo projectDirectory,
        CancellationToken cancellationToken = default);
}