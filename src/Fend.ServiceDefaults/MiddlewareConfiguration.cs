using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Fend.ServiceDefaults;

public static class MiddlewareConfiguration
{
    public static void UseMiddlewareDefaults(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseOpenApi();
            app.UseSwaggerUi();
        }
        
        app.UseSerilogRequestLogging();
        app.UseCors(c => c.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        app.UseHttpsRedirection();
        app.UseHealthChecks("/health");
        app.MapControllers();
    }
}