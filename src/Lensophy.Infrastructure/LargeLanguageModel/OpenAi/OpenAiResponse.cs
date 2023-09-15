namespace Lensophy.Infrastructure.LargeLanguageModel.OpenAi;

/// <summary>
/// Estrutura de resposta do OpenAI.
/// </summary>
internal record struct OpenAiResponse
{
    public string Id { get; set; }
    public string Object { get; set; }
    public int Created { get; set; }
    public string Model { get; set; }
    public OpenAiChoice[] Choices { get; set; }
    public OpenAiUsage OpenAiUsage { get; set; }
    public OpenAiError? Error { get; set; }
}