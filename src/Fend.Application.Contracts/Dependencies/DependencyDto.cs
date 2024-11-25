namespace Fend.Application.Contracts.Dependencies;

public sealed record DependencyDto(
    string Name,
    string Version,
    IReadOnlyCollection<DependencyDto> Dependencies); 