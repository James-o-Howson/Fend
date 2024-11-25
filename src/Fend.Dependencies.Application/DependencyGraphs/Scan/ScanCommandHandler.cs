using Fend.Application.Contracts.Dependencies;
using Fend.Application.Contracts.Vulnerabilities;
using MediatR;

namespace Fend.Dependencies.Application.DependencyGraphs.Scan;

internal sealed class ScanCommandHandler : IRequestHandler<ScanCommand, VulnerabilityGraphDto>
{
    public Task<VulnerabilityGraphDto> Handle(ScanCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}