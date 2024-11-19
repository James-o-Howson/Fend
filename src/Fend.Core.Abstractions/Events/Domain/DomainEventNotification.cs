﻿using Fend.Core.SharedKernel.Events.Domain;
using MediatR;

namespace Fend.Core.Abstractions.Events.Domain;

public sealed class DomainEventNotification<TDomainEvent> : INotification
    where TDomainEvent : IDomainEvent
{
    public TDomainEvent DomainEvent { get; }

    public DomainEventNotification(TDomainEvent domainEvent) => DomainEvent = domainEvent;
}