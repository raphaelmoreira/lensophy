namespace Lensophy.Infrastructure.LargeLanguageModel;

internal abstract class BaseApi
{
    protected readonly IJsonSerializer Json = new JilSerializer();
}