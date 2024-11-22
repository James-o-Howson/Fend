using Fend.Abstractions.Interfaces;
using Fend.Infrastructure.Services;
using Fend.SharedKernel.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using DateTime = Fend.Infrastructure.Services.DateTime;

namespace Fend.Infrastructure;

public static class ServiceConfiguration
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<IFileWriter, FileWriter>();
        services.AddTransient<IJsonSerializer, JsonSerializer>();
        services.AddTransient<IDateTime, DateTime>();
    }
}