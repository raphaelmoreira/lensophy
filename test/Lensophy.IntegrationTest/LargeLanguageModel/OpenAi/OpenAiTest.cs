using Lensophy.Domain.Dto;
using Lensophy.IntegrationTest.Fixture.OpenAi;

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
        var context = string.Empty;
        const string message = "you are a scum";
        var contentToAnalyse = new ContentAnalyse(context, message);
        
        //act
        var contentAnalysed = await _fixture.LensophyLanguageModel
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
        const string context = "Crítica do filme Indiana Jones";
        const string message = "Você é chato, sua opinião não presta. vá estudar pra ficar mais inteligente. " +
                               "Os filmes antigos eram melhores, babaca!";
        
        var contentToAnalyse = new ContentAnalyse(context, message);
        
        //act
        var contentAnalysed = await _fixture.LensophyLanguageModel
            .Analyse(_fixture.CurrentHttpClient, contentToAnalyse)
            .ConfigureAwait(false);
        
        //assert
        contentAnalysed.IsHarmful.Should().BeTrue();
    }
}