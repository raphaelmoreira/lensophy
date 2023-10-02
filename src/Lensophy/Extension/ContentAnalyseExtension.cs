using Lensophy.Dto;
using Lensophy.Dto.OpenAi;

namespace Lensophy.Extension;

internal static class ContentAnalyseExtension
{
    /// <summary>
    /// The prompt is generated around a series of preparations, ranging from context to philosophical teachings.
    /// </summary>
    /// <param name="contentAnalyse">The content to be analyzed.</param>
    /// <returns>Prepare a properly suitable prompt.</returns>
    public static string ToPreparedPrompt(this ContentAnalyse contentAnalyse)
    {
        var preparedPrompt = $"{string.Format(Resource.Shared.AnalyseTheMessage, contentAnalyse.Message)} " +
                             $"{Resource.Shared.ConsideringFollowingVirtues} " +
                             $"{Resource.Shared.SocratesVirtue} " +
                             $"{Resource.Shared.BuddhaVirtue} " +
                             $"{Resource.Shared.ConfuciusVirtue} " +
                             $"{Resource.Shared.SaitamaVirtue} " +
                             $"{Resource.Shared.RewriteMessage} " +
                             $"{string.Format(Resource.Shared.DoNotExceedMaxTokens, OpenAiConstant.MaxTokens)}";

        if (!string.IsNullOrWhiteSpace(contentAnalyse.Context))
        {
            preparedPrompt = $"{string.Format(Resource.Shared.ConsideringFollowingContext, contentAnalyse.Context)} " +
                             preparedPrompt;
        }
        
        return preparedPrompt;
    }
    
    public static string ToCompletionChatRequest(this string content) => @"{" +
                                                                  $"\"max_tokens\": 256," +
                                                                  $"\"temperature\": 1, " +
                                                                  "\"messages\": [" +
                                                                  "{" +
                                                                  $"\"content\": \"{content}\"," +
                                                                  $"\"role\": \"user\"," +
                                                                  "}" +
                                                                  "]," +
                                                                  $"\"model\": \"gpt-3.5-turbo\"" +
                                                                  "}";
}