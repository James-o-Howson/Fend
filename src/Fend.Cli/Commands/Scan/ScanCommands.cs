using Cocona;
using Cocona.Application;
using Fend.Cli.Validation;

namespace Fend.Cli.Commands.Scan;

internal sealed class ScanCommands
{
    private readonly IScanner _scanner;

    public ScanCommands(IScanner scanner)
    {
        _scanner = scanner;
    }

    [Command("scan", 
        Aliases = ["s"], 
        Description = "Scans a project directory to construct a dependency graph")]
    public async Task Scan([FromService] ICoconaAppContextAccessor contextAccessor, 
        [Argument][PathExistsOrNull] string? target,
        [Option("o")] string? output)
    {
        await _scanner.ScanAsync(target, output, contextAccessor.CancellationToken());
    }
}