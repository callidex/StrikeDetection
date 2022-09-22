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

    [HttpPost]
    [Route("FindStrikePointWithDetectors")]
    public async Task<ActionResult<StrikePoint>> FindStrikePointWithDetectors([FromBody] List<DetectorSample> detectorSamples)
    {
        var dt = new DetectionInstance();
        foreach (var d in detectorSamples)
        {
            var point = _service.GetPointFromDetectorID(d.DetectorID);
            if (point != null)
            {
                point.TimeFromTarget = d.TimeStamp;
                dt.Points.Add(point);
            }
        }
        return Ok(_service.Test(dt));
    }
    public class DetectorSample
    {
        public int DetectorID { get; set; }
        public double TimeStamp { get; set; }
    }
}
