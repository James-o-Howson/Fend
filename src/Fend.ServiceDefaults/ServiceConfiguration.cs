using Fend.Infrastructure.Options;
using Fend.Infrastructure.Options.Configuration;
using Fend.Infrastructure.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Fend.ServiceDefaults;

public static class ServiceConfiguration
{
    public static void AddServiceDefaults(this WebApplicationBuilder application)
    {
        application.Host.AddLoggingDefaults(application.Configuration);

        application.AddSwaggerDefaults();
        application.AddExceptionHandlingDefaults();

        application.Services.AddCors();
        application.Services.AddControllers();
        application.Services.AddEndpointsApiExplorer();
        application.Services.AddHttpContextAccessor();
        application.Services.AddHealthChecks();
    }

    private static void AddExceptionHandlingDefaults(this WebApplicationBuilder application)
    {
        application.Services.AddExceptionHandler<GlobalExceptionHandler>();
        application.Services.AddProblemDetails();
    }

    private static void AddSwaggerDefaults(this WebApplicationBuilder application)
    {
        var options = application.Configuration.TryLoad<OpenApiDocumentOptions>();
        application.Services.AddOpenApiDocument(settings =>
        {
            settings.Title = options.Title;
            settings.Version = options.Version;
            settings.Description = options.Description;
        });
    }
}