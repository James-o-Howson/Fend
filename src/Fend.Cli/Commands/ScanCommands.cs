using Cocona;
using Cocona.Application;
using Fend.Commands.RunScan;
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
        [Argument] string? target,
        [Option("o")] string? output)
    {
        await _mediator.Send(
            new RunScanCommand(target, output),
            contextAccessor.CancellationToken());
    }
}