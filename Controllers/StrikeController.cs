using Microsoft.AspNetCore.Mvc;

namespace StrikeDetection.Controllers;

public class DetectionInstance
{
    public List<DetectorPointDTO> Points { get; set; }
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

        List<DetectorPoint> points = new List<DetectorPoint>();
        points.Add(new DetectorPoint((float)1.5325811767578125E+002, (float)-2.7504890441894531E+001, (float)6.5891998291015625E+001, "8"));
        points.Add(new DetectorPoint((float)1.5311061096191406E+002, (float)-2.7495552062988281E+001, (float)6.6796997070312500E+001, "5"));
        points.Add(new DetectorPoint((float)1.5321546936035156E+002, (float)-2.7617309570312500E+001, (float)2.5816299438476562E+002, "7"));
        points.Add(new DetectorPoint((float)1.5320046997070312E+002, (float)-2.7539850234985352E+001, (float)6.6009002685546875E+001, "2"));

        return Ok(_service.Test(points));
    }
}
