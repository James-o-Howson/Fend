using Fend.Cli.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Fend.Cli;

internal static class ServiceConfiguration
{
    public static void AddCli(this IServiceCollection services)
    {
        services.AddTransient<IScanner, Scanner>();
    }
}