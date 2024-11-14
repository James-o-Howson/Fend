namespace Fend.Domain.DependencyGraphs.Builders;

public interface IDependencyGraphBuilder
{
    Task<DependencyGraph?> BuildAsync(DirectoryInfo projectDirectory, CancellationToken cancellationToken = default);
}