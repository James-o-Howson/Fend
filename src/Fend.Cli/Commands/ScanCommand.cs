using Cocona;
using Fend.Cli.Validation;
using Fend.Domain.DependencyGraphs;
using Fend.Domain.DependencyGraphs.Builders;

namespace Fend.Cli.Commands;

internal sealed class ScanCommand
{
    private readonly IScanner _scanner;

    public ScanCommand(IScanner scanner)
    {
        _scanner = scanner;
    }

    [Command("scan", 
        Aliases = ["s"], 
        Description = "Scan a project directory to construct a dependency graph")]
    public async Task Scan([Argument][PathExists] string? target)
    {
        target ??= Directory.GetCurrentDirectory();
        
        var directory = Directory.CreateDirectory(target);
        
        await _scanner.ScanAsync(directory, CancellationToken.None);
    }
}