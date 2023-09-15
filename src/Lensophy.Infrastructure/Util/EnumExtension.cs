namespace Lensophy.Infrastructure.Util;

/// <summary>
/// Facilitadores para o tipo <see cref="Enum"/>.
/// </summary>
public static class EnumExtension
{
    /// <summary>
    /// Obtém a descrição do enumerador.
    /// </summary>
    /// <param name="value">O enumerador em questão.</param>
    /// <returns><para>A descrição definida através do <see cref="DescriptionAttribute"/>. Caso não tenha sido decorado
    /// com o atributo, será considerado seu nome literal. Se o <b><see cref="value"/></b> não fizer parte dos itens
    /// definidos no enumerador em questão, o mesmo valor é retornado.</para>
    /// <para>Esta extensão não lança exceção por conta de enumeradores inexistentes. Veja o remark para
    /// esclarecimentos.</para></returns>
    /// <exception cref="ArgumentNullException">Se <b>value</b> for nulo.</exception>
    /// <remarks>
    /// <para>Atente-se ao fato de que, durante uma transformação (<i>cast</i>) de enumeradores, por design, o C# não
    /// lança exceção se o valor numérico não fizer parte do enumerador em questão. Ele preserva o mesmo número recebido.</para>
    /// <para>Como esse comportamento pode criar cenários inadequados, procure sempre avaliar se a transformação
    /// ocorreu com sucesso, através do <see cref="Enum.IsDefined"/> em conjunto com
    /// <see cref="Enum.TryParse{TEnum}(string,out TEnum)"/>, antes de fazer uso da extensão.</para>
    /// <para>Esta extensão não tem o intuito de resolver essa particularidade.</para>
    /// </remarks>
    public static string GetDescription(this Enum value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }
        
        var atributoExiste = value
            .GetType()
            .GetField(value.ToString())?
            .GetCustomAttributes(typeof(DescriptionAttribute), false); 

        if (atributoExiste is DescriptionAttribute[] { Length: > 0 } attributes)
        {
            return attributes[0].Description;
        }

        return value.ToString();
    }
}