namespace Lensophy.Domain.Dto.OpenAi;

public class OpenAiConfig
{
    public string Secret { get; }
    
    public OpenAiConfig(string? secret)
    {
        ArgumentNullException.ThrowIfNull(secret);

        Secret = secret;
    }
}