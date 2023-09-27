namespace Lensophy;

/// <summary>
/// Provides a class for message analysis, returning a gentle suggestion if the content is offensive in any way.
/// </summary>
/// <remarks>A service account is required to use it.</remarks>
public class Lensophy
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Lensophy"/> class using a <see cref="OpenAiConfig"/>.
    /// </summary>
    /// <param name="httpClient">The current HTTP instance (see remark).</param>
    /// <param name="config">An OpenAI account <b>secret</b> is required to use it.</param>
    /// <returns>Returns the <see cref="ILensophyLanguageModel"/> interface.</returns>
    /// <remarks><para>Avoid creating a <b>new</b> instance of the <see cref="HttpClient"/> class. Instead, consider
    /// passing the current request's instance or preferably injecting it through the <see cref="IHttpClientFactory"/>.</para>
    /// <para>Even when wrapped in a <b>using</b> scope, you may not have control over when the <see cref="HttpClient"/> is
    /// disposed, potentially leading to more instances than the Garbage Collector can release, resulting in a
    /// <see cref="SocketException"/> issue.</para>
    /// </remarks>
    public static ILensophyLanguageModel CreateWithOpenAi(HttpClient httpClient, OpenAiConfig config)
    {
        return new OpenAiApi(config);
    }
}