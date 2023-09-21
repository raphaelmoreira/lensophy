namespace Lensophy.Domain.Dto;

/// <summary>
/// Estrutura de envio do conteúdo a ser analisado.
/// </summary>
public readonly record struct ContentAnalyse(string? Context, string Message);