using Fend.Application.Contracts.Dependencies;
using FluentValidation;

namespace Fend.Dependencies.Application.DependencyGraphs.Scan;

internal sealed class ScanCommandValidator : AbstractValidator<ScanCommand>
{
    public ScanCommandValidator()
    {
        
    }
}