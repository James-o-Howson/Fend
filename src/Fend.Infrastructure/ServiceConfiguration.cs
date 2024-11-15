using Fend.Abstractions;
using Fend.SharedKernel.Abstractions;
using Microsoft.Extensions.DependencyInjection;

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