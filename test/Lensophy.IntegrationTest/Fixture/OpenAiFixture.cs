using Lensophy.Domain.Dto.OpenAi;
using Lensophy.Domain.Interface;
using Lensophy.Infrastructure.LargeLanguageModel;
using Lensophy.IntegrationTest.Util;
using Microsoft.Extensions.Configuration;

namespace Lensophy.IntegrationTest.Fixture;

public record OpenAiFixture
{
    public readonly HttpClient CurrentHttpClient = new DefaultHttpClientFactory().CreateClient();
    public readonly ILensophy Lensophy;

    private readonly IConfiguration? _configuration = new ConfigurationBuilder()
        .AddUserSecrets<LensophyTest>()
        .AddEnvironmentVariables()
        .Build();

    public OpenAiFixture()
    {
        ArgumentNullException.ThrowIfNull(_configuration);
        var openAiConfig = new OpenAiConfig(_configuration.GetSection("openaiconfigsecret").Value);
        Lensophy = new OpenAiApi(openAiConfig);
    }
}