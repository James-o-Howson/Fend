using Fend.Domain.DependencyGraphs.Builders;

namespace Fend.Cli.Commands;

internal interface IScanner
{
    Task ScanAsync(DirectoryInfo target, CancellationToken cancellationToken = default);
}

internal sealed class Scanner : IScanner
{
    private readonly IDependencyGraphBuilder _dependencyGraphBuilder;

    public Scanner(IDependencyGraphBuilder dependencyGraphBuilder)
    {
        _dependencyGraphBuilder = dependencyGraphBuilder;
    }

    public async Task ScanAsync(DirectoryInfo target, CancellationToken cancellationToken = default)
    {
        await _dependencyGraphBuilder.BuildAsync(target, cancellationToken);
    }
}