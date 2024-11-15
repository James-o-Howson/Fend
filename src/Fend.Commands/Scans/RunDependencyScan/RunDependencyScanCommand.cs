using Fend.Abstractions.Commands;
using Fend.Contracts.DependencyGraphs;

namespace Fend.Commands.Scans.RunDependencyScan;

public record RunDependencyScanCommand(DirectoryInfo Target) : ICommand<DependencyGraphDto>;