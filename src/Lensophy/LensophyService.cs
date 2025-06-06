using System.Net;
using Lensophy.Dto.OpenAi; // Ensure ModerationResult is accessible, already read Dto.OpenAi.ModerationResponse and Dto.OpenAi.ModerationResult
using System.Globalization;

namespace Lensophy;

/// <summary>
/// Provides a service for message analysis, returning a gentle suggestion if the content is offensive in any way.
/// </summary>
/// <remarks>A service account is required to use it.</remarks>
public sealed class LensophyService
{
    private readonly HttpClient _httpClient;
    private readonly string _openAiApiKey;
    private static readonly int MaxMessageLength = 4000;
    private static readonly int MaxContextLength = 1000;
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
    /// <param name="openAiConfig">The OpenAI API configuration, including the API key.</param>
    /// <exception cref="ArgumentNullException">In case of <c>httpClient</c> or <c>openAiConfig</c> is null or empty.</exception>
    /// <remarks>
    /// <para>Lensophy handle the life cycle of <see cref="HttpClient"/> through the
    /// <a href="https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-7.0#basic-usage">IHttpClientFactory.</a>
    /// </para>
    /// <para><b>Security Note:</b> It is strongly recommended to store API keys securely,
    /// for example, using environment variables, Azure Key Vault, or other secret management services,
    /// especially in production environments. Avoid hardcoding keys directly in the source code.
    /// Refer to <see cref="OpenAiConfig"/> for more details on secure key management.</para>
    /// <para>
    /// Therefore, avoid instantiating <see cref="LensophyService"/> directly, with a <c>new HttpClient()</c>. Consider
    /// register the dependency with <see cref="ServiceCollectionExtension.AddLensophy"/> extension, unless
    /// you know how to use <see cref="IHttpClientFactory"/> properly.
    /// </para>
    /// <para>For curiosity, even when wrapped in a <c>using</c> scope, you may not have control over when the <see cref="HttpClient"/> is
    /// disposed, potentially leading to
    /// <a href="https://learn.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests#issues-with-the-original-httpclient-class-available-in-net">more instances</a>
    /// than the Garbage Collector can release, resulting in a <see cref="SocketException"/> issue.</para>
    /// </remarks>
    public LensophyService(HttpClient httpClient, OpenAiConfig openAiConfig)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(openAiConfig);
        
