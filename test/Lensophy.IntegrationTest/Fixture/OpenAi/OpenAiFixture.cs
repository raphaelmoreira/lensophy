using Lensophy.Domain.Dto.OpenAi;
using Lensophy.Domain.Interface;
using Lensophy.Infrastructure.LargeLanguageModel;
using Lensophy.IntegrationTest.Util;
using Microsoft.Extensions.Configuration;
using Xunit.Sdk;

namespace Lensophy.IntegrationTest.Fixture.OpenAi;

public record OpenAiFixture
{
    public readonly HttpClient CurrentHttpClient = new DefaultHttpClientFactory().CreateClient();
    public readonly ILensophyLanguageModel LensophyLanguageModel;
    private readonly string? _secret = new ConfigurationBuilder().AddJsonFile("appsettings.Test.json").Build()
        .GetSection("OpenAiConfig:Secret").Value;

    public OpenAiFixture()
    {
        if (string.IsNullOrWhiteSpace(_secret))
        {
            throw new TestClassException("The secret was not provided!");
        }
        var config = new OpenAiConfig(_secret);
        LensophyLanguageModel = new OpenAiApi(config);
    }
}