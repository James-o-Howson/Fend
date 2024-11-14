using Fend.SharedKernel.Abstractions;

namespace Fend.Infrastructure;

internal sealed class DateTime : IDateTime
{
    public System.DateTime Now => System.DateTime.Now;
}