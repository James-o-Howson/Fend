using Fend.Cli.Commands.Scan;
using Microsoft.Extensions.DependencyInjection;

namespace Fend.Cli;

internal static class ServiceConfiguration
{
    public static void AddCli(this IServiceCollection services)
    {
        services.AddTransient<IScanner, Scanner>();
    }
}