﻿using System.Collections.Generic;
using System.Linq;

namespace Lensophy.Dto.OpenAi;

/// <summary>
/// Estrutura de resposta do modelo <b>moderation</b> do OpenAI.
/// </summary>
internal record ModerationResponse : BaseResponse
{
    public string Id { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public IEnumerable<ModerationResult>? Results { get; set; }
    public bool Flagged => Results?.Any(a => a.Flagged) ?? false;
}