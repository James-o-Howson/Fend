namespace Fend.SharedKernel.Events.Integration;

public interface IIntegrationEvent
{
    DateTime OccuredOn { get; set; }
    string AssemblyQualifiedName { get; }
}