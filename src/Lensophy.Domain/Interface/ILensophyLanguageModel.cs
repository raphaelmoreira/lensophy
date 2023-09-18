using System.Net.Http;
using System.Threading.Tasks;

namespace Lensophy.Domain.Interface;

/// <summary>
/// Interface do modelo de linguagem do Lensophy.
/// </summary>
public interface ILensophyLanguageModel
{
    /// <summary>
    /// Verifica se a api está online e funcional antes de uma requisição.
    /// </summary>
    /// <param name="httpClient">O cliente http corrente.</param>
    /// <returns>Retorna <b>True</b> se estiver online, do contrário, <b>False</b></returns>
    /// <remarks>Este método não é executado a cada chamada de <see cref="Analyse"/>, pois isso resultaria num
    /// excesso de chamada. Ele será futuramente usado no tramento de Retry. Use-o de forma esporádica.</remarks>
    Task<bool> IsOnline(HttpClient httpClient);
    
    /// <summary>
    /// Analisa um conteúdo baseado nas diretivas filosóficas de Sócrates, Siddharta Gautama e Confúcio.
    /// </summary>
    /// <param name="httpClient">O cliente http corrente.</param>
    /// <param name="contentAnalyse">O conteúdo para análise.</param>
    /// <returns>O conteúdo analisado.</returns>
    ContentAnalysed Analyse(HttpClient httpClient, ContentAnalyse contentAnalyse);
}