using System.Text.Json;
using Fend.Abstractions;
using Fend.Abstractions.Commands;
using Fend.Contracts.DependencyGraphs;
using Fend.Contracts.Scans;
using Fend.Domain.DependencyGraphs;

namespace Fend.Cli.Commands.Scan.RunDependencyScan;

internal sealed class RunDependencyScanHandler : ICommandHandler<RunDependencyScanCommand>
{
    private static readonly JsonSerializerOptions DefaultOptions = new()
    {
        WriteIndented = true,
    };
    
    private const string FileName = "vulns.json";
    
    private readonly IJsonSerializer _serializer;
    private readonly IFileWriter _fileWriter;
    private readonly IDependencyGraphBuilder _graphBuilder;

    public RunDependencyScanHandler(IJsonSerializer serializer,
        IFileWriter fileWriter, 
        IDependencyGraphBuilder graphBuilder)
    {
        _serializer = serializer;
        _fileWriter = fileWriter;
        _graphBuilder = graphBuilder;
    }

    public async Task HandleAsync(RunDependencyScanCommand command, CancellationToken cancellationToken = default)
    {
        var targetDirectory = GetScanTargetDirectory(command.Target);
        
        var dependencyGraph = await _graphBuilder.BuildAsync(targetDirectory, cancellationToken);

        var vulnDocument = new ScanResultDto(dependencyGraph.ToDto(), GetOutputPath(targetDirectory, command.OutputPath));
        
        await WriteToFileAsync(vulnDocument, cancellationToken);
    }
    
    private static string GetOutputPath(DirectoryInfo scanTargetDir, string? outputPath) =>
        string.IsNullOrWhiteSpace(outputPath) ? 
            Path.Join(scanTargetDir.FullName, FileName) : 
            outputPath;
    
    private async Task WriteToFileAsync(ScanResultDto scanResults, CancellationToken cancellationToken)
    {
        var content = _serializer.Serialize(scanResults.DependencyGraph, DefaultOptions);
        await _fileWriter.WriteAsync(scanResults.OutputPath, content, cancellationToken);
    }
    
    private static DirectoryInfo GetScanTargetDirectory(string? target) => 
        Directory.CreateDirectory(target ?? Directory.GetCurrentDirectory());
}