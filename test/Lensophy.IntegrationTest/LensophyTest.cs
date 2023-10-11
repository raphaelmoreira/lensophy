using Lensophy.Dto;
using Lensophy.IntegrationTest.Fixture;

namespace Lensophy.IntegrationTest;

public class LensophyTest : IClassFixture<OpenAiFixture>
{
    private readonly OpenAiFixture _openAiFixture;
    private const string EmptyContextMessage = "";
    private const string ContextMessage = "They are discussing the latest Indiana Jones movie";
    private const string HarmfullMessage =
        "You are an idiot, and your opinion is worthless. The old movies were better, you jerk!";
    
    public LensophyTest(OpenAiFixture openAiFixture)
    {
        _openAiFixture = openAiFixture;
    }
    
    [Fact]
    [Trait("Service", nameof(LensophyService.AnalyseAsync))]
    public async Task LensophyService_AnalyseAsync_WithContext_ShouldBe_Harmful()
    {
        //arrange
        var contentToAnalyse = new ContentAnalyse(HarmfullMessage, ContextMessage);
        
        //act
        var contentAnalysed = 
            await _openAiFixture.LensophyService.AnalyseAsync(contentToAnalyse, CancellationToken.None);
        
        //assert
        contentAnalysed.IsHarmful.Should().BeTrue();
    }
    
    [Fact]
    [Trait("Service", nameof(LensophyService.AnalyseAsync))]
    public async Task LensophyService_AnalyseAsync_WithoutContext_ShouldBe_Harmful()
    {
        //arrange
        var contentToAnalyse = new ContentAnalyse(HarmfullMessage, EmptyContextMessage);
        
        //act
        var contentAnalysed = 
            await _openAiFixture.LensophyService.AnalyseAsync(contentToAnalyse, CancellationToken.None);
        
        //assert
        contentAnalysed.IsHarmful.Should().BeTrue();
    }
}