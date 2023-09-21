namespace Lensophy.Domain.Dto.OpenAi;

internal class OpenAiError
{
    public string Message { get; set; }
    public string Type { get; set; }
    public object Param { get; set; }
    public object Code { get; set; }
}