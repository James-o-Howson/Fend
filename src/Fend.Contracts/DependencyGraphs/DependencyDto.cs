namespace Fend.Contracts.DependencyGraphs;

public sealed record DependencyDto(
    string Name,
    string Version,
    IReadOnlyCollection<DependencyDto> Dependencies); 