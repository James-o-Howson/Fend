namespace Fend.Domain.Dependencies;

public interface IDependencyGraphBuilder
{
    Task<DependencyGraph> BuildAsync(DirectoryInfo projectDirectory,
        CancellationToken cancellationToken = default);
}