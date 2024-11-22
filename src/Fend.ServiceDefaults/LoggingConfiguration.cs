using Fend.Infrastructure.Options;
using Fend.Infrastructure.Options.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Fend.ServiceDefaults;

public static class LoggingConfiguration
{
    public static void AddLoggingDefaults(this IHostBuilder host, IConfiguration configuration)
    {
        var options = configuration.TryLoad<LoggingOptions>();
       
        var loggerConfiguration = new LoggerConfiguration();

        if (options.WriteToConsole)
        {
            loggerConfiguration = loggerConfiguration.WriteTo.Console();
        }
        
        if (options.WriteToFile)
        {
            loggerConfiguration = loggerConfiguration.WriteTo.File(
                $"Logs/{options.IdentifierName}_.txt", 
                rollingInterval: RollingInterval.Day, 
                rollOnFileSizeLimit: true);
        }
        
        Log.Logger = loggerConfiguration.CreateLogger();

        host.UseSerilog();
    }
}