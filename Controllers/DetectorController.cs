using Microsoft.AspNetCore.Mvc;
using StrikeDetection.Models;
using StrikeDetection.Services;

namespace StrikeDetection.Controllers;

[ApiController]
[Route("[controller]")]
public class DetectorController : ControllerBase
{

    private readonly ILogger<DetectorController> _logger;
    private readonly IDbService _service;

    public DetectorController(ILogger<DetectorController> logger, IDbService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet]
    public async Task<DetectorInfo[]> GetAll()
    {
        return await _service.GetAllDetectorsAsync();
    }
}
