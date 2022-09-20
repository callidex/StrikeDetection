
/// <summary>
/// The grunt work goes here
/// </summary>
public class DetectionService
{
    public List<DetectorPoint> Points = new List<DetectorPoint>();


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
