// ReSharper disable once CheckNamespace

namespace Cocona.Application;

internal static class AppContextAccessorExtensions
{
    public static CancellationToken CancellationToken(this ICoconaAppContextAccessor accessor)
    {
        return accessor.GetRequiredContext().CancellationToken;
    }

    public static CoconaAppContext GetRequiredContext(this ICoconaAppContextAccessor accessor)
    {
        ArgumentNullException.ThrowIfNull(accessor.Current);

        return accessor.Current;
    }
}