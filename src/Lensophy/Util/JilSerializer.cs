using Jil;

namespace Lensophy.Util;

internal class JilSerializer : IJsonSerializer
{
    private static readonly Options Options = new(
        dateFormat: DateTimeFormat.ISO8601, 
        includeInherited: true,
        serializationNameFormat: SerializationNameFormat.CamelCase);

    public string Serialize<T>(T? data)
    {
        return JSON.Serialize(data, Options);
    }

    public T? Deserialize<T>(string json)
    {
        json = json.Trim();
        var validJson = !string.IsNullOrWhiteSpace(json) &&
                        json.StartsWith("{") &&
                        json.EndsWith("}");

        return validJson ? JSON.Deserialize<T>(json, Options) : default;
    }
}