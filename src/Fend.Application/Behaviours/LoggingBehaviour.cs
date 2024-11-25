using Fend.Application.Interfaces;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Fend.Application.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger<LoggingBehaviour<TRequest>> _logger;
    private readonly IUser? _user;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest>> logger, IUser user)
    {
        _logger = logger;
        _user = user;
    }
    
    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest>> logger)
    {
        _logger = logger;
    }

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = _user?.UserId ?? string.Empty;
        var userName = string.Empty;

        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogInformation("Fend Request: {Name} {@Request}",
                requestName, request);
        }
        else
        {
            _logger.LogInformation("Fend Request: {Name} {@UserId} {@UserName} {@Request}",
                requestName, userId, userName, request);
        }
        
        return Task.CompletedTask;
    }
}
