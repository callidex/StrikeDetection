
namespace Services.Nav.VLBL
{
    public interface IVLBLMeasurement
    {
        double BearingFromRefPoint { get; }
        double DistanceToRefPoint { get; }
        void UpdateRefPoint(GeoPoint refPoint);
    }
}
