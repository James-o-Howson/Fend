﻿namespace Fend.Scanner.Domain.Graphs.Building;

public interface IManifestDependencyBuilder
{
    bool IsRootDirectory(DirectoryInfo potentialProject);
    bool IsManifest(string potentialProjectPath);
    Task<ManifestBuilderResult?> BuildAsync(FileInfo projectFile, IBuilderContext context,
        CancellationToken cancellationToken = default);
}