using Fend.Core.SharedKernel;
using Fend.Core.SharedKernel.Abstractions;
using Fend.Core.SharedKernel.Base;

namespace Fend.Core.Domain.Dependencies.ValueObjects.Ids;

public sealed class DependencyId : ValueObject, IId
{
    public required string Name { get; init; }
    public string Version { get; init; } = string.Empty;
    
    private DependencyId() { }

    public static DependencyId Create(string name, string version)
    {
        DomainException.ThrowIfNullOrWhitespace(name, nameof(name));
        DomainException.ThrowIfNullOrWhitespace(version, nameof(version));
        
        return new DependencyId
        {
            Name = name,
            Version = version
        };
    }
    
    public static DependencyId Create(string name)
    {
        DomainException.ThrowIfNullOrWhitespace(name, nameof(name));
        
        return new DependencyId
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