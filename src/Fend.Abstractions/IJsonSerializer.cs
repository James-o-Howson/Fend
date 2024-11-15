using System.Text.Json;

namespace Fend.Abstractions;

public interface IJsonSerializer
{
    string Serialize<TValue>(TValue value, JsonSerializerOptions? options = null);
}