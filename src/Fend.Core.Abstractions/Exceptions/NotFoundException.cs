namespace Fend.Core.Abstractions.Exceptions;

public sealed class NotFoundException(string message) : Exception(message);