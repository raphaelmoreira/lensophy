namespace Lensophy.Dto.OpenAi;

internal record BaseResponse
{
    public OpenAiError? Error { get; set; }
    
    public string FullErrorMessage => Error is { Message: not null, Type: not null }
        ? $"[{Error?.Type}] - {Error?.Message}"
        : string.Empty;
    
    public bool HasError => !string.IsNullOrWhiteSpace(Error?.Message);
}