
using Geolocation;
using Services.Nav;
/// <summary>
/// The grunt work goes here
/// </summary>
public class DetectionService
{
    const double velocity = 299792458;
    public List<DetectorPoint> Points = new List<DetectorPoint>();


    public string Test()
    {
        double target_lat = 0;
        double target_lon = 0;
        double radialError = 0;
        double target_z = 0;
        int it_cnt;
        List<GeoPoint3DT> allDetectors = new List<GeoPoint3DT>();

        foreach (var p in Points)
        {
            allDetectors.Add(new GeoPoint3DT(p.Lat, p.Lon, p.Hgt, p.Delta, p.Label));
        }

        var centre = Navigation.TDOA_Locate3D(allDetectors.ToArray(),
                                          double.NaN, double.NaN,
                                          double.NaN,
                                          Algorithms.NLM_DEF_IT_LIMIT, Algorithms.NLM_DEF_PREC_THRLD, 10,
                                          Algorithms.WGS84Ellipsoid,
                                          velocity,
                                          out target_lat, out target_lon, out target_z, out radialError, out it_cnt);
        if (it_cnt > Algorithms.NLM_DEF_IT_LIMIT)
        {
            return "No Solution Found";
        }
        return $"LAT: {target_lat:F07}°   LON: {target_lon:F07}°  Height: {target_z} Estimated radial error: {radialError:F03} m, Iterations: {it_cnt}";


    }
    void CalculateDistancesFromTarget(DetectorPoint target)
    {

    }

}

public class DetectorPoint
{
    public double Lon { get; set; }
    public double Lat { get; set; }
    public double Hgt { get; set; }

    public string Label { get; set; }
    public double DistanceFromTarget { get; internal set; }
    public double TimeFromTarget { get; internal set; }
    public double Delta { get; internal set; }
    public string V { get; }

    public DetectorPoint()
    {
    }

    public DetectorPoint(double Lon, double Lat)
    {
        this.Lon = Lon;
        this.Lat = Lat;
    }

    public DetectorPoint(double Lon, double Lat, string name) : this(Lon, Lat)
    {
        this.Label = name;
    }

    public DetectorPoint(double Lon, double Lat, double hgt) : this(Lon, Lat)
    {
        //Hgt = hgt;
        Hgt = 0;
    }

    public DetectorPoint(double Lon, double Lat, double hgt, string v) : this(Lon, Lat, hgt)
    {
        Label = v;
    }
}
