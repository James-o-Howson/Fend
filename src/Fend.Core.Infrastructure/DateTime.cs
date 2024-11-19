using Fend.Core.SharedKernel.Abstractions;

namespace Fend.Core.Infrastructure;

internal sealed class DateTime : IDateTime
{
    public System.DateTime Now => System.DateTime.Now;
}