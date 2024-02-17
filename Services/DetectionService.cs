
//using Geolocation;
//using Services.Nav;


//public class DetectionInstance
//{
//    public List<DetectorPointDTO> Points { get; set; }
//}

///// <summary>
///// The grunt work goes here
///// </summary>
//public class DetectionService
//{
//    const double velocity = 299792458;

//    public async Task<string> TestAsync(DetectionInstance instance)
//    {
//        double target_lat = 0;
//        double target_lon = 0;
//        double radialError = 0;
//        double target_z = 0;
//        int it_cnt;
//        List<GeoPoint3DT> allDetectors = new List<GeoPoint3DT>();
//        if (!instance.Points.Any() || instance.Points.Count < 3) return "Not enough points to calculate";
//        // we have X points, location and time they 'heard' the signal.  Now convert that into deltas between them
//        var ordered = instance.Points.OrderBy(x => x.TimeFromTarget);
//        var current = ordered.First().TimeFromTarget;
//        foreach (var p in ordered)
//        {
//            var delta = p.TimeFromTarget - current;
//            current = p.TimeFromTarget;
//            allDetectors.Add(new GeoPoint3DT(p.Lat, p.Lon, p.Hgt, delta, p.Label));
//        }

//        return await Task.Run(() =>
//        {
//            var centre = Navigation.TDOA_Locate3D(allDetectors.ToArray(),
//                                          Algorithms.NLM_DEF_IT_LIMIT, Algorithms.NLM_DEF_PREC_THRLD, 10,
//                                          Algorithms.WGS84Ellipsoid,
//                                          out target_lat, out target_lon, out target_z, out radialError, out it_cnt);
//            if (it_cnt > Algorithms.NLM_DEF_IT_LIMIT)
//            {
//                return "No Solution Found";
//            }
//            return $"https://www.google.com/maps/@{target_lat:F07},{target_lon:F07},18z     LAT: {target_lat:F07}°   LON: {target_lon:F07}°  Height: {target_z} Estimated radial error: {radialError:F03} m, Iterations: {it_cnt}     ";
//        });
//    }

//    public static void NLM3D_Solve<T>(Func<T[], double, double, double, double> eps,
//                                      T[] baseElements, double xPrev, double yPrev, double zPrev,
//                                      int maxIterations, double precisionThreshold, double simplexSize,
//                                      out double xBest, out double yBest, out double zBest, out double radialError, out int itCnt)
//    {
//        const int vertexCount = 4;
//        const double reflectionCoefficient = 1;
//        const double expansionCoefficient = 2;
//        const double contractionCoefficient = 0.5;
//        const double shrinkCoefficient = 0.5;

//        double[] x = new double[vertexCount];
//        double[] y = new double[vertexCount];
//        double[] z = new double[vertexCount];
//        double[] f = new double[vertexCount];

//        InitializeVertices(x, y, z, xPrev, yPrev, zPrev, simplexSize);
//        itCnt = 0;
//        bool isFinished = false;

//        while (!isFinished)
//        {
//            for (int i = 0; i < vertexCount; i++)
//            {
//                f[i] = eps(baseElements, x[i], y[i], z[i]);
//            }

//            SortVertices(ref x, ref y, ref z, ref f);

//            double mean = CalculateMean(f);
//            double sigma = CalculateStandardDeviation(f, mean);

//            if (++itCnt > maxIterations || sigma < precisionThreshold)
//            {
//                isFinished = true;
//            }
//            else
//            {
//                double xCentroid, yCentroid, zCentroid;
//                CalculateCentroid(x, y, z, out xCentroid, out yCentroid, out zCentroid);

//                double xReflection, yReflection, zReflection;
//                CalculateReflection(xCentroid, yCentroid, zCentroid, x[vertexCount - 1], y[vertexCount - 1], z[vertexCount - 1], reflectionCoefficient,
//                                    out xReflection, out yReflection, out zReflection);
//                double fReflection = eps(baseElements, xReflection, yReflection, zReflection);

