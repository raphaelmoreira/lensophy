namespace Lensophy.Domain.Dto;

/// <summary>
/// Estrutura de retorno do conteúdo analisado.
/// </summary>
public readonly record struct ContentAnalysed(string SuggestedMessage, bool IsHarmful, string ErrorMessage)
{
    public string SuggestedMessage { get; } = SuggestedMessage;
    public bool IsHarmful { get; } = IsHarmful;
    public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);
    public string ErrorMessage { get; } = ErrorMessage;
}