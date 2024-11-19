using MediatR;

namespace Fend.Commands.RunScan;

public record RunScanCommand(string? Target, string? OutputPath) : IRequest;