using System.Net.Sockets;

namespace Lensophy.Domain.Interface;

/// <summary>
/// An abstraction for the Lensophy Language Model.
/// </summary>
public interface ILensophy
{
    /// <summary>
    /// Analyze content based on the philosophical directives of Socrates, Siddhartha Gautama, and Confucius.
    /// </summary>
    /// <param name="httpClient">The current HTTP instance (see remark).</param>
    /// <param name="contentAnalyse">The content to be analyzed.</param>
    /// <returns>The content already analyzed.</returns>
    /// <remarks><para>Avoid creating a <b>new</b> instance of the <see cref="HttpClient"/> class. Instead, consider
    /// passing the current request's instance or preferably injecting it through the <b>IHttpClientFactory</b>.</para>
    /// <para>Even when wrapped in a <b>using</b> scope, you may not have control over when the <see cref="HttpClient"/> is
    /// disposed, potentially leading to more instances than the Garbage Collector can release, resulting in a
    /// <see cref="SocketException"/> issue.</para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">In case of <c>httpClient</c>, <c>contentAnalyse</c>, or
    /// <c>contentAnalyse.Message</c> are null or empty.</exception>
    Task<ContentAnalysed> Analyse(HttpClient httpClient, ContentAnalyse contentAnalyse);

    /// <summary>
    /// Check if the content is harmful, based on the philosophical guidelines of Socrates, Siddhartha Gautama, and Confucius.
    /// </summary>
    /// <param name="httpClient">The current HTTP instance (see remark).</param>
    /// <param name="contentAnalyse">The content to be analyzed.</param>
    /// <returns>Returns <c>true</c> if the content is considered harmful. Otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// <para>Avoid creating a <b>new</b> instance of the <see cref="HttpClient"/> class. Instead, consider
    /// passing the current request's instance or preferably injecting it through the <b>IHttpClientFactory</b>.</para>
    /// <para>Even when wrapped in a <b>using</b> scope, you may not have control over when the <see cref="HttpClient"/> is
    /// disposed, potentially leading to more instances than the Garbage Collector can release, resulting in a
    /// <see cref="SocketException"/> issue.</para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">In case of <c>httpClient</c>, <c>contentAnalyse</c>, or
    /// <c>contentAnalyse.Message</c> are null or empty.</exception>
    Task<bool> IsHarmful(HttpClient httpClient, ContentAnalyse contentAnalyse);
}