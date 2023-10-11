namespace Lensophy;

/// <summary>
/// Provides a service for message analysis, returning a gentle suggestion if the content is offensive in any way.
/// </summary>
/// <remarks>A service account is required to use it.</remarks>
public sealed class LensophyService
{
    private readonly HttpClient _httpClient;
    private const string ApplicationJsonMediaTypeRequest = "application/json";
    private const string AcceptHeaderRequest = "Accept";
    private const string OpenAiApiBaseUrl = "https://api.openai.com/v1/";
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="LensophyService"/>.
    /// </summary>
    /// <param name="httpClient">The current HTTP instance.</param>
    /// <exception cref="ArgumentNullException">In case of <c>httpClient</c> is null or empty.</exception>
    /// <remarks><para>Lensophy handle the life cycle of <see cref="HttpClient"/> through the
    /// <a href="https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-7.0#basic-usage">IHttpClientFactory.</a>
    /// </para>
    /// <para>
    /// Therefore, avoid instantiating <see cref="LensophyService"/> directly, with a <c>new HttpClient()</c>. Consider
    /// register the dependency with <see cref="ServiceCollectionExtension.AddLensophy"/> extension, unless
    /// you know how to use <see cref="IHttpClientFactory"/> properly.
    /// </para>
    /// <para>For curiosity, even when wrapped in a <b>using</b> scope, you may not have control over when the <see cref="HttpClient"/> is
    /// disposed, potentially leading to more instances than the Garbage Collector can release, resulting in a
    /// <see cref="SocketException"/> issue.</para>
    /// </remarks>
    public LensophyService(HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(OpenAiApiBaseUrl);
        _httpClient.DefaultRequestHeaders.Add(AcceptHeaderRequest, ApplicationJsonMediaTypeRequest);
    }

    /// <summary>
    /// Analyze content based on the philosophical directives of
    /// <a href="https://raphaelmoreira.github.io/lensophy/articles/philosophy.html#the-triple-filter-of-socrates">Socrates</a>,
    /// <a href="https://raphaelmoreira.github.io/lensophy/articles/philosophy.html#the-right-speech-of-buddha">Siddhartha Gautama</a> and
    /// <a href="https://raphaelmoreira.github.io/lensophy/articles/philosophy.html#confuciuss-trusted-disciple">Confucius.</a>
    /// </summary>
    /// <param name="contentAnalyse">The content to be analyzed.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive
    /// notice of cancellation.</param>
    /// <returns>The content already analyzed.</returns>
    /// <exception cref="ArgumentNullException">In case of <c>contentAnalyse</c> or <c>contentAnalyse.Message</c>
    /// are null or empty.</exception>
    public async Task<ContentAnalysed> AnalyseAsync(ContentAnalyse contentAnalyse, CancellationToken cancellationToken)
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
        var content = new StringContent(data, Encoding.UTF8, ApplicationJsonMediaTypeRequest);
        var responseMessage = 
            await _httpClient.PostAsync("chat/completions", content, cancellationToken).ConfigureAwait(false);
        
        var responseContent = 
            await responseMessage.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

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
        var content = new StringContent(data, Encoding.UTF8, ApplicationJsonMediaTypeRequest);
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