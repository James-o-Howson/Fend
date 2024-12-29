using MediatR;

namespace Fend.Identity.Application.Auth.AuthorizationCodeAuth;

public sealed record AuthorizationCodeAuthCommand : IRequest<AuthenticationResult>;

internal sealed class AuthorizationCodeAuthCommandHandler : IRequestHandler<AuthorizationCodeAuthCommand, AuthenticationResult>
{
    public Task<AuthenticationResult> Handle(AuthorizationCodeAuthCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}