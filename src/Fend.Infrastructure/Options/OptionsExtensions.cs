using Microsoft.Extensions.Configuration;

namespace Fend.Infrastructure.Options;

public static class OptionsExtensions
{
    public static TOptions TryLoad<TOptions>(this IConfiguration configuration) 
        where TOptions : IOptions, new()
    {
        var options = new TOptions();
        options = configuration.GetSection(options.SectionName).Get<TOptions>();
        
        ArgumentNullException.ThrowIfNull(options);
        
        return options;
    }
}