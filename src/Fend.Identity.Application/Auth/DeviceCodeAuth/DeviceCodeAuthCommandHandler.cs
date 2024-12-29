using MediatR;

namespace Fend.Identity.Application.Auth.DeviceCodeAuth;

public sealed record DeviceCodeAuthCommand : IRequest<AuthenticationResult>;

internal sealed class DeviceCodeAuthCommandHandler : IRequestHandler<DeviceCodeAuthCommand, AuthenticationResult>
{
    public Task<AuthenticationResult> Handle(DeviceCodeAuthCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}