namespace Lensophy.Domain.Interface;

/// <summary>
/// Interface do modelo de linguagem do Lensophy.
/// </summary>
public interface ILensophyLanguageModel
{
    /// <summary>
    /// Analisa um conteúdo baseado nas diretivas filosóficas de Sócrates, Siddharta Gautama e Confúcio.
    /// </summary>
    /// <param name="contentAnalyse">Ver <see cref="ContentAnalyse"/></param>
    /// <returns>Ver <see cref="ContentAnalysed"/></returns>
    ContentAnalysed Analyse(ContentAnalyse contentAnalyse);
}