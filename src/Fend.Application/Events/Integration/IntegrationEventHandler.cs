using Fend.Core.SharedKernel.Events.Integration;
using MediatR;

namespace Fend.Application.Events.Integration;

public abstract class IntegrationEventHandler<TIntegrationEvent> : INotificationHandler<IntegrationEventNotification<TIntegrationEvent>>
    where TIntegrationEvent : IIntegrationEvent
{
    public async Task Handle(IntegrationEventNotification<TIntegrationEvent> notification, CancellationToken cancellationToken)
    {
        await HandleAsync(notification.IntegrationEvent, cancellationToken);
    }
    
    protected abstract Task HandleAsync(TIntegrationEvent integrationEvent, CancellationToken cancellationToken);
}