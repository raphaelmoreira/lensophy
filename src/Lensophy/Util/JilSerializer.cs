using Jil;

namespace Lensophy.Util;

internal static class JilSerializer
{
    private static readonly Options Options = new(
        dateFormat: DateTimeFormat.ISO8601, 
        includeInherited: true,
        serializationNameFormat: SerializationNameFormat.CamelCase);

    public static string Serialize<T>(T? data)
    {
        return JSON.Serialize(data, Options);
    }

    public static T? Deserialize<T>(string json)
    {
        json = json.Trim();
        var validJson = !string.IsNullOrWhiteSpace(json) &&
                        json.StartsWith("{") &&
                        json.EndsWith("}");

        return validJson ? JSON.Deserialize<T>(json, Options) : default;
    }
}