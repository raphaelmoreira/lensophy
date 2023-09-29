using Lensophy.Dto.OpenAi;
using Lensophy.Interface;
using Lensophy.LargeLanguageModel;

namespace Lensophy;

/// <summary>
/// Provides a class for message analysis, returning a gentle suggestion if the content is offensive in any way.
/// </summary>
/// <remarks>A service account is required to use it.</remarks>
public sealed class Lens
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ILensophy"/> using a <see cref="OpenAiConfig"/>.
    /// </summary>
    /// <param name="config">An OpenAI account <b>key</b> is required to use it.</param>
    /// <returns>Returns the <see cref="ILensophy"/> interface.</returns>
    
    /// <exception cref="ArgumentNullException">In case the <c>httpClient</c> or <c>config</c> are null.</exception>
    public static ILensophy CreateWithOpenAi(OpenAiConfig config)
    {
        ArgumentNullException.ThrowIfNull(config);
        return new OpenAiApi(config);
    }
}