//                if (f[0] <= fReflection && fReflection <= f[vertexCount - 2])
//                {
//                    UpdateVertex(vertexCount - 1, xReflection, yReflection, zReflection, fReflection, x, y, z, f);
//                }
//                else if (fReflection < f[0])
//                {
//                    double xExpansion, yExpansion, zExpansion;
//                    CalculateExpansion(xCentroid, yCentroid, zCentroid, xReflection, yReflection, zReflection, expansionCoefficient,
//                                       out xExpansion, out yExpansion, out zExpansion);
//                    double fExpansion = eps(baseElements, xExpansion, yExpansion, zExpansion);

//                    if (fExpansion < fReflection)
//                    {
//                        UpdateVertex(vertexCount - 1, xExpansion, yExpansion, zExpansion, fExpansion, x, y, z, f);
//                    }
//                    else
//                    {
//                        UpdateVertex(vertexCount - 1, xReflection, yReflection, zReflection, fReflection, x, y, z, f);
//                    }
//                }
//                else
//                {
//                    CalculateContraction(xCentroid, yCentroid, zCentroid, x[vertexCount - 1], y[vertexCount - 1], z[vertexCount - 1], contractionCoefficient,
//                                         out double xContraction, out double yContraction, out double zContraction);
//                    double fContraction = eps(baseElements, xContraction, yContraction, zContraction);

//                    if (fContraction < f[vertexCount - 1])
//                    {
//                        UpdateVertex(vertexCount - 1, xContraction, yContraction, zContraction, fContraction, x, y, z, f);
//                    }
//                    else
//                    {
//                        ShrinkSimplex(ref x, ref y, ref z, ref f, shrinkCoefficient);
//                    }
//                }
//            }
//        }
//        xBest = x[0];
//        yBest = y[0];
//        zBest = z[0];
//        radialError = Math.Sqrt(eps(baseElements, xBest, yBest, zBest));
//    }
//    private static void InitializeSimplex(double xPrev, double yPrev, double zPrev, double simplexSize, double[] x, double[] y, double[] z)
//    {
//        x[0] = xPrev;
//        y[0] = yPrev;
//        z[0] = zPrev;
//        x[1] = x[0] + simplexSize;
//        y[1] = y[0] + simplexSize;
//        z[1] = z[0] - simplexSize;
//        x[2] = x[0] - simplexSize / 2;
//        y[2] = y[0] + simplexSize / 2;
//        z[2] = z[0] + simplexSize / 2;
//        x[3] = x[0] + simplexSize / 2;
//        y[3] = y[0] - simplexSize / 2;
//        z[3] = z[0] - simplexSize / 2;
//    }

//    private static void CalculateCentroid(int vertexCount, double[] x, double[] y, double[] z, out double xCentroid, out double yCentroid, out double zCentroid)
//    {
//        xCentroid = 0;
//        yCentroid = 0;
//        zCentroid = 0;
//        for (int i = 0; i < vertexCount - 1; i++)
//        {
//            xCentroid += x[i];
//            yCentroid += y[i];
//            zCentroid += z[i];
//        }
//        xCentroid /= (vertexCount - 1);
//        yCentroid /= (vertexCount - 1);
//        zCentroid /= (vertexCount - 1);
//    }
//    private static void CalculateReflection(double xCentroid, double yCentroid, double zCentroid, double xWorst, double yWorst, double zWorst, double reflectionCoefficient,
//                                            out double xReflection, out double yReflection, out double zReflection)
//    {
//        xReflection = xCentroid + reflectionCoefficient * (xCentroid - xWorst);
//        yReflection = yCentroid + reflectionCoefficient * (yCentroid - yWorst);
//        zReflection = zCentroid + reflectionCoefficient * (zCentroid - zWorst);
//    }

