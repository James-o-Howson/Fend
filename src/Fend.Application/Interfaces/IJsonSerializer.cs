using System.Text.Json;

namespace Fend.Application.Interfaces;

public interface IJsonSerializer
{
    string Serialize<TValue>(TValue value, JsonSerializerOptions? options = null);
}