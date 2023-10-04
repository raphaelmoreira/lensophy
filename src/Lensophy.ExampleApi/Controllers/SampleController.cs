using Lensophy.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Lensophy.ExampleApi.Controllers;

[ApiController]
[Route("[controller]")]
public class SampleController : ControllerBase
{
    private readonly LensophyService _lensophyService;

    public SampleController(LensophyService lensophyService) => _lensophyService = lensophyService;

    [HttpPost(Name = "Analyse")]
    public async Task<ContentAnalysed> Analyse([FromBody]ContentAnalyse contentToAnalyse)
    {
        var contentAnalysed = await _lensophyService.AnalyseAsync(contentToAnalyse).ConfigureAwait(false);
        return contentAnalysed;
    }
}