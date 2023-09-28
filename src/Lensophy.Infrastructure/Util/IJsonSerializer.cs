namespace Lensophy.Infrastructure.Util;

internal interface IJsonSerializer
{
    string Serialize<T>(T? data);
    T? Deserialize<T>(string json);
}