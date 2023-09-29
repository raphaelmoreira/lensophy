using Lensophy.Dto;
using Lensophy.IntegrationTest.Fixture;
using Lensophy.Interface;

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
    [Trait("Factory", nameof(Lens.CreateWithOpenAi))]
    public void Lens_CreateWithOpenAi_ShouldNotBe_Null()
    {
        //arrange
        var lensophyCreated = Lens.CreateWithOpenAi(_openAiFixture.OpenAiConfig);
        
        //assert
        lensophyCreated.Should().NotBeNull();
    }
    
    [Fact]
    [Trait("Interface", nameof(ILensophy.IsHarmful))]
    public async Task Lensophy_IsHarmful_WithContext_ShouldBe_True()
    {
        //arrange
        var contentToAnalyse = new ContentAnalyse(HarmfullMessage, EmptyContextMessage);
        
        //act
        var contentAnalysed = 
            await _openAiFixture.OpenAiLensophy.IsHarmful(_openAiFixture.CurrentHttpClient, contentToAnalyse);
        
        //assert
        contentAnalysed.Should().BeTrue();
    }
    
    [Fact]
    [Trait("Interface", nameof(ILensophy.IsHarmful))]
    public async Task Lensophy_IsHarmful_WithoutContext_ShouldBe_True()
    {
        //arrange
        var contentToAnalyse = new ContentAnalyse(HarmfullMessage, EmptyContextMessage);
        
        //act
        var contentAnalysed = 
            await _openAiFixture.OpenAiLensophy.IsHarmful(_openAiFixture.CurrentHttpClient, contentToAnalyse);
        
        //assert
        contentAnalysed.Should().BeTrue();
    }
    
    [Fact]
    [Trait("Interface", nameof(ILensophy.Analyse))]
    public async Task Lensophy_Analyse_WithContext_ShouldBe_Harmful()
    {
        //arrange
        var contentToAnalyse = new ContentAnalyse(HarmfullMessage, ContextMessage);
        
        //act
        var contentAnalysed = 
            await _openAiFixture.OpenAiLensophy.Analyse(_openAiFixture.CurrentHttpClient, contentToAnalyse);
        
        //assert
        contentAnalysed.IsHarmful.Should().BeTrue();
    }
    
    [Fact]
    [Trait("Interface", nameof(ILensophy.Analyse))]
    public async Task Lensophy_Analyse_WithoutContext_ShouldBe_Harmful()
    {
        //arrange
        var contentToAnalyse = new ContentAnalyse(HarmfullMessage, EmptyContextMessage);
        
        //act
        var contentAnalysed = 
            await _openAiFixture.OpenAiLensophy.Analyse(_openAiFixture.CurrentHttpClient, contentToAnalyse);
        
        //assert
        contentAnalysed.IsHarmful.Should().BeTrue();
    }
}