namespace Lensophy.Infrastructure.LargeLanguageModel.OpenAi;

public class OpenAiError
{
    public string Message { get; set; }
    public string Type { get; set; }
    public object Param { get; set; }
    public object Code { get; set; }
}