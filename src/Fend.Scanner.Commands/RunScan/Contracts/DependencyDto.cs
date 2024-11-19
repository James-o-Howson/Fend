namespace Fend.Commands.RunScan.Contracts;

public sealed record DependencyDto(
    string Name,
    string Version,
    IReadOnlyCollection<DependencyDto> Dependencies); 