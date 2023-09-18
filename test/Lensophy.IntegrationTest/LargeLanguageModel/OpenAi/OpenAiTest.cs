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
    [Trait("OpenAI", "Verifica a requisição é inválida")]
    public async Task OpenAiApiCallShouldBeInvalidRequestError()
    {
        //arrange
        const string initialErrorMessageExpected = "[invalid_request_error] - ";
        var contentToAnalyse = new ContentAnalyse();
        
        //act
        var contentAnalysed = await _fixture.LensophyLanguageModel
            .Analyse(_fixture.CurrentHttpClient, contentToAnalyse)
            .ConfigureAwait(false);
        
        //assert
        contentAnalysed.ErrorMessage.Should().StartWith(initialErrorMessageExpected);
    }
}