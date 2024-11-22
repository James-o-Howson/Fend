using Fend.SharedKernel.Abstractions;
using Fend.SharedKernel.Events.Domain;

namespace Fend.SharedKernel.Base;

public abstract class AggregateRoot<TId> : IEntity<TId>, IEventRaiser
    where TId : IId
{
    private readonly List<DomainEvent> _events = [];
    
    protected AggregateRoot() { /*Required by EF Core*/ } 

    public abstract TId Id { get; protected set; }
    public IReadOnlyCollection<DomainEvent> GetEvents() => _events.ToList();
    public void ClearEvents() => _events.Clear();
    protected void RaiseEvent(DomainEvent @event) => _events.Add(@event);
}