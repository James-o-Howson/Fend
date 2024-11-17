using System.Text;
using System.Text.Json;
using Fend.Abstractions;

namespace Fend.Infrastructure;

internal sealed class FileWriter : IFileWriter
{
    private readonly IJsonSerializer _jsonSerializer;

    public FileWriter(IJsonSerializer jsonSerializer)
    {
        _jsonSerializer = jsonSerializer;
    }

    public async Task WriteAsync(string path, string content, CancellationToken cancellationToken = default)
    {
        await File.WriteAllTextAsync(path, content, Encoding.UTF8, cancellationToken);
    }
}