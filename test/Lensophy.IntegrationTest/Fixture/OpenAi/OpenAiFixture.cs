using Lensophy.Domain.Interface;
using Lensophy.Infrastructure.LargeLanguageModel.OpenAi;
using Lensophy.IntegrationTest.Util;
using Microsoft.Extensions.Configuration;

namespace Lensophy.IntegrationTest.Fixture.OpenAi;

public record OpenAiFixture
{
    public readonly HttpClient CurrentHttpClient = new DefaultHttpClientFactory().CreateClient();
    public readonly ILensophyLanguageModel LensophyLanguageModel;
    private readonly string? _secret = new ConfigurationBuilder().AddJsonFile("appsettings.Test.json").Build()
        .GetSection("OpenAiConfig:Secret").Value;

    public OpenAiFixture()
    {
        var config = new OpenAiConfig(_secret);
        LensophyLanguageModel = new OpenAiApi(config);
    }
}