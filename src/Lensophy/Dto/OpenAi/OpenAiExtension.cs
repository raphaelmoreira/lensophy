namespace Lensophy.Dto.OpenAi;

internal static class OpenAiExtension
{
    public static ContentAnalysed ToContentAnalysed(this CompletionChatResponse? completionChatResponse, bool isHarmfull)
    {
        var suggestedMessage = Resource.Shared.EmptySeeFullErrorMessage;
        if (completionChatResponse is not null && !string.IsNullOrWhiteSpace(completionChatResponse.SuggestedMessage))
        {
            suggestedMessage = completionChatResponse.SuggestedMessage;
        }

        var fullErrorMessage = string.Empty;
        if (completionChatResponse is not null && !string.IsNullOrWhiteSpace(completionChatResponse.FullErrorMessage))
        {
            fullErrorMessage = completionChatResponse.FullErrorMessage;
        }
        
        return new ContentAnalysed(suggestedMessage, isHarmfull, fullErrorMessage);
    }
}