        _httpClient = httpClient;
        _openAiApiKey = openAiConfig.Key;
        _httpClient.BaseAddress = new Uri(OpenAiApiBaseUrl);
        _httpClient.DefaultRequestHeaders.Add(AcceptHeaderRequest, ApplicationJsonMediaTypeRequest);
    }

    /// <summary>
    /// Analyze content based on the philosophical directives of
    /// <a href="https://raphaelmoreira.github.io/lensophy/articles/philosophy.html#socratess-triple-filter">Socrates</a>,
    /// <a href="https://raphaelmoreira.github.io/lensophy/articles/philosophy.html#buddhas-right-speech">Siddhartha Gautama</a> and
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
        
        var moderationResponse = await IsHarmful(contentAnalyse);
        
        if(moderationResponse is null || moderationResponse.HasError)
        {
            var noChanges = $"{Resource.Shared.NoChanges} {contentAnalyse.Message}";
            var errorMessage = moderationResponse is null
                ? Resource.Shared.ServiceDidNotRespondAsExpected
                : moderationResponse.FullErrorMessage;
            
            // It's important to use contentAnalyse.Message as the first parameter if analysis fails before completion.
            // Or null, but current code uses a "noChanges" version of the message.
            // For consistency, if moderation fails, we return the original message.
            return new ContentAnalysed(noChanges, moderationResponse?.Flagged ?? false, errorMessage);
        }
        
        var data = contentAnalyse.ToPreparedPrompt().ToCompletionChatRequest();
        var content = new StringContent(data, Encoding.UTF8, ApplicationJsonMediaTypeRequest);

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _openAiApiKey);
        var responseMessage = 
            await _httpClient.PostAsync("chat/completions", content, cancellationToken);
        
        if (!responseMessage.IsSuccessStatusCode)
        {
            var errorContent = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
            string specificErrorMessage;
            switch (responseMessage.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    specificErrorMessage = Resource.Shared.OpenAiUnauthorized;
                    break;
                case HttpStatusCode.TooManyRequests:
                    specificErrorMessage = Resource.Shared.OpenAiTooManyRequests;
                    break;
                case HttpStatusCode.InternalServerError:
                    specificErrorMessage = Resource.Shared.OpenAiInternalServerError;
                    break;
                default:
                    specificErrorMessage = string.Format(Resource.Shared.OpenAiGenericError, responseMessage.StatusCode, errorContent);
                    break;
            }
            // Use contentAnalyse.Message as the original message when analysis fails.
            // moderationResponse.Flagged is used as it's the result from the (successful) moderation.
            return new ContentAnalysed(contentAnalyse.Message, moderationResponse.Flagged, specificErrorMessage);
        }

        var responseContent = 
            await responseMessage.Content.ReadAsStringAsync(cancellationToken);

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

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _openAiApiKey);
        var responseMessage = await _httpClient.PostAsync("moderations", content);

        if (!responseMessage.IsSuccessStatusCode)
        {
            var errorContent = await responseMessage.Content.ReadAsStringAsync();
            string specificErrorMessage;
            switch (responseMessage.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    specificErrorMessage = Resource.Shared.ModerationOpenAiUnauthorized;
                    break;
                case HttpStatusCode.TooManyRequests:
                    specificErrorMessage = Resource.Shared.ModerationOpenAiTooManyRequests;
                    break;
                case HttpStatusCode.InternalServerError:
                    specificErrorMessage = Resource.Shared.ModerationOpenAiInternalServerError;
                    break;
                default:
                    specificErrorMessage = string.Format(Resource.Shared.ModerationOpenAiGenericError, responseMessage.StatusCode, errorContent);
                    break;
            }
            // Assuming ModerationResponse has settable HasError and FullErrorMessage, and Results can be set to empty.
            // And that BaseResponse part of ModerationResponse has these error fields.
            return new ModerationResponse
            {
                Results = Array.Empty<ModerationResult>(),
                HasError = true, // This property should exist on BaseResponse or ModerationResponse
                FullErrorMessage = specificErrorMessage // This property should exist on BaseResponse or ModerationResponse
            };
        }

        var responseContent = await responseMessage.Content.ReadAsStringAsync();
        var response = JsonSerializer.Deserialize<ModerationResponse>(responseContent, _serializerOptions);

        return response;
    }

    /// <summary>
    /// Check if the terms of use have been properly met.
    /// </summary>
    /// <param name="contentAnalyse">The content to be analyzed.</param>
    /// <exception cref="ArgumentNullException">Thrown if <c>contentAnalyse.Message</c> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if <c>contentAnalyse.Message</c> is empty or consists only of white-space characters.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <c>contentAnalyse.Message</c> exceeds 4000 characters or if <c>contentAnalyse.Context</c> (when provided) exceeds 1000 characters.</exception>
    private static void EnsureContract(ContentAnalyse contentAnalyse)
    {
        if (string.IsNullOrWhiteSpace(contentAnalyse.Message))
        {
            if (contentAnalyse.Message == null)
            {
                throw new ArgumentNullException(nameof(contentAnalyse.Message), Resource.Shared.MessageCannotBeNull);
            }
            else // It's empty or whitespace
            {
                throw new ArgumentException(Resource.Shared.MessageCannotBeEmpty, nameof(contentAnalyse.Message));
            }
        }

        if (contentAnalyse.Message.Length > MaxMessageLength)
        {
            throw new ArgumentOutOfRangeException(nameof(contentAnalyse.Message),
                string.Format(CultureInfo.CurrentCulture, Resource.Shared.MessageTooLong, MaxMessageLength));
        }

        if (contentAnalyse.Context != null && contentAnalyse.Context.Length > MaxContextLength)
        {
            throw new ArgumentOutOfRangeException(nameof(contentAnalyse.Context),
                string.Format(CultureInfo.CurrentCulture, Resource.Shared.ContextTooLong, MaxContextLength));
        }
    }
}