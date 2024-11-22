using Fend.SharedKernel.Events.Domain;

namespace Fend.Abstractions.Events.Domain;

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