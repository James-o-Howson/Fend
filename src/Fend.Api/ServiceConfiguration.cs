using Fend.Application.Behaviours;
using Fend.Dependencies.Application;
using Fend.Dependencies.Application.Abstractions;
using Fend.Dependencies.Data;
using Fend.Infrastructure.Data;
using FluentValidation;
using MediatR;

namespace Fend.Api;

internal static class ServiceConfiguration
{
    public static void AddApiServices(this IServiceCollection services)
    {
        services.AddMediatr();
        services.AddValidatorsFromAssemblies([CommandsAssembly.Assembly]);
        services.AddDatabase<IDependenciesDbContext, DependenciesDbContext>();
    }
    
    private static void AddMediatr(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(CommandsAssembly.Assembly);
            
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        });
    }
}