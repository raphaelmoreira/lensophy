using System.Runtime.Serialization;

namespace Lensophy.Dto.OpenAi;

internal record struct CompletionChatChoice
{
    public int Index { get; set; }
    public CompletionChatMessage Message { get; set; }
    [DataMember(Name="finish_reason")]
    public string FinishReason { get; set; }
}