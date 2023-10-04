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
    internal static string ToPreparedPrompt(this ContentAnalyse contentAnalyse)
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
    
    internal static string ToModerationRequest(this ContentAnalyse contentAnalyse)
    {
        var content = contentAnalyse.Message.Replace("\"", "'");
        var json = "{" +
                   $"\"input\": \"{content}\"" +
                   "}";

        return json;
    }

    internal static string ToCompletionChatRequest(this string content)
    {
        content = content.Replace("\"", "'");
        var json = "{" +
                   "\"max_tokens\": 256," +
                   "\"temperature\": 1, " +
                   "\"messages\": [" +
                   "{" +
                   $"\"content\": \"{content}\"," +
                   "\"role\": \"user\"" +
                   "}" +
                   "]," +
                   "\"model\": \"gpt-3.5-turbo\"" +
                   "}";

        return json;
    }
}