namespace Fend.SharedKernel.Events.Domain;

public abstract class DomainEvent : IDomainEvent
{
    public DateTime OccuredOn { get; }

    protected DomainEvent(DateTime occuredOn)
    {
        OccuredOn = occuredOn;
    }
}