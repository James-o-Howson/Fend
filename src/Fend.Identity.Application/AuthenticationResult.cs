using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace Fend.Identity.Application;

public class AuthenticationResult
{
    private static string AuthenticationScheme => OpenIddictServerAspNetCoreDefaults.AuthenticationScheme;

    public required ActionResult Value { get; init; }
    public required bool IsAuthenticated { get; init; }

    private AuthenticationResult() { }

    public static AuthenticationResult Ok(IIdentity identity) => new()
    {
        Value = new SignInResult(AuthenticationScheme, new ClaimsPrincipal(identity)),
        IsAuthenticated = true
    };

    public static AuthenticationResult Forbid(string errorDescription)
    {
        var authenticationProperties = new AuthenticationProperties(new Dictionary<string, string?>
        {
            [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = errorDescription
        });

        return new AuthenticationResult
        {
            Value = new ForbidResult(AuthenticationScheme, authenticationProperties),
            IsAuthenticated = false
        };
    }

    public static implicit operator ActionResult(AuthenticationResult authenticationResult) 
        => authenticationResult.Value;
}