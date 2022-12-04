using Microsoft.AspNetCore.Mvc;
using StrikeDetection.Models;
using StrikeDetection.Services;

namespace StrikeDetection.Controllers;

[ApiController]
[Route("[controller]")]
public class SampleController : ControllerBase
{

    private readonly ILogger<SampleController> _logger;
    private readonly IDbService _service;

    public SampleController(ILogger<SampleController> logger, IDbService service)
    {
        _logger = logger;
        _service = service;
    }
    [HttpPost]
    public async Task<int> AddSample([FromBody] Sample status)
    {
        return await _service.AddSample(status);
    }

    [HttpGet]
    public async Task<Sample[]> GetRecent(int count, int detectorID = 0)
    {
        return await _service.GetLastXSampleAsync(count, detectorID);
    }
}
