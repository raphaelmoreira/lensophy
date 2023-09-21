using Lensophy.Domain.Dto.OpenAi;

namespace Lensophy.Domain.Extension;

internal static class ContentAnalyseExtension
{
    /// <summary>
    /// O prompt é gerado em torno de uma série de preparos, que vão do contexto à ensinamentos filosóficos.
    /// </summary>
    /// <param name="contentAnalyse">Conteúdo a ser analisado.</param>
    /// <returns>Transforma o conteúdo a ser analisado num prompt adequado.</returns>
    public static string ToPreparedPrompt(this ContentAnalyse contentAnalyse)
    {
        var preparedPrompt = $"{string.Format(Resource.Message.AnalyseTheMessage, contentAnalyse.Message)} " +
                             $"{Resource.Message.ConsideringFollowingVirtues} " +
                             $"{Resource.Message.SocratesVirtue} " +
                             $"{Resource.Message.BuddhaVirtue} " +
                             $"{Resource.Message.ConfuciusVirtue} " +
                             $"{Resource.Message.SaitamaVirtue} " +
                             $"{Resource.Message.RewriteMessage} " +
                             $"{string.Format(Resource.Message.DoNotExceedMaxTokens, OpenAiConstant.MaxTokens)}";

        if (!string.IsNullOrWhiteSpace(contentAnalyse.Context))
        {
            preparedPrompt = $"{string.Format(Resource.Message.ConsideringFollowingContext, contentAnalyse.Context)} " +
                             preparedPrompt;
        }
        
        return preparedPrompt;
    }
}