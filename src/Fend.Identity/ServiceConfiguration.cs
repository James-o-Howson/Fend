using Fend.Application.Behaviours;
using Fend.Core.SharedKernel.Abstractions;
using Fend.Identity.Application;
using Fend.Identity.Application.Abstractions;
using Fend.Identity.Data;
using Fend.Infrastructure.Data;
using Fend.Infrastructure.Data.Interceptors;
using Fend.Infrastructure.Options;
using MediatR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using OpenIddict.Validation.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Fend.Identity;

internal static class ServiceConfiguration
{
    public static void AddIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuth(configuration);
        services.AddMediatr();
        services.AddDatabase();
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<IdentityContext>()
            .AddDefaultTokenProviders()
            .AddDefaultUI();
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
    
    private static void AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
        });

        services.AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore()
                    .UseDbContext<IdentityContext>();
            })
            .AddServer(options =>
            {
                options.SetAuthorizationEndpointUris("connect/authorize")
                    .SetVerificationEndpointUris("connect/verify")
                    .SetDeviceEndpointUris("connect/device")
                    .SetIntrospectionEndpointUris("connect/introspect")
                    .SetTokenEndpointUris("connect/token");
                
                // Mark the "email", "profile" and "roles" scopes as supported scopes.
                options.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Roles);
                
                options.AllowAuthorizationCodeFlow()
                    .AllowRefreshTokenFlow()
                    .AllowDeviceCodeFlow();
                
                options.AddDevelopmentSigningCertificate();
                options.AddDevelopmentEncryptionCertificate();
                
                options.UseAspNetCore()
                    .EnableAuthorizationEndpointPassthrough()
                    .EnableTokenEndpointPassthrough()
                    .EnableVerificationEndpointPassthrough()
                    .EnableStatusCodePagesIntegration();
            })
            .AddValidation(options =>
            {
                options.UseLocalServer();
                options.UseAspNetCore();
                options.UseDataProtection(Constants.ApplicationName);
                
                var identityOptions = configuration.TryLoad<IdentityOptions>();
                options.SetIssuer(identityOptions.Url);
            });
    }

    private static void AddDatabase(this IServiceCollection services)
    {
        services.AddScoped<ISaveChangesInterceptor, AuditableEntitySaveChangesInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DomainEventsSaveChangesInterceptor>();
        
        services.AddDbContext<IdentityContext>((sp, options) =>
        {
            var databaseOptions = sp.GetRequiredService<IOptions<DatabaseOptions>>().Value;
            
            options.UseSqlite(databaseOptions.ConnectionString).LogTo(Console.WriteLine, LogLevel.Information);
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            options.UseOpenIddict();
        });

        services.AddScoped<IIdentityDbContext>(provider => provider.GetRequiredService<IdentityContext>());
        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<IdentityContext>());
    }
    
    private static void UseDataProtection(this OpenIddictValidationBuilder builder, string applicationDiscriminator) 
        => builder.UseDataProtection().Services.ConfigureApplicationDiscriminator(applicationDiscriminator);
    
    private static void ConfigureApplicationDiscriminator(this IServiceCollection services,
        string applicationDiscriminator)
    {
        services.Configure<DataProtectionOptions>(dataProtectionOptions =>
        {
            dataProtectionOptions.ApplicationDiscriminator = applicationDiscriminator;
        });
    }
}