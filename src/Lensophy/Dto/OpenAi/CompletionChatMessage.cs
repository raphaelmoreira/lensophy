using System.Runtime.Serialization;

namespace Lensophy.Dto.OpenAi;

/// <summary>
/// Estrutura de requisição do OpenAI.
/// </summary>
public record struct CompletionChatMessage()
{
    /// <summary>
    /// Modelo de linguagem a ser utilizado.
    /// </summary>
    [DataMember(Name="role")]
    public readonly string Role = "user";
    /// <summary>
    /// Temperatura da saída gerada. Temperaturas altas e baixas indicam, respectivamente, aleatoriedade e previsibilidade.
    /// </summary>
    [DataMember(Name="content")]
    public string? Content { get; set; } = string.Empty;
}