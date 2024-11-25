using Fend.Application.Contracts.Vulnerabilities;
using MediatR;

namespace Fend.Application.Contracts.Dependencies;

public sealed record ScanCommand(DependencyGraphDto DependencyGraph) : IRequest<VulnerabilityGraphDto>;