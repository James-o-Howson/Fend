using Fend.Abstractions.Commands;

namespace Fend.Cli.Commands.Scan.RunDependencyScan;

public record RunDependencyScanCommand(string? Target, string? OutputPath) : ICommand;