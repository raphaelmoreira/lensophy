using System.Net.Http.Headers;
using Lensophy.IntegrationTest.Util;
using Microsoft.Extensions.Configuration;

namespace Lensophy.IntegrationTest.Fixture;

public record OpenAiFixture
{
    public readonly LensophyService LensophyService;

    private readonly IConfiguration? _configuration = new ConfigurationBuilder()
        .AddUserSecrets<LensophyTest>()
        .AddEnvironmentVariables()
        .Build();

    public OpenAiFixture()
    {
        ArgumentNullException.ThrowIfNull(_configuration);

        var openAiConfigSecretGitAction = _configuration.GetSection("OPENAICONFIGSECRET").Value;
        
        var openAiConfigSecret = openAiConfigSecretGitAction ?? _configuration.GetSection("OpenAiConfig:Secret").Value;
        var key = openAiConfigSecret ?? throw new Exception("OpenAI key not found!");
        
        var currentHttpClient = new DefaultHttpClientFactory().CreateClient();
        currentHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", key);
        
        LensophyService = new LensophyService(currentHttpClient);
    }
}