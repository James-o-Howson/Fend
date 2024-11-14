using Fend.SharedKernel.Abstractions;

namespace Fend.Domain.DependencyGraphs.ValueObjects;

public readonly record struct DependencyGraphId() : IId
{
    public Guid Id { get; private init; } = Guid.NewGuid();

    public static DependencyGraphId New => new();
    public static DependencyGraphId Explicit(Guid value) => new()
    {
        Id = value
    };
}