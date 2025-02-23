﻿namespace Fend.SharedKernel.Abstractions;

public interface IEntity<out TId> 
    where TId : IId, new()
{
    TId Id { get; }
}