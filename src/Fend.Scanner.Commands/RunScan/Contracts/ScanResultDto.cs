namespace Fend.Commands.RunScan.Contracts;

public sealed record ScanResultDto(DependencyDto DependencyGraph, string OutputPath);