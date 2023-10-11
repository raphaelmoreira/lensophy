namespace Lensophy.Extension;

/// <summary>
/// Extension methods to configure an <see cref="IServiceCollection"/> for <see cref="LensophyService"/>.
/// </summary>
public static class ServiceCollectionExtension
{
    /// <summary>
    /// Adds the <see cref="LensophyService"/> and configure the binding for <see cref="HttpClient"/> through the <see cref="IHttpClientFactory"/>.
    /// </summary>
    /// <param name="serviceCollection">The <see cref="IServiceCollection"/></param>
    /// <param name="secret">The OpenAI secret.</param>
    /// <exception cref="ArgumentNullException">If <c>serviceCollection</c> or <c>secret</c> are null or empty.</exception>
    public static void AddLensophy(this IServiceCollection serviceCollection, string secret)
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);
        ArgumentNullException.ThrowIfNull(secret);
        
        serviceCollection.AddHttpClient<LensophyService>(httpClient =>
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", secret);
        });
    }
}