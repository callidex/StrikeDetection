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

    public StrikeController(ILogger<StrikeController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "FindStrikePoint")]
    public async Task<ActionResult<StrikePoint>> FindStrikePoint([FromQuery] DetectionInstance detections)
    {
        return Ok();
    }
}
