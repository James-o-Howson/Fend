using Fend.Application.Events.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Fend.Application;

public static class ServiceConfiguration
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddTransient<IDomainEventDispatcher, DomainEventDispatcher>();
    }
}