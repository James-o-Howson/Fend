﻿using System.Text.Json;
using Fend.Application.Contracts.Dependencies;
using Fend.Application.Interfaces;
using Fend.Commands.RunScan.Contracts;
using Fend.Core.Domain.Dependencies;
using MediatR;

namespace Fend.Commands.RunScan;

public record RunScanCommand(string? Target, string? OutputPath) : IRequest;

internal sealed class RunScanCommandHandler : IRequestHandler<RunScanCommand>
{
    private static readonly JsonSerializerOptions DefaultOptions = new()
    {
        WriteIndented = true,
    };
    
    private const string FileName = "vulns.json";
    
    private readonly IJsonSerializer _serializer;
    private readonly IFileWriter _fileWriter;
    private readonly IDependencyGraphBuilder _graphBuilder;

    public RunScanCommandHandler(IJsonSerializer serializer,
        IFileWriter fileWriter, 
        IDependencyGraphBuilder graphBuilder)
    {
        _serializer = serializer;
        _fileWriter = fileWriter;
        _graphBuilder = graphBuilder;
    }

    public async Task Handle(RunScanCommand command, CancellationToken cancellationToken = default)
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