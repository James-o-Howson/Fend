﻿namespace Fend.DependencyGraph.Building.Manifests.Nuget.CSharp;

public class DotNetProjectManifest
{
    public string ProjectId { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string FilePath { get; init; } = string.Empty;

    public bool Exists => File.Exists(FilePath);
    public string Name => Path.GetFileName(FilePath);
    public string Content => File.ReadAllText(FilePath);
    public FileInfo FileInfo => new(FilePath);
}