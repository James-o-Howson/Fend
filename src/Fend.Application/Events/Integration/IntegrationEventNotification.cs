using Fend.Core.SharedKernel.Events.Integration;
using MediatR;

namespace Fend.Application.Events.Integration;

public sealed class IntegrationEventNotification<TIntegrationEvent> : INotification
    where TIntegrationEvent : IIntegrationEvent
{
    public TIntegrationEvent IntegrationEvent { get; }

    public IntegrationEventNotification(TIntegrationEvent integrationEvent) => IntegrationEvent = integrationEvent;
}