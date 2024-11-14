using Cocona;
using Fend.Cli.Validation;
using Fend.Domain.DependencyGraphs;
using Fend.Domain.DependencyGraphs.Builders;

namespace Fend.Cli.Commands;

internal sealed class ScanCommand
{
    private readonly IDependencyGraphBuilder _dependencyGraphBuilder;

    public ScanCommand(IDependencyGraphBuilder dependencyGraphBuilder)
    {
        _dependencyGraphBuilder = dependencyGraphBuilder;
    }

    [Command("scan", 
        Aliases = ["s"], 
        Description = "Scan a project directory to construct a dependency graph")]
    public Task Scan([Argument][PathExists] string? target)
    {
        target ??= Directory.GetCurrentDirectory();
        
        var directory = Directory.CreateDirectory(target);
        
        _dependencyGraphBuilder.BuildAsync(directory);
        
        return Task.CompletedTask;
    }
}