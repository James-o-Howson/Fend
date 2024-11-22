using Fend.Api.Services;
using Fend.Abstractions.Behaviours;
using Fend.Abstractions.Interfaces;
using Fend.Dependencies.Commands;
using FluentValidation;
using MediatR;

namespace Fend.Api;

internal static class ServiceConfiguration
{
    public static void AddApiServices(this IServiceCollection services)
    {
        services.AddMediatr();
        services.AddValidatorsFromAssemblies([CommandsAssembly.Assembly]);

        services.AddScoped<IUser, CurrentUser>();
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