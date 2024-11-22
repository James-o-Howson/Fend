using Fend.Infrastructure.Options;

namespace Fend.Identity;

internal sealed class IdentityOptions : IOptions
{
    public string SectionName => "Identity";

    public string Url { get; set; } = string.Empty;
}