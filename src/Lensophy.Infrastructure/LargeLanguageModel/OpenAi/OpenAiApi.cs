using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Lensophy.Infrastructure.LargeLanguageModel.OpenAi;

/// <summary>
/// Api do OpenIA.
/// </summary>
internal class OpenAiApi : BaseApi, ILensophyLanguageModel
{
    private readonly OpenAiConfig _config;
    private const string BaseAddress = "https://api.openai.com/v1/";
    
    /// <summary>
    /// Cria uma nova instância.
    /// </summary>
    /// <param name="config">Configuração do OpenAI.</param>
    public OpenAiApi(OpenAiConfig config)
    {
        _config = config;
    }

    /// <inheritdoc/>
    public async Task<ContentAnalysed> Analyse(HttpClient httpClient, ContentAnalyse contentAnalyse)
    {
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _config.Secret);
        httpClient.BaseAddress = new Uri(BaseAddress);

        var data = new OpenAiRequest(string.Empty);
        var content = new StringContent(Json.Serialize(data), Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync("completions", content).ConfigureAwait(false);
        var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        var openAiResponse = Json.Deserialize<OpenAiResponse>(responseContent);

        return openAiResponse.HasError 
            ? new ContentAnalysed(openAiResponse.FullErrorMessage) 
            : default;
    }
}