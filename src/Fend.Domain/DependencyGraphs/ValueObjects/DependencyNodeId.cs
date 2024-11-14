using Fend.SharedKernel.Abstractions;

namespace Fend.Domain.DependencyGraphs.ValueObjects;

public readonly record struct DependencyNodeId() : IId
{
    public Guid Id { get; private init; } = Guid.NewGuid();
    
    public static DependencyNodeId New => new();
    public static DependencyNodeId Explicit(Guid value) => new()
    {
        Id = value
    };
}