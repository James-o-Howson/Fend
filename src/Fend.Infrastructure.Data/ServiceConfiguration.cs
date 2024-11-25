using Fend.Core.SharedKernel.Abstractions;
using Fend.Infrastructure.Data.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fend.Infrastructure.Data;

public static class ServiceConfiguration
{
    public static void AddDatabase<TDbContextInterface, TDbContext>(this IServiceCollection services) 
        where TDbContext : ModuleDbContext, TDbContextInterface 
        where TDbContextInterface : class, IUnitOfWork
    {
        services.AddScoped<ISaveChangesInterceptor, AuditableEntitySaveChangesInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DomainEventsSaveChangesInterceptor>();
        
        services.AddDbContext<TDbContext>((sp, options) =>
        {
            var databaseOptions = sp.GetRequiredService<IOptions<DatabaseOptions>>().Value;
            
            options.UseSqlite(databaseOptions.ConnectionString).LogTo(Console.WriteLine, LogLevel.Information);
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
        });

        services.AddScoped(provider => provider.GetRequiredService<TDbContext>());
        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<TDbContext>());
    }
}