namespace Lensophy.Dto.OpenAi;

/// <summary>
/// It provides an OpenAI Api configuration class as expected by Lensophy.
/// </summary>
internal sealed class OpenAiConfig
{
    internal readonly string Key;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="OpenAiConfig"/>.
    /// </summary>
    /// <param name="key">Your <a href="https://platform.openai.com/">OpenApi</a> key.</param>
    /// <remarks><para>To create a new key, access the <a href="https://platform.openai.com/account/api-keys">Api Keys</a>
    /// resource (an account and login are required).</para>
    /// <para><b>Important:</b> Lensophy never stores or shares the <see cref="Key"/> outside of its context. Its use
    /// is solely for the purpose of making Api calls.</para>
    /// <para>Do not share your key with others, or expose it in the browser or other client-side code.</para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">In case the sent key is null or empty.</exception>
    public OpenAiConfig(string key)
    {
        ArgumentNullException.ThrowIfNull(key);
        Key = key;
    }
}