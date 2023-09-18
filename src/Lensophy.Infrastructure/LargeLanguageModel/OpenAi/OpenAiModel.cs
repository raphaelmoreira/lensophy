namespace Lensophy.Infrastructure.LargeLanguageModel.OpenAi;

/// <summary>
/// The OpenAI API is powered by a diverse set of models with different capabilities.
/// </summary>
internal enum OpenAiModel
{
    /// <summary>
    /// None.
    /// </summary>
    [Description("None")]
    None = 0,
    /// <summary>
    /// Can do any language task with better quality, longer output, and consistent instruction-following than the curie,
    /// babbage, or ada models. Also supports some additional features such as
    /// <a href="https://platform.openai.com/docs/guides/gpt/inserting-text">inserting text.</a>
    /// </summary>
    /// <remarks>Consider this as Legacy and will be deprecated on 2024-01-04. Will be replaced by gpt-3.5-turbo-instruct.</remarks>
    [Description("text-davinci-003")]
    TextDaVinci003 = 1,
    /// <summary>
    /// Most capable GPT-3.5 model and optimized for chat at 1/10th the cost of text-davinci-003.
    /// </summary>
    [Description("gpt-3.5-turbo")]
    Gpt35Turbo = 2,
    /// <summary>
    /// Same capabilities as the standard gpt-3.5-turbo model but with 4 times the context.
    /// </summary>
    [Description("gpt-3.5-turbo-16k")]
    Gpt35Turbo16K = 3
}