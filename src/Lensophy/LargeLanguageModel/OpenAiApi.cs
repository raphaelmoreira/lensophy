using System.Threading.Tasks;
using Lensophy.Extension;
using Lensophy.Util;

namespace Lensophy.LargeLanguageModel;

public sealed class OpenAiApi : BaseApi, ILensophy
{
    public OpenAiApi(OpenAiConfig config) : base(config.Key) { }

    /// <inheritdoc/>
    public async Task<ContentAnalysed> Analyse(HttpClient httpClient, ContentAnalyse contentAnalyse)
    {
        EnsureContract(httpClient, contentAnalyse);
        
        const string model = "chat/completions";
        var response = await DoRequest<CompletionChatResponse>(
            model, 
            httpClient,
            contentAnalyse.ToPreparedPrompt().ToCompletionChatRequest());

        var isHarmfull = await IsHarmful(httpClient, contentAnalyse).ConfigureAwait(false);

        return response.ToContentAnalysed(isHarmfull);
    }
    
    /// <inheritdoc/>
    public async Task<bool> IsHarmful(HttpClient httpClient, ContentAnalyse contentAnalyse)
    {
        EnsureContract(httpClient, contentAnalyse);

        var dataRequest = new ModerationRequest(contentAnalyse.Message);
        
        const string model = "moderations";
        var response = await DoRequest<ModerationResponse>(model, httpClient, JilSerializer.Serialize(dataRequest));
        
        return response.Flagged;
    }
}