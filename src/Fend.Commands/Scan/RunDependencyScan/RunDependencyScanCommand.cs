using MediatR;

namespace Fend.Commands.Scan.RunDependencyScan;

public record RunDependencyScanCommand(string? Target, string? OutputPath) : IRequest;