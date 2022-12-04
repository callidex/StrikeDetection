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

    public ActionResult<StrikePoint> FindStrikePoint([FromBody] DetectionInstance detections)
    {

        return Ok(_service.TestAsync(detections));
    }

    [HttpPost]
    [Route("FindStrikePointWithDetectors")]
    public async Task<ActionResult<StrikePoint>> FindStrikePointWithDetectors([FromBody] List<DetectorSample> detectorSamples)
    {
        var dt = new DetectionInstance();
        dt.Points = new List<DetectorPointDTO>();
        foreach (var d in detectorSamples)
        {
            var point = await _service.GetPointFromDetectorIDViaRemote(d.DetectorID);
            if (point != null)
            {
                point.TimeFromTarget = d.TimeStamp;
                dt.Points.Add(point);
            }
        }
        var dr = new DetectorsResponse()
        {
            Components = dt
        };
        dr.Result = await _service.TestAsync(dt);
        return Ok(dr);
    }
    public class DetectorsResponse
    {
        public string Result { get; set; }
        public DetectionInstance Components { get; set; }

    }

    public class DetectorSample
    {
        public int DetectorID { get; set; }
        public double TimeStamp { get; set; }
    }
}
