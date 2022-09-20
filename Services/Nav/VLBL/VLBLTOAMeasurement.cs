﻿
namespace Services.Nav.VLBL
{
    public class VLBLTOAMeasurement : GeoPoint3DD, IVLBLMeasurement
    {
        #region Properties

        double bearingFromRefPoint;
        double distanceToRefPoint;

        #endregion

        #region Constructor

        public VLBLTOAMeasurement(double lat, double lon, double dpt, double slantRange)
            : base(lat, lon, dpt, slantRange)
        {
            bearingFromRefPoint = double.NaN;
            distanceToRefPoint = double.NaN;
        }

        public VLBLTOAMeasurement(double lat, double lon, double dpt, double slantRange, GeoPoint3D refPoint)
            : base(lat, lon, dpt, slantRange)
        {
            UpdateRefPoint(refPoint);
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return string.Format("{0}, BFR: {1:F03}°, DTR: {2:F03} m", base.ToString(), BearingFromRefPoint, DistanceToRefPoint);
        }

        #endregion

        #region INavigationMeasurement

        public double BearingFromRefPoint
        {
            get
            {
                if (!double.IsNaN(bearingFromRefPoint))
                    return bearingFromRefPoint;
                else
                    throw new InvalidOperationException("Property not initialized");
            }
        }

        public double DistanceToRefPoint
        {
            get
            {
                if (!double.IsNaN(distanceToRefPoint))
                    return distanceToRefPoint;
                else
                    throw new InvalidOperationException("Property not initialized");
            }
        }

        public void UpdateRefPoint(GeoPoint refPoint)
        {
            double sp_lat_rad = Algorithms.Deg2Rad(refPoint.Latitude);
            double sp_lon_rad = Algorithms.Deg2Rad(refPoint.Longitude);
            double ep_lat_rad = Algorithms.Deg2Rad(Latitude);
            double ep_lon_rad = Algorithms.Deg2Rad(Longitude);
            distanceToRefPoint = Algorithms.HaversineInverse(sp_lat_rad, sp_lon_rad, ep_lat_rad, ep_lon_rad, Algorithms.WGS84Ellipsoid.MajorSemiAxis_m);
            bearingFromRefPoint = Algorithms.Rad2Deg(Algorithms.HaversineInitialBearing(sp_lat_rad, sp_lon_rad, ep_lat_rad, ep_lon_rad));
        }

        #endregion
    }
}
