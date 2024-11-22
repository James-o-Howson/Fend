namespace Fend.Infrastructure.Options.Configuration;

public sealed class OpenApiDocumentOptions : IOptions
{
    public string SectionName => "OpenApiDocument";
    
    public string Title { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}