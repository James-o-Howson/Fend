namespace Fend.Core.SharedKernel.Abstractions;

public interface IEntity<out TId> 
    where TId : IId
{
    TId Id { get; }
}