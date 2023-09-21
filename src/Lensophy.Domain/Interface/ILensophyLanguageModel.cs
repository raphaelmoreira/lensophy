namespace Lensophy.Domain.Interface;

/// <summary>
/// Interface do modelo de linguagem do Lensophy.
/// </summary>
public interface ILensophyLanguageModel
{
    /// <summary>
    /// Analisa um conteúdo baseado nas diretivas filosóficas de Sócrates, Siddharta Gautama e Confúcio.
    /// </summary>
    /// <param name="httpClient">O cliente http corrente.</param>
    /// <param name="contentAnalyse">O conteúdo para análise.</param>
    /// <returns>O conteúdo analisado.</returns>
    Task<ContentAnalysed> Analyse(HttpClient httpClient, ContentAnalyse contentAnalyse);

    Task<bool> IsHarmful(HttpClient httpClient, ContentAnalyse contentAnalyse);
}