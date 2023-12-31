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

        var openAiConfigSecret = _configuration.GetSection("openaiconfigsecret").Value;
        var key = openAiConfigSecret ?? throw new Exception("OpenAI key not found!");
        
        var currentHttpClient = new DefaultHttpClientFactory().CreateClient();
        currentHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", key);
        
        LensophyService = new LensophyService(currentHttpClient);
    }
}