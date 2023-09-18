namespace Lensophy.Infrastructure.LargeLanguageModel.OpenAi;

internal class OpenAiConfig
{
    public string Secret { get; }
    
    public OpenAiConfig(string? secret)
    {
        ArgumentNullException.ThrowIfNull(secret);
        
        Secret = secret;
    }
}