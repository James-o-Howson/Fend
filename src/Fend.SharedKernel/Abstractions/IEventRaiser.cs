using Fend.SharedKernel.Events.Domain;

namespace Fend.SharedKernel.Abstractions;

public interface IEventRaiser
{
    IReadOnlyCollection<DomainEvent> GetEvents();
    void ClearEvents();
}