namespace Lensophy.Domain.Dto.OpenAi;

internal static class OpenAiExtension
{
    public static ContentAnalysed ToContentAnalysed(this CompletionChatResponse completionChatResponse, bool isHarmfull)
    {
        return new ContentAnalysed(completionChatResponse.SuggestedMessage, isHarmfull, completionChatResponse.FullErrorMessage);
    }
}