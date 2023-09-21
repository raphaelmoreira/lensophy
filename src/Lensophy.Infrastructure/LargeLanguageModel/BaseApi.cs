using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;

namespace Lensophy.Infrastructure.LargeLanguageModel;

internal abstract class BaseApi
{
    private readonly string _secret;
    private const string MediaType = "application/json";
    private const string BaseAddress = "https://api.openai.com/v1/";
    protected readonly IJsonSerializer Json = new JilSerializer();

    protected BaseApi(string secret)
    {
        _secret = secret;
    }
    
    protected async Task<T?> DoRequest<T>(string model, HttpClient httpClient, string data)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, new Uri($"{BaseAddress}{model}"));
        request.Headers.Accept.Clear();
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaType));
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _secret);
        request.Content = new StringContent(data, Encoding.UTF8, MediaType);
        
        var responseMessage = await httpClient.SendAsync(request, CancellationToken.None).ConfigureAwait(false);
        var responseContent = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
        
        return Json.Deserialize<T>(responseContent);
    }
    
    protected static void EnsureContract(HttpClient httpClient, ContentAnalyse contentAnalyse)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(contentAnalyse);
        ArgumentNullException.ThrowIfNull(contentAnalyse.Message);
    }
}