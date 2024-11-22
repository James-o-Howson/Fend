using Fend.Abstractions.Events.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Fend.Abstractions;

public static class ServiceConfiguration
{
    public static void AddCoreAbstractions(this IServiceCollection services)
    {
        services.AddTransient<IDomainEventDispatcher, DomainEventDispatcher>();
    }
}