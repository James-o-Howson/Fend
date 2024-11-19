using Fend.Core.SharedKernel.Abstractions;

namespace Fend.Scanner.Domain.Graphs.ValueObjects;

public readonly record struct DependencyNodeId() : IId
{
    public Guid Id { get; private init; } = Guid.NewGuid();
    
    public static DependencyNodeId New => new();
    public static DependencyNodeId Explicit(Guid value) => new()
    {
        Id = value
    };
}