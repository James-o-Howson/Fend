using Fend.Contracts.DependencyGraphs;

namespace Fend.Contracts.Scans;

public sealed record ScanResultDto(DependencyDto DependencyGraph, string OutputPath);