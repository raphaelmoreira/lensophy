namespace Lensophy.Infrastructure.LargeLanguageModel.OpenAi;

internal class OpenAiChoice
{
    public string Text { get; set; }
    public int Index { get; set; }
    public object Logprobs { get; set; }
    public string FinishReason { get; set; }
}