namespace Fend.Commands.Scan.RunDependencyScan.Contracts;

public sealed record DependencyDto(
    string Name,
    string Version,
    IReadOnlyCollection<DependencyDto> Dependencies); 