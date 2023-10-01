namespace Lensophy.Interface;

public interface IJsonSerializer
{
    string Serialize<T>(T? data);
    T? Deserialize<T>(string json);
}