namespace Fend.SharedKernel.Abstractions;

public interface IDateTime
{
    DateTime Now { get; }
    DateTime UtcNow { get; }
}