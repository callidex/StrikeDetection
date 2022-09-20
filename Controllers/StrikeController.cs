using Microsoft.AspNetCore.Mvc;

namespace StrikeDetection.Controllers;

public class DetectionInstance
{
    public string Name { get; set; }
    public string Coordinate { get; set; }

}

public class StrikePoint
{


}

[ApiController]
[Route("[controller]")]
public class StrikeController : ControllerBase
{
    private readonly ILogger<StrikeController> _logger;
    private readonly DetectionService _service;

    public StrikeController(ILogger<StrikeController> logger)
    {
        _logger = logger;
        _service = new DetectionService();
    }

    [HttpGet]
    [Route("FindStrikePoint")]
    public async Task<ActionResult<StrikePoint>> FindStrikePoint([FromQuery] DetectionInstance detections)
    {
        return Ok(_service.Test());
    }
}
