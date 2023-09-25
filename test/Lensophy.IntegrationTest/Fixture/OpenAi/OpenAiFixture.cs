using Lensophy.Domain.Dto.OpenAi;
using Lensophy.Domain.Interface;
using Lensophy.Infrastructure.LargeLanguageModel;
using Lensophy.IntegrationTest.LargeLanguageModel.OpenAi;
using Lensophy.IntegrationTest.Util;
using Microsoft.Extensions.Configuration;

namespace Lensophy.IntegrationTest.Fixture.OpenAi;

public record OpenAiFixture
{
    public readonly HttpClient CurrentHttpClient = new DefaultHttpClientFactory().CreateClient();
    public readonly ILensophyLanguageModel LensophyLanguageModel;

    private readonly IConfiguration? _configuration = 
        new ConfigurationBuilder().AddUserSecrets<OpenAiTest>(true).Build();

    public OpenAiFixture()
    {
        ArgumentNullException.ThrowIfNull(_configuration);
        var openAiConfig = new OpenAiConfig(_configuration["OpenAiConfigSecret"]);
        LensophyLanguageModel = new OpenAiApi(openAiConfig);
    }
}