using System.Collections.Generic;
using System.Net.Http;
using Lensophy.Domain.Dto.OpenAi;

namespace Lensophy.Infrastructure.LargeLanguageModel;

/// <summary>
/// Api do OpenIA.
/// </summary>
internal class OpenAiApi : BaseApi, ILensophyLanguageModel
{
    /// <summary>
    /// Cria uma nova instância.
    /// </summary>
    /// <param name="config">Configuração do OpenAI.</param>
    public OpenAiApi(OpenAiConfig config) : base(config.Secret) { }

    /// <inheritdoc/>
    public async Task<ContentAnalysed> Analyse(HttpClient httpClient, ContentAnalyse contentAnalyse)
    {
        EnsureContract(httpClient, contentAnalyse);

        var dataRequest = new CompletionChatRequest
        {
            Messages = new List<CompletionChatMessage>(1)
            {
                new(){ Content = contentAnalyse.ToPreparedPrompt() }
            }
        };
        
        const string model = "chat/completions";
        var response = await DoRequest<CompletionChatResponse>(model, httpClient, Json.Serialize(dataRequest));

        var isHarmfull = await IsHarmful(httpClient, contentAnalyse).ConfigureAwait(false);

        return response.ToContentAnalysed(isHarmfull);
    }

    public async Task<bool> IsHarmful(HttpClient httpClient, ContentAnalyse contentAnalyse)
    {
        EnsureContract(httpClient, contentAnalyse);

        var dataRequest = new ModerationRequest(contentAnalyse.Message);
        
        const string model = "moderations";
        var response = await DoRequest<ModerationResponse>(model, httpClient, Json.Serialize(dataRequest));
        
        return response.Flagged;
    }
}