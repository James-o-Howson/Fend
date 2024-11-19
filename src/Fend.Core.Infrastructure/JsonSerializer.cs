using System.Text.Json;
using Fend.Core.Abstractions.Interfaces;

namespace Fend.Core.Infrastructure;

internal sealed class JsonSerializer : IJsonSerializer
{
    public string Serialize<TValue>(TValue value, JsonSerializerOptions? options = null) =>
        System.Text.Json.JsonSerializer.Serialize(value, options);
}