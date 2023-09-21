using System.Runtime.Serialization;

namespace Lensophy.Domain.Dto.OpenAi;

internal record struct CompletionChatUsage
{
    [DataMember(Name="prompt_tokens")]
    public int PromptTokens { get; set; }
    [DataMember(Name="completion_tokens")]
    public int CompletionTokens { get; set; }
    [DataMember(Name="total_tokens")]
    public int TotalTokens { get; set; }
}