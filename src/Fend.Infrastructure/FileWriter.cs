using System.Text;
using Fend.Abstractions;

namespace Fend.Infrastructure;

internal sealed class FileWriter : IFileWriter
{
    public async Task WriteAsync(string path, string content, CancellationToken cancellationToken = default)
    {
        await File.WriteAllTextAsync(path, content, Encoding.UTF8, cancellationToken);
    }
}