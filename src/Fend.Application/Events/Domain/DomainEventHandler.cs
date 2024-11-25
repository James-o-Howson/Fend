﻿using Fend.Core.SharedKernel.Events.Domain;
using MediatR;

namespace Fend.Application.Events.Domain;

public abstract class DomainEventHandler<TDomainEvent> : INotificationHandler<DomainEventNotification<TDomainEvent>>
    where TDomainEvent : IDomainEvent
{
    public async Task Handle(DomainEventNotification<TDomainEvent> notification, CancellationToken cancellationToken)
    {
        await HandleAsync(notification.DomainEvent, cancellationToken);
    }
    
    protected abstract Task HandleAsync(TDomainEvent domainEvent, CancellationToken cancellationToken);
}