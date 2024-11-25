namespace Fend.Application.Contracts.Dependencies;

public sealed record DependencyGraphDto(DependencyDto Root, string Path);