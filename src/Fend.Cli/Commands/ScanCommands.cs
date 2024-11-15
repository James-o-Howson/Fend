using System.Text.Json;
using System.Text.Json.Serialization;
using Cocona;
using Cocona.Application;
using Fend.Abstractions;
using Fend.Abstractions.Commands;
using Fend.Cli.Validation;
using Fend.Commands.Scans.RunDependencyScan;
using Fend.Contracts.DependencyGraphs;

namespace Fend.Cli.Commands;

internal sealed class ScanCommands
{
    private readonly ICommandHandler<RunDependencyScanCommand, DependencyGraphDto> _scanCommandHandler;
    private readonly IJsonSerializer _jsonSerializer;
    private readonly IFileWriter _fileWriter;

    public ScanCommands(ICommandHandler<RunDependencyScanCommand, DependencyGraphDto> scanCommandHandler,
        IJsonSerializer jsonSerializer,
        IFileWriter fileWriter)
    {
        _scanCommandHandler = scanCommandHandler;
        _jsonSerializer = jsonSerializer;
        _fileWriter = fileWriter;
    }

    [Command("scan", 
        Aliases = ["s"], 
        Description = "Scans a project directory to construct a dependency graph")]
    public async Task Scan([FromService] ICoconaAppContextAccessor contextAccessor, 
        [Argument][PathExists] string? target)
    {
        target ??= Directory.GetCurrentDirectory();
        
        var directory = Directory.CreateDirectory(target);

        var cancellationToken = contextAccessor.CancellationToken();
        var depGraph =  await _scanCommandHandler.HandleAsync(new RunDependencyScanCommand(directory), cancellationToken);
        
        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true
        };
        
        var content = _jsonSerializer.Serialize(depGraph, options);
        var outputPath = Path.Join(directory.FullName, $"Dependencies - {depGraph.Root.Name}.json"); 
        await _fileWriter.WriteAsync(outputPath, content, cancellationToken);
    }
}