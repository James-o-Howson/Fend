using Fend.Identity.Data;
using Fend.Infrastructure.Options;
using Microsoft.AspNetCore.DataProtection;
using OpenIddict.Validation.AspNetCore;

namespace Fend.Identity;

internal static class ServiceConfiguration
{
    public static void AddOpenIddict(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
        });

        services.AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore()
                    .UseDbContext<IdentityDbContext>();
            })
            .AddServer(options =>
            {
                options.SetAuthorizationEndpointUris("authorize")
                    .SetIntrospectionEndpointUris("introspect")
                    .SetTokenEndpointUris("token");
                
                options.AllowAuthorizationCodeFlow()
                    .AllowRefreshTokenFlow();
                
                options.AddDevelopmentSigningCertificate();
                
                options.UseAspNetCore()
                    .EnableAuthorizationEndpointPassthrough();
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