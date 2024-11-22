using System.Text;
using Fend.Abstractions.Interfaces;

namespace Fend.Infrastructure.Services;

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