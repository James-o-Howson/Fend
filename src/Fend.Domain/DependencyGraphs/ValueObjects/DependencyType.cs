﻿namespace Fend.Domain.DependencyGraphs.ValueObjects;

public enum DependencyType
{
    Project,
    NuGet,
    Local,
    Npm
}