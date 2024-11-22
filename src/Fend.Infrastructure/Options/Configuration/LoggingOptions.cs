namespace Fend.Infrastructure.Options.Configuration;

public sealed class LoggingOptions : IOptions
{
    public string SectionName => "Logging";

    public string IdentifierName { get; set; } = string.Empty;
    public bool WriteToFile { get; set; }
    public bool WriteToConsole { get; set; }
}