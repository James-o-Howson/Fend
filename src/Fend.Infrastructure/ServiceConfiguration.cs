using Fend.Application.Interfaces;
using Fend.Core.SharedKernel.Abstractions;
using Fend.Infrastructure.Services;
using Fend.Infrastructure.Web;
using Microsoft.Extensions.DependencyInjection;

namespace Fend.Infrastructure;

using DateTime = Services.DateTime;

public static class ServiceConfiguration
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<IFileWriter, FileWriter>();
        services.AddTransient<IJsonSerializer, JsonSerializer>();
        services.AddTransient<IDateTime, DateTime>();
        services.AddScoped<IUser, CurrentUser>();
    }
}