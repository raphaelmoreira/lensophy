using System.Threading.Tasks;
using Lensophy.Extension;
using Microsoft.Net.Http.Headers;
using System.Text;
using Lensophy.Util;

namespace Lensophy;

/// <summary>
/// Provides a service for message analysis, returning a gentle suggestion if the content is offensive in any way.
/// </summary>
/// <remarks>A service account is required to use it.</remarks>
public class LensophyService
{
    private readonly HttpClient _httpClient;
    private const string MediaType = "application/json";

    /// <summary>
    /// Initializes a new instance of the <see cref="LensophyService"/>.
    /// </summary>
    /// <param name="httpClient">The current HTTP instance (see remark).</param>
    /// <remarks><para>Avoid creating a <b>new</b> instance of the <see cref="HttpClient"/> class. Instead, consider
    /// passing the current request's instance or preferably injecting it through the
    /// <a href="https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-7.0#basic-usage">IHttpClientFactory.</a>
    /// </para>
    /// <para>Even when wrapped in a <b>using</b> scope, you may not have control over when the <see cref="HttpClient"/> is
    /// disposed, potentially leading to more instances than the Garbage Collector can release, resulting in a
    /// <see cref="SocketException"/> issue.</para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">In case of <c>httpClient</c> is null or empty.</exception>
    public LensophyService(HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.openai.com/v1/");
        _httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, MediaType);
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
        var content = new StringContent(data, Encoding.UTF8, MediaType);
        var responseMessage = await _httpClient.PostAsync("chat/completions", content).ConfigureAwait(false);
        var responseContent = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

        var response = JilSerializer.Deserialize<CompletionChatResponse>(responseContent);

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
        var content = new StringContent(data, Encoding.UTF8, MediaType);
        var responseMessage = await _httpClient.PostAsync("moderations", content).ConfigureAwait(false);
        var responseContent = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

        var response = JilSerializer.Deserialize<ModerationResponse>(responseContent);

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