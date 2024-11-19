using Fend.Core.SharedKernel;
using Fend.Core.SharedKernel.Base;

namespace Fend.Scanner.Domain.Graphs.ValueObjects;

public sealed class DependencyItemId : ValueObject
{
    public required string Name { get; init; }
    public string Version { get; init; } = string.Empty;
    
    private DependencyItemId() { }

    public static DependencyItemId Create(string name, string version)
    {
        DomainException.ThrowIfNullOrWhitespace(name, nameof(name));
        DomainException.ThrowIfNullOrWhitespace(version, nameof(version));
        
        return new DependencyItemId
        {
            Name = name,
            Version = version
        };
    }
    
    public static DependencyItemId Create(string name)
    {
        DomainException.ThrowIfNullOrWhitespace(name, nameof(name));
        
        return new DependencyItemId
        {
            Name = name
        };
    }

    public override string ToString() => $"{Name}@{Version}";

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Name;
        yield return Version;
    }
}