using Fend.Core.SharedKernel.Events.Integration;

namespace Fend.Application.Events.Integration;

public interface IIntegrationEventPublisher
{
    Task PublishAsync(IntegrationEvent integrationEvent, CancellationToken cancellationToken = default);
}