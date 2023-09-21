using System.Collections.Generic;
using System.Linq;

namespace Lensophy.Domain.Dto.OpenAi;

/// <summary>
/// Estrutura de resposta do modelo <b>moderation</b> do OpenAI.
/// </summary>
internal record struct ModerationResponse
{
    public string Id { get; set; }
    public string Model { get; set; }
    public IEnumerable<ModerationResult> Results { get; set; }
    public bool Flagged => Results.Any(a => a.Flagged);
}