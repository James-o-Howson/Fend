using System.Collections.Immutable;
using Fend.Core.SharedKernel.Base;

namespace Fend.Scanner.Domain.Graphs.ValueObjects;

public sealed class DependencyItem : ValueObject
{
    public required DependencyItemId Id { get; init; }
    public required DependencyType Type { get; init; }
    public IDictionary<string, string> Metadata { get; init; } = ImmutableDictionary<string, string>.Empty;
    
    private DependencyItem() { }

    public static DependencyItem Create(DependencyItemId id, DependencyType type) => new()
    {
        Id = id,
        Type = type
    };

    public static DependencyItem Create(DependencyItemId id, DependencyType type, IDictionary<string, string> metadata) => new()
    {
        Id = id,
        Type = type,
        Metadata = metadata.ToImmutableDictionary()
    };
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Id;
        yield return Type;
        yield return Metadata;
    }
}