namespace Fend.Core.SharedKernel.Events.Domain;

public interface IDomainEvent
{
    DateTime OccuredOn { get; }
}