using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;

namespace Lensophy.Extension;

/// <summary>
/// Extension methods to configure an <see cref="IServiceCollection"/> for <see cref="LensophyService"/>.
/// </summary>
public static class ServiceCollectionExtension
{
    /// <summary>
    /// Adds the <see cref="LensophyService"/> to the <see cref="IHttpClientFactory"/> and configures a binding for
    /// <see cref="HttpClient"/>.
    /// </summary>
    /// <param name="serviceCollection">The <see cref="IServiceCollection"/></param>
    /// <param name="secret">The OpenAI secret.</param>
    /// <exception cref="ArgumentNullException">If <c>serviceCollection</c> or <c>secret</c> are null or empty.</exception>
    /// <remarks><para>Lensophy handle the life cycle of <see cref="HttpClient"/>, without creating a <b>new</b> instance
    /// of the <see cref="HttpClient"/> class, through the
    /// <a href="https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-7.0#basic-usage">IHttpClientFactory.</a>
    /// </para>
    /// <para>For curiosity, even when wrapped in a <b>using</b> scope, you may not have control over when the <see cref="HttpClient"/> is
    /// disposed, potentially leading to more instances than the Garbage Collector can release, resulting in a
    /// <see cref="SocketException"/> issue.</para>
    /// </remarks>
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