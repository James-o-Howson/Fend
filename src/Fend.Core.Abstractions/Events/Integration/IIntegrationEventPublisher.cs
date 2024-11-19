using Fend.Core.SharedKernel.Events.Integration;

namespace Fend.Core.Abstractions.Events.Integration;

public interface IIntegrationEventPublisher
{
    Task PublishAsync(IntegrationEvent integrationEvent, CancellationToken cancellationToken = default);
}