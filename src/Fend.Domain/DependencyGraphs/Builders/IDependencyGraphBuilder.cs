namespace Fend.Domain.DependencyGraphs.Builders;

public interface IDependencyGraphBuilder
{
    Task BuildAsync(DirectoryInfo projectDirectory, CancellationToken cancellationToken = default);
}