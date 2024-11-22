using System.Text.Json;
using Fend.Abstractions.Interfaces;

namespace Fend.Infrastructure.Services;

internal sealed class JsonSerializer : IJsonSerializer
{
    public string Serialize<TValue>(TValue value, JsonSerializerOptions? options = null) =>
        System.Text.Json.JsonSerializer.Serialize(value, options);
}