//    private static void CalculateExpansion(double xCentroid, double yCentroid, double zCentroid, double xReflection, double yReflection, double zReflection, double expansionCoefficient,
//                                           out double xExpansion, out double yExpansion, out double zExpansion)
//    {
//        xExpansion = xCentroid + expansionCoefficient * (xReflection - xCentroid);
//        yExpansion = yCentroid + expansionCoefficient * (yReflection - yCentroid);
//        zExpansion = zCentroid + expansionCoefficient * (zReflection - zCentroid);
//    }

//    private static void CalculateContraction(double xCentroid, double yCentroid, double zCentroid, double xWorst, double yWorst, double zWorst, double contractionCoefficient,
//                                             out double xContraction, out double yContraction, out double zContraction)
//    {
//        xContraction = xCentroid + contractionCoefficient * (xWorst - xCentroid);
//        yContraction = yCentroid + contractionCoefficient * (yWorst - yCentroid);
//        zContraction = zCentroid + contractionCoefficient * (zWorst - zCentroid);
//    }

//    private static void UpdateVertex(int index, double xNew, double yNew, double zNew, double fNew, double[] x, double[] y, double[] z, double[] f)
//    {
//        x[index] = xNew;
//        y[index] = yNew;
//        z[index] = zNew;
//        f[index] = fNew;
//    }

//    private static void ShrinkSimplex(ref double[] x, ref double[] y, ref double[] z, ref double[] f, double shrinkCoefficient)
//    {
//        for (int i = 1; i < x.Length; i++)
//        {
//            x[i] = x[0] + shrinkCoefficient * (x[i] - x[0]);
//            y[i] = y[0] + shrinkCoefficient * (y[i] - y[0]);
//            z[i] = z[0] + shrinkCoefficient * (z[i] - z[0]);
//        }
//    }

//    internal class BobsDetectorInfo
//    {
//        public int id { get; set; }

//        public double Lon { get; set; }
//        public double Lat { get; set; }
//        public double Height { get; set; }

//    }

//    internal async Task<DetectorPoint> GetPointFromDetectorIDViaRemote(int detectorID)
//    {
//        //TODO: Call out to either db  or Bobs Lightsrv to get status
//        // populate coords and label
//        try
//        {
//            HttpClient bobsClient = new HttpClient();
//            bobsClient.BaseAddress = new Uri("http://lightning.vk4ya.com:9080/");
//            var response = await bobsClient.GetAsync($"/detector/{detectorID}");
//            BobsDetectorInfo b = (BobsDetectorInfo)await response.Content.ReadFromJsonAsync(typeof(BobsDetectorInfo));
//            Console.WriteLine("remote call complete");
//            if (b != null)
//                return new DetectorPoint(b.Lon, b.Lat, b.Height, detectorID.ToString());
//            else return null;

//        }
//        catch (Exception e)
//        {
//            Console.WriteLine(e.Message);

//        }

//        return null;
//    }

//    void CalculateDistancesFromTarget(DetectorPoint target)
//    {

//    }

//}

//public class DetectorPointDTO
//{
//    public double Lon { get; set; }
//    public double Lat { get; set; }
//    public double Hgt { get; set; }
//    public double TimeFromTarget { get; set; }
//    public string Label { get; set; }
//}

//public class DetectorPoint : DetectorPointDTO
//{

//    public double DistanceFromTarget { get; internal set; }
//    public double Delta { get; internal set; }
//    public string V { get; }

//    public DetectorPoint(double Lon, double Lat)
//    {
//        this.Lon = Lon;
//        this.Lat = Lat;
//    }

//    public DetectorPoint(double Lon, double Lat, double hgt) : this(Lon, Lat)
//    {
//        Hgt = hgt;
//    }

//    public DetectorPoint(double Lon, double Lat, double hgt, string v) : this(Lon, Lat, hgt)
//    {
//        Label = v;
//    }
//}
