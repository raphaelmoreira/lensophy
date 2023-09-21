namespace Lensophy.Domain.Dto.OpenAi;

/// <summary>
/// Estrutura de resultado usado na resposta do modelo <b>moderation</b> do OpenAI.
/// </summary>
/// <remarks><para>A título de implementação futura, o objeto de resultado contém dezenas de informações adicionais.
/// Você pode ver detalhes se sua estrutura na <a href="https://platform.openai.com/docs/guides/moderation/quickstart">
/// documentação oficial.</a></para>
/// <para>Para o propósito do Lensophy, apenas o indicativo negativo é suficiente.</para></remarks>
internal record struct ModerationResult
{
    public bool Flagged { get; set; }
}