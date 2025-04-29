using System.Linq;

namespace Lensophy.Dto.OpenAi;

/// <summary>
/// Estrutura de resposta do OpenAI.
/// </summary>
internal record CompletionChatResponse : BaseResponse
{
    public string Id { get; set; } = string.Empty;
    public string Object { get; set; } = string.Empty;
    public int Created { get; set; }
    public string Model { get; set; } = string.Empty;
    public CompletionChatChoice[] Choices { get; set; } = [];
    public CompletionChatUsage Usage { get; set; }
    public string? SuggestedMessage => Choices?.FirstOrDefault().Message.Content;
}