using Fend.Infrastructure.Options;

namespace Fend.Infrastructure.Data;

public sealed class DatabaseOptions : IOptions
{
    public string SectionName => "Database";
    public string ConnectionString { get; set; } = string.Empty;
}