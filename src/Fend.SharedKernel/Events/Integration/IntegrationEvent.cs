using System;

namespace Fend.SharedKernel.Events.Integration;

public abstract class IntegrationEvent : IIntegrationEvent
{
    public DateTime OccuredOn { get; set; } = DateTime.UtcNow;
    
    protected IntegrationEvent() { }

    protected IntegrationEvent(DateTime occuredOn)
    {
        OccuredOn = occuredOn;
    }
    
    public abstract string AssemblyQualifiedName { get; }
}