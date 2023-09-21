namespace Lensophy.Domain.Dto.OpenAi;

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
    /// Most capable GPT-3.5 model and optimized for chat at 1/10th the cost of text-davinci-003.
    /// </summary>
    [Description("gpt-3.5-turbo")]
    Gpt35Turbo = 1
}