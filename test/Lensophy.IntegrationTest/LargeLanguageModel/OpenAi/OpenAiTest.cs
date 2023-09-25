using Lensophy.Domain.Dto;
using Lensophy.Domain.Dto.OpenAi;
using Lensophy.Domain.Interface;
using Lensophy.Infrastructure.LargeLanguageModel;
using Lensophy.IntegrationTest.Fixture.OpenAi;
using Microsoft.Extensions.Configuration;

namespace Lensophy.IntegrationTest.LargeLanguageModel.OpenAi;

public class OpenAiTest : IClassFixture<OpenAiFixture>
{
    private readonly OpenAiFixture _fixture;
    public OpenAiTest(OpenAiFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    [Trait("OpenAI", "Verifica se a mensagem contém conteúdo prejudicial usando contexto")]
    public async Task ContentAnalysedWithModerationShouldBeHarmful()
    {
        //arrange
        IConfiguration? _configuration = 
            new ConfigurationBuilder().AddUserSecrets<OpenAiTest>(true).Build();
        var openAiConfig = new OpenAiConfig(_configuration["openaiconfigsecret"]);
        ILensophyLanguageModel LensophyLanguageModel = new OpenAiApi(openAiConfig);
        var context = string.Empty;
        const string message = "you are a scum";
        var contentToAnalyse = new ContentAnalyse(context, message);
        
        //act
        var contentAnalysed = await LensophyLanguageModel
            .IsHarmful(_fixture.CurrentHttpClient, contentToAnalyse)
            .ConfigureAwait(false);
        
        //assert
        contentAnalysed.Should().BeTrue();
    }
    
    [Fact]
    [Trait("OpenAI", "Verifica se a mensagem contém conteúdo prejudicial")]
    public async Task ContentAnalysedShouldBeHarmful()
    {
        //arrange
        IConfiguration? _configuration = 
            new ConfigurationBuilder().AddUserSecrets<OpenAiTest>(true).Build();
        var openAiConfig = new OpenAiConfig(_configuration["openaiconfigsecret"]);
        ILensophyLanguageModel LensophyLanguageModel = new OpenAiApi(openAiConfig);
        const string context = "Crítica do filme Indiana Jones";
        const string message = "Você é chato, sua opinião não presta. vá estudar pra ficar mais inteligente. " +
                               "Os filmes antigos eram melhores, babaca!";
        
        var contentToAnalyse = new ContentAnalyse(context, message);
        
        //act
        var contentAnalysed = await LensophyLanguageModel
            .Analyse(_fixture.CurrentHttpClient, contentToAnalyse)
            .ConfigureAwait(false);
        
        //assert
        contentAnalysed.IsHarmful.Should().BeTrue();
    }
}