using System.Linq;

namespace Lensophy.Dto.OpenAi;

/// <summary>
/// Estrutura de resposta do OpenAI.
/// </summary>
internal record CompletionChatResponse : BaseResponse
{
    public string Id { get; set; }
    public string Object { get; set; }
    public int Created { get; set; }
    public string Model { get; set; }
    public CompletionChatChoice[] Choices { get; set; }
    public CompletionChatUsage Usage { get; set; }
    public string? SuggestedMessage => Choices?.FirstOrDefault().Message.Content;
}