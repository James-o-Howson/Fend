using Cocona;
using Cocona.Application;
using Fend.Abstractions.Commands;
using Fend.Cli.Commands.Scan.RunDependencyScan;
using Fend.Cli.Validation;

namespace Fend.Cli.Commands;

internal sealed class ScanCommands
{
    private readonly ICommandHandler<RunDependencyScanCommand> _scanCommandHandler;

    public ScanCommands(ICommandHandler<RunDependencyScanCommand> scanCommandHandler)
    {
        _scanCommandHandler = scanCommandHandler;
    }
    
    [Command("scan", 
        Aliases = ["s"], 
        Description = "Scans a project directory to construct a dependency graph")]
    public async Task Scan([FromService] ICoconaAppContextAccessor contextAccessor, 
        [Argument][PathExistsOrNull] string? target,
        [Option("o")] string? output)
    {
        await _scanCommandHandler.HandleAsync(
            new RunDependencyScanCommand(target, output), 
            contextAccessor.CancellationToken());
    }
}