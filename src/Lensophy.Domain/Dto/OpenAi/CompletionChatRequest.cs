using System.Collections.Generic;
using System.Runtime.Serialization;
using Lensophy.Domain.Extension;

namespace Lensophy.Domain.Dto.OpenAi;

/// <summary>
/// Estrutura de requisição do OpenAI.
/// </summary>
internal class CompletionChatRequest
{
    /// <summary>
    /// Modelo de linguagem a ser utilizado.
    /// </summary>
    [DataMember(Name="model")]
    public readonly string Model = OpenAiModel.Gpt35Turbo.GetDescription();
    /// <summary>
    /// Temperatura da saída gerada. Temperaturas altas e baixas indicam, respectivamente, aleatoriedade e previsibilidade.
    /// </summary>
    [DataMember(Name="temperature")]
    public readonly float Temperature = OpenAiConstant.Temperature;
    /// <summary>
    /// Limita a quantidade de saída gerada pelo modelo.
    /// </summary>
    [DataMember(Name="max_tokens")]
    public readonly int MaxTokens = OpenAiConstant.MaxTokens;
    /// <summary>
    /// A mensagem que deseja enviar ao modelo de linguagem.
    /// </summary>
    [DataMember(Name="messages")]
    public IEnumerable<CompletionChatMessage>? Messages { get; set; }
}