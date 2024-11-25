namespace Fend.Core.Domain.Dependencies;

public interface IDependencyGraphBuilder
{
    Task<DependencyGraph> BuildAsync(DirectoryInfo projectDirectory,
        CancellationToken cancellationToken = default);
}