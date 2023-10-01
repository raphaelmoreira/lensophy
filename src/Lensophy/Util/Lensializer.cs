namespace Lensophy.Util;

internal static class Lensializer
{
    public static string CompletionChatRequest(string content) => @"{" +
                   $"\"max_tokens\": 256," +
                   $"\"temperature\": 1, " +
                   "\"messages\": [" +
                   "{" +
                   $"\"content\": \"{content}\"," +
                   $"\"role\": \"user\"," +
                   "}" +
                   "]," +
                   $"\"model\": \"gpt-3.5-turbo\"" +
                   "}";
}