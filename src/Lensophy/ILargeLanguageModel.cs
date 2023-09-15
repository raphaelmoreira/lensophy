namespace Lensophy;

/// <summary>
/// Interface do modelo de linguagem.
/// </summary>
internal interface ILargeLanguageModel
{
    /// <summary>
    /// Analisa um conteúdo baseado nas diretivas filosóficas de Sócrates, Siddharta Gautama e Confúcio.
    /// </summary>
    /// <param name="messageAnalyse">Ver <see cref="MessageAnalyse"/></param>
    /// <returns>Ver <see cref="MessageAnalysed"/></returns>
    MessageAnalysed Analyse(MessageAnalyse messageAnalyse);
}