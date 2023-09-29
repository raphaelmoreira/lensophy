using Lensophy.Dto.OpenAi;
using Lensophy.IntegrationTest.Util;
using Lensophy.Interface;
using Lensophy.LargeLanguageModel;
using Microsoft.Extensions.Configuration;

namespace Lensophy.IntegrationTest.Fixture;

public record OpenAiFixture
{
    public readonly HttpClient CurrentHttpClient = new DefaultHttpClientFactory().CreateClient();
    public readonly ILensophy OpenAiLensophy;
    public readonly OpenAiConfig OpenAiConfig;

    private readonly IConfiguration? _configuration = new ConfigurationBuilder()
        .AddUserSecrets<LensophyTest>()
        .AddEnvironmentVariables()
        .Build();

    public OpenAiFixture()
    {
        ArgumentNullException.ThrowIfNull(_configuration);

        var openAiConfigSecret = _configuration.GetSection("openaiconfigsecret").Value;
        var key = openAiConfigSecret ?? throw new Exception("OpenAI key not found!");
        
        OpenAiConfig = new OpenAiConfig(key);
        OpenAiLensophy = new OpenAiApi(OpenAiConfig);
    }
}