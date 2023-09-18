namespace Lensophy;

/// <summary>
/// Cada instância configura e inicia a biblioteca para avaliação e sugestão de comentários.
/// </summary>
public class Lensophy
{
    /// <summary>
    /// Cria uma nova instância da biblioteca.
    /// </summary>
    /// <param name="httpClient">O cliente http corrente.</param>
    /// <returns>Retorna a interface de uso.</returns>
    /// <remarks><para>Se estiver numa aplicação web, evite criar <see cref="HttpClient"/> usando o keyword <b>new</b>.
    /// Sempre repasse a instância corrente ou gerida através do <see cref="IHttpClientFactory"/>.</para>
    /// <para>Mesmo que esteja envolto do escopo <b>using</b>, você não terá controle de quando o <see cref="HttpClient"/>
    /// será descartado, podendo gerar mais instâncias do que o Garbage Collector é capaz de liberar,
    /// ocasionando um <see cref="SocketsHttpHandler"/>.</para>
    /// </remarks>
    public ILensophyLanguageModel Create(HttpClient httpClient)
    {
        throw new NotImplementedException();
    }
}