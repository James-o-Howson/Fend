using System.Text.Json;

namespace Fend.Core.Abstractions.Interfaces;

public interface IJsonSerializer
{
    string Serialize<TValue>(TValue value, JsonSerializerOptions? options = null);
}