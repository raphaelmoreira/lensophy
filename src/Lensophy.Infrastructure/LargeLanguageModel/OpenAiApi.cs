using Lensophy.Domain.Dto.OpenAi;

namespace Lensophy.Infrastructure.LargeLanguageModel;

internal sealed class OpenAiApi : BaseApi, ILensophy
{
    public OpenAiApi(OpenAiConfig config) : base(config.Key) { }

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
    
    /// <inheritdoc/>
    public async Task<bool> IsHarmful(HttpClient httpClient, ContentAnalyse contentAnalyse)
    {
        EnsureContract(httpClient, contentAnalyse);

        var dataRequest = new ModerationRequest(contentAnalyse.Message);
        
        const string model = "moderations";
        var response = await DoRequest<ModerationResponse>(model, httpClient, Json.Serialize(dataRequest));
        
        return response.Flagged;
    }
}