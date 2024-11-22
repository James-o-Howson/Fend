using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Fend.SharedKernel;

public sealed class DomainException(string message)
    : Exception(message)
{
    public static void ThrowIfNullOrEmpty([NotNull] string? argument,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (string.IsNullOrEmpty(argument))
        {
            ThrowNullOrEmptyException(argument, paramName);
        }
    }
    
    public static void ThrowIfNullOrWhitespace([NotNull] string? argument,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (string.IsNullOrWhiteSpace(argument))
        {
            ThrowNullOrEmptyException(argument, paramName);
        }
    }
    
    public static void ThrowIfNull([NotNull] object? argument, 
        [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (argument is null)
        {
            Throw(paramName);
        }
    }
    
    [DoesNotReturn]
    private static void ThrowNullOrEmptyException(string? argument, string? paramName)
    {
        ThrowIfNull(argument, paramName);
        throw new DomainException($"{paramName} is empty");
    }
    
    [DoesNotReturn]
    private static void ThrowNullOrWhitespaceException(string? argument, string? paramName)
    {
        ThrowIfNull(argument, paramName);
        throw new DomainException($"{paramName} is empty");
    }
    
    [DoesNotReturn]
    private static void Throw(string? paramName) =>
        throw new DomainException(paramName ?? string.Empty);
}