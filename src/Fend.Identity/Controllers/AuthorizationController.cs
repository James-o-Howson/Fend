using Fend.Identity.Application.Auth.AuthorizationCodeAuth;
using Fend.Identity.Application.Auth.DeviceCodeAuth;
using Fend.Identity.Application.Auth.RefreshTokenAuth;
using Fend.Infrastructure.Web;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;

namespace Fend.Identity.Controllers;

internal sealed class AuthorizationController : ApiControllerBase
{
    /// <summary>
    /// This Token endpoint is required to support non-interactive flows like resource
    /// owner password, client credentials and refresh tokens. This is also where we support our own custom
    /// grant type for windows domain login.
    /// 
    /// Warning: this action is decorated with IgnoreAntiforgeryTokenAttribute to override
    /// the global antiforgery token validation policy applied by the MVC modules stack,
    /// which is required for this stateless OAuth2/OIDC token endpoint to work correctly.
    /// To prevent effective CSRF/session fixation attacks, this action MUST NOT return
    /// an authentication cookie or try to establish an ASP.NET Core user session.
    /// </summary>
    /// <returns>
    /// The created SignInResult for the response.
    /// Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
    /// </returns>
    /// <exception cref="InvalidOperationException"></exception>
    [AllowAnonymous, HttpPost("~/connect/token")]
    [IgnoreAntiforgeryToken]
    [Produces("application/json")]
    public async Task<ActionResult> Exchange(CancellationToken cancellationToken)
    {
        var request = HttpContext.GetOpenIddictServerRequest();
        if (request is null) return NotFound();

        var authenticationResult = await RouteAsync(request, cancellationToken);
        if (authenticationResult is null) return NotFound("No appropriate request handler found for this request. This is usually due to an unsupported grant type.");

        return authenticationResult;
    }

    public async Task<ActionResult?> RouteAsync(OpenIddictRequest request, CancellationToken cancellationToken)
    {
        if (request.IsDeviceCodeGrantType())
        {
            var deviceCodeCommand = new DeviceCodeAuthCommand();
            return await Mediator.Send(deviceCodeCommand, cancellationToken);
        }

        if (request.IsRefreshTokenGrantType())
        {
            var deviceCodeCommand = new RefreshTokenAuthCommand();
            return await Mediator.Send(deviceCodeCommand, cancellationToken);
        }
        
        if (request.IsAuthorizationCodeFlow())
        {
            var deviceCodeCommand = new AuthorizationCodeAuthCommand();
            return await Mediator.Send(deviceCodeCommand, cancellationToken);
        }

        return null;
    }
}