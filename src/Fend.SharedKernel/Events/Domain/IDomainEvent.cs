namespace Fend.SharedKernel.Events.Domain;

public interface IDomainEvent
{
    DateTime OccuredOn { get; }
}