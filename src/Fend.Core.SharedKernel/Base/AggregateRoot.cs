using Fend.Core.SharedKernel.Abstractions;
using Fend.Core.SharedKernel.Events.Domain;

namespace Fend.Core.SharedKernel.Base;

public abstract class AggregateRoot<TId> : IEntity<TId> 
    where TId : IId, new()
{
    private readonly List<DomainEvent> _events = [];
    
    protected AggregateRoot() { /*Required by EF Core*/ } 

    public TId Id { get; protected set; } = new();
    public IReadOnlyCollection<DomainEvent> GetEvents() => _events.ToList();
    public void ClearEvents() => _events.Clear();
    protected void RaiseEvent(DomainEvent @event) => _events.Add(@event);
}