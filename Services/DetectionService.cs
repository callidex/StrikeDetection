
using Geolocation;
using Services.Nav;


public class DetectionInstance
{
    public List<DetectorPointDTO> Points { get; set; }
}

/// <summary>
/// The grunt work goes here
/// </summary>
public class DetectionService
{
    const double velocity = 299792458;

    public async Task<string> TestAsync(DetectionInstance instance)
    {
        double target_lat = 0;
        double target_lon = 0;
        double radialError = 0;
        double target_z = 0;
        int it_cnt;
        List<GeoPoint3DT> allDetectors = new List<GeoPoint3DT>();
        if (!instance.Points.Any() || instance.Points.Count < 3) return "Not enough points to calculate";
        // we have X points, location and time they 'heard' the signal.  Now convert that into deltas between them
        var ordered = instance.Points.OrderBy(x => x.TimeFromTarget);
        var current = ordered.First().TimeFromTarget;
        foreach (var p in ordered)
        {
            var delta = p.TimeFromTarget - current;
            current = p.TimeFromTarget;
            allDetectors.Add(new GeoPoint3DT(p.Lat, p.Lon, p.Hgt, delta, p.Label));
        }

        return await Task.Run(() =>
        {
            var centre = Navigation.TDOA_Locate3D(allDetectors.ToArray(),
                                          Algorithms.NLM_DEF_IT_LIMIT, Algorithms.NLM_DEF_PREC_THRLD, 10,
                                          Algorithms.WGS84Ellipsoid,
                                          out target_lat, out target_lon, out target_z, out radialError, out it_cnt);
            if (it_cnt > Algorithms.NLM_DEF_IT_LIMIT)
            {
                return "No Solution Found";
            }
            return $"https://www.google.com/maps/@{target_lat:F07},{target_lon:F07},18z     LAT: {target_lat:F07}°   LON: {target_lon:F07}°  Height: {target_z} Estimated radial error: {radialError:F03} m, Iterations: {it_cnt}     ";
        });
    }

    internal class BobsDetectorInfo
    {
        public int id { get; set; }

        public double Lon { get; set; }
        public double Lat { get; set; }
        public double Height { get; set; }

    }

    internal async Task<DetectorPoint> GetPointFromDetectorIDViaRemote(int detectorID)
    {
        //TODO: Call out to either db  or Bobs Lightsrv to get status
        // populate coords and label
        try
        {
            HttpClient bobsClient = new HttpClient();
            bobsClient.BaseAddress = new Uri("http://lightning.vk4ya.com:9080/");
            var response = await bobsClient.GetAsync($"/detector/{detectorID}");
            BobsDetectorInfo b = (BobsDetectorInfo)await response.Content.ReadFromJsonAsync(typeof(BobsDetectorInfo));
            Console.WriteLine("remote call complete");
            if (b != null)
                return new DetectorPoint(b.Lon, b.Lat, b.Height, detectorID.ToString());
            else return null;

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);

        }

        return null;
    }

    void CalculateDistancesFromTarget(DetectorPoint target)
    {

    }

}

public class DetectorPointDTO
{
    public double Lon { get; set; }
    public double Lat { get; set; }
    public double Hgt { get; set; }
    public double TimeFromTarget { get; set; }
    public string Label { get; set; }
}

public class DetectorPoint : DetectorPointDTO
{

    public double DistanceFromTarget { get; internal set; }
    public double Delta { get; internal set; }
    public string V { get; }

    public DetectorPoint(double Lon, double Lat)
    {
        this.Lon = Lon;
        this.Lat = Lat;
    }

    public DetectorPoint(double Lon, double Lat, double hgt) : this(Lon, Lat)
    {
        Hgt = hgt;
    }

    public DetectorPoint(double Lon, double Lat, double hgt, string v) : this(Lon, Lat, hgt)
    {
        Label = v;
    }
}
