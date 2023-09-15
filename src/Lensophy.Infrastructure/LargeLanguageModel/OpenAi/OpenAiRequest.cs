namespace Lensophy.Infrastructure.LargeLanguageModel.OpenAi;

/// <summary>
/// Estrutura de requisição do OpenAI.
/// </summary>
internal record struct OpenAiRequest
{
    /// <summary>
    /// Modelo de linguagem a ser utilizado.
    /// </summary>
    public string Model = OpenAiModel.TextDaVinci003.GetDescription();
    /// <summary>
    /// Temperatura da saída gerada. Temperaturas altas e baixas indicam, respectivamente, aleatoriedade e previsibilidade.
    /// </summary>
    public float Temperature = 0f;
    /// <summary>
    /// Limita a quantidade de saída gerada pelo modelo.
    /// </summary>
    public int MaxTokens = 10;
    /// <summary>
    /// A mensagem que deseja enviar ao modelo de linguagem.
    /// </summary>
    public string Prompt { get; } = string.Empty;
    
    /// <summary>
    /// Constrói uma estrutura de requisição a partir de um prompt.
    /// </summary>
    /// <param name="prompt">Prompt.</param>
    public OpenAiRequest(string prompt)
    {
        Prompt = prompt;
    }
}