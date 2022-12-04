using Microsoft.AspNetCore.Mvc;
using StrikeDetection.Models;
using StrikeDetection.Services;

namespace StrikeDetection.Controllers;

[ApiController]
[Route("[controller]")]
public class StatusController : ControllerBase
{
    private readonly ILogger<StatusController> _logger;
    private readonly IDbService _service;

    public StatusController(ILogger<StatusController> logger, IDbService service)
    {
        _logger = logger;
        _service = service;
    }
    [HttpPost]
    public async Task<int> AddStatus([FromBody] Status status)
    {
        return await _service.AddStatus(status);

    }



    [HttpGet]
    public async Task<Status[]> GetRecent(int count, int detectorID = 0)
    {
        return await _service.GetLastXStatusAsync(count, detectorID);
    }
}
