using Fend.SharedKernel.Abstractions;

namespace Fend.Infrastructure.Services;

internal sealed class DateTime : IDateTime
{
    public System.DateTime Now => System.DateTime.Now;
    public System.DateTime UtcNow => System.DateTime.UtcNow;
}