using Microsoft.AspNetCore.Mvc;

namespace StrikeDetection.Controllers;



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

    [HttpPost]
    [Route("FindStrikePoint")]

    public async Task<ActionResult<StrikePoint>> FindStrikePoint([FromBody] DetectionInstance detections)
    {

        return Ok(_service.Test(detections));
    }
}
