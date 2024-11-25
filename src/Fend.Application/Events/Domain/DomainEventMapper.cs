﻿using Fend.Core.SharedKernel.Events.Domain;

namespace Fend.Application.Events.Domain;

public static class DomainEventMapper
{
    public static object ToNotification(this IDomainEvent domainEvent)
    {
        var notificationType = typeof(DomainEventNotification<>)
            .MakeGenericType(domainEvent.GetType());
            
        var notification = Activator.CreateInstance(notificationType, domainEvent);
        ArgumentNullException.ThrowIfNull(notification);
        
        return notification;
    }
}