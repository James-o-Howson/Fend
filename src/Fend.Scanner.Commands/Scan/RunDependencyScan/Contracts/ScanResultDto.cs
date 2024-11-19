namespace Fend.Commands.Scan.RunDependencyScan.Contracts;

public sealed record ScanResultDto(DependencyDto DependencyGraph, string OutputPath);