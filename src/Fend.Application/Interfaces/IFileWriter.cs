namespace Fend.Application.Interfaces;

public interface IFileWriter
{
    Task WriteAsync(string path, string content, CancellationToken cancellationToken = default);
}