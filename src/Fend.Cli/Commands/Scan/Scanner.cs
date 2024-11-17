using System.Text.Json;
using System.Text.Json.Serialization;
using Fend.Abstractions;
using Fend.Abstractions.Commands;
using Fend.Commands.Scans.RunDependencyScan;
using Fend.Contracts.Scans;

namespace Fend.Cli.Commands.Scan;

internal interface IScanner
{
    Task ScanAsync(string? target, string? output, CancellationToken cancellationToken = default);
}

internal sealed class Scanner : IScanner
{
    private static readonly JsonSerializerOptions DefaultOptions = new()
    {
        ReferenceHandler = ReferenceHandler.Preserve,
        WriteIndented = true
    };
    
    private readonly ICommandHandler<RunDependencyScanCommand, ScanResultDto> _scanCommandHandler;
    private readonly IJsonSerializer _serializer;
    private readonly IFileWriter _fileWriter;

    public Scanner(ICommandHandler<RunDependencyScanCommand, ScanResultDto> scanCommandHandler, 
        IJsonSerializer serializer, 
        IFileWriter fileWriter)
    {
        _scanCommandHandler = scanCommandHandler;
        _serializer = serializer;
        _fileWriter = fileWriter;
    }

    public async Task ScanAsync(string? target, string? output, CancellationToken cancellationToken = default)
    {
        var directory = GetScanTargetDirectory(target);

        var command = new RunDependencyScanCommand(directory, output);
        var scanResults =  await _scanCommandHandler.HandleAsync(command, cancellationToken);
        
        await WriteToFileAsync(scanResults, cancellationToken);
    }

    private static DirectoryInfo GetScanTargetDirectory(string? target) => 
        Directory.CreateDirectory(target ?? Directory.GetCurrentDirectory());

    private async Task WriteToFileAsync(ScanResultDto scanResults, CancellationToken cancellationToken)
    {
        var content = _serializer.Serialize(scanResults.DependencyGraph, DefaultOptions);
        await _fileWriter.WriteAsync(scanResults.OutputPath, content, cancellationToken);
    }
}