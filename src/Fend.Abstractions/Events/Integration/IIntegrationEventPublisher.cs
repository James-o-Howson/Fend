using Fend.SharedKernel.Events.Integration;

namespace Fend.Abstractions.Events.Integration;

public interface IIntegrationEventPublisher
{
    Task PublishAsync(IntegrationEvent integrationEvent, CancellationToken cancellationToken = default);
}