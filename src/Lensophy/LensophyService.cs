using System.Threading.Tasks;
using Lensophy.Extension;
using System.Text;

namespace Lensophy;

/// <summary>
/// Provides a service for message analysis, returning a gentle suggestion if the content is offensive in any way.
/// </summary>
/// <remarks>A service account is required to use it.</remarks>
public class LensophyService
{
    private readonly HttpClient _httpClient;
    private const string MediaTypeRequest = "application/json";
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="LensophyService"/>.
    /// </summary>
    /// <param name="httpClient">The current HTTP instance (see remark).</param>
    /// <exception cref="ArgumentNullException">In case of <c>httpClient</c> is null or empty.</exception>
    public LensophyService(HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.openai.com/v1/");
        _httpClient.DefaultRequestHeaders.Add("Accept", MediaTypeRequest);
    }

    /// <summary>
    /// Analyze content based on the philosophical directives of Socrates, Siddhartha Gautama, and Confucius.
    /// </summary>
    /// <param name="contentAnalyse">The content to be analyzed.</param>
    /// <returns>The content already analyzed.</returns>
    /// <exception cref="ArgumentNullException">In case of <c>contentAnalyse</c> or <c>contentAnalyse.Message</c>
    /// are null or empty.</exception>
    public async Task<ContentAnalysed> AnalyseAsync(ContentAnalyse contentAnalyse)
    {
        EnsureContract(contentAnalyse);
        
        var moderationResponse = await IsHarmful(contentAnalyse).ConfigureAwait(false);
        
        if(moderationResponse is null || moderationResponse.HasError)
        {
            var noChanges = $"{Resource.Shared.NoChanges} {contentAnalyse.Message}";
            var errorMessage = moderationResponse is null
                ? Resource.Shared.ServiceDidNotRespondAsExpected
                : moderationResponse.FullErrorMessage;
            
            return new ContentAnalysed(noChanges, false, errorMessage);
        }
        
        var data = contentAnalyse.ToPreparedPrompt().ToCompletionChatRequest();
        var content = new StringContent(data, Encoding.UTF8, MediaTypeRequest);
        var responseMessage = await _httpClient.PostAsync("chat/completions", content).ConfigureAwait(false);
        var responseContent = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

        var response = JsonSerializer.Deserialize<CompletionChatResponse>(responseContent, _serializerOptions);

        return response.ToContentAnalysed(moderationResponse.Flagged);
    }
    
    /// <summary>
    /// Check if the content is harmful, based on the philosophical guidelines of Socrates, Siddhartha Gautama, and Confucius.
    /// </summary>
    /// <param name="contentAnalyse">The content to be analyzed.</param>
    /// <returns>Returns <c>true</c> if the content is considered harmful. Otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">In case of <c>contentAnalyse</c> or <c>contentAnalyse.Message</c> are null or empty.</exception>
    private async Task<ModerationResponse?> IsHarmful(ContentAnalyse contentAnalyse)
    {
        var data = contentAnalyse.ToModerationRequest();
        var content = new StringContent(data, Encoding.UTF8, MediaTypeRequest);
        var responseMessage = await _httpClient.PostAsync("moderations", content).ConfigureAwait(false);
        var responseContent = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

        var response = JsonSerializer.Deserialize<ModerationResponse>(responseContent, _serializerOptions);

        return response;
    }

    /// <summary>
    /// Check if the terms of use have been properly met.
    /// </summary>
    /// <param name="contentAnalyse">The content to be analyzed.</param>
    /// <exception cref="ArgumentNullException">In case of <c>contentAnalyse</c> or <c>contentAnalyse.Message</c> are null or empty.</exception>
    private static void EnsureContract(ContentAnalyse contentAnalyse)
    {
        ArgumentNullException.ThrowIfNull(contentAnalyse);
        ArgumentNullException.ThrowIfNull(contentAnalyse.Message);
    }
}