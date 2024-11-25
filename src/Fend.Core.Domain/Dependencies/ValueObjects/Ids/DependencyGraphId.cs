using Fend.Core.SharedKernel.Abstractions;

namespace Fend.Core.Domain.Dependencies.ValueObjects.Ids;

public readonly record struct DependencyGraphId() : IId
{
    public Guid Value { get; private init; } = Guid.NewGuid();

    public static DependencyGraphId New => new();
    public static DependencyGraphId Explicit(Guid value) => new()
    {
        Value = value
    };
}