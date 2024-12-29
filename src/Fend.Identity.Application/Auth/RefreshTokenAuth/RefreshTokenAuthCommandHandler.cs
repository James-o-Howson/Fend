using MediatR;

namespace Fend.Identity.Application.Auth.RefreshTokenAuth;

public sealed record RefreshTokenAuthCommand : IRequest<AuthenticationResult>;

internal sealed class RefreshTokenAuthCommandHandler : IRequestHandler<RefreshTokenAuthCommand, AuthenticationResult>
{
    public Task<AuthenticationResult> Handle(RefreshTokenAuthCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}