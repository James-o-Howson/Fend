using Fend.Core.SharedKernel.Events.Domain;

namespace Fend.Core.SharedKernel.Abstractions;

public interface IEventRaiser
{
    IReadOnlyCollection<DomainEvent> GetEvents();
    void ClearEvents();
}