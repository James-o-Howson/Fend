using System.Diagnostics;
using Fend.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fend.Application.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly Stopwatch _timer;
    private readonly ILogger<PerformanceBehaviour<TRequest, TResponse>> _logger;
    private readonly IUser? _user;

    public PerformanceBehaviour(
        ILogger<PerformanceBehaviour<TRequest, TResponse>> logger,
        IUser user)
    {
        _timer = new Stopwatch();

        _logger = logger;
        _user = user;
    }
    
    public PerformanceBehaviour(
        ILogger<PerformanceBehaviour<TRequest, TResponse>> logger)
    {
        _timer = new Stopwatch();

        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _timer.Start();
        var response = await next();
        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;
        if (elapsedMilliseconds <= 500) return response;
        
        var requestName = typeof(TRequest).Name;
        var userId = _user?.UserId ?? string.Empty;

        _logger.LogWarning("Fend Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@Request}",
            requestName, elapsedMilliseconds, userId, request);

        return response;
    }
}
