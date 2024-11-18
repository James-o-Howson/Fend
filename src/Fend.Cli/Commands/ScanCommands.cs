using Cocona;
using Cocona.Application;
using Fend.Cli.Validation;
using Fend.Commands.Scan.RunDependencyScan;
using MediatR;

namespace Fend.Cli.Commands;

internal sealed class ScanCommands
{
    private readonly IMediator _mediator;
    
    public ScanCommands(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Command("scan", 
        Aliases = ["s"], 
        Description = "Scans a project directory to construct a dependency graph")]
    public async Task Scan([FromService] ICoconaAppContextAccessor contextAccessor, 
        [Argument][PathExistsOrNull] string? target,
        [Option("o")] string? output)
    {
        await _mediator.Send(
            new RunDependencyScanCommand(target, output), 
            contextAccessor.CancellationToken());
    }
}