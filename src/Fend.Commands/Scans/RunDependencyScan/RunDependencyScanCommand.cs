using Fend.Abstractions.Commands;
using Fend.Contracts.DependencyGraphs;
using Fend.Contracts.Scans;

namespace Fend.Commands.Scans.RunDependencyScan;

public record RunDependencyScanCommand(DirectoryInfo Target, string? OutputPath) : ICommand<ScanResultDto>;