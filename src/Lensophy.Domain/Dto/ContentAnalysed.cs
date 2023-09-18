namespace Lensophy.Domain.Dto;

/// <summary>
/// Estrutura de retorno do conteúdo analisado.
/// </summary>
public readonly record struct ContentAnalysed(string ErrorMessage)
{
    public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);
    public string ErrorMessage { get; } = ErrorMessage;
}