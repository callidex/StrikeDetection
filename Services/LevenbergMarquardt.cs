

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Optimization;
using StrikeDetection.Models;

namespace StrikeDetection.Controllers;

public class LevenbergMarquardt
{
    private static Vector<double> Function(Vector<double> x, PossibleStrike[] samples)
    {
        var result = Vector<double>.Build.Dense(samples.Length);

        for (int i = 0; i < samples.Length; i++)
        {
            var distance = Math.Sqrt(Math.Pow(x[0] - samples[i].Latitude, 2) + Math.Pow(x[1] - samples[i].Longitude, 2));
            result[i] = distance / x[2] - samples[i].Time;
        }

        return result;
    }

    private static Matrix<double> Jacobian(Vector<double> x, PossibleStrike[] samples)
    {
        var result = Matrix<double>.Build.Dense(samples.Length, 3);

        for (int i = 0; i < samples.Length; i++)
        {
            var distance = Math.Sqrt(Math.Pow(x[0] - samples[i].Latitude, 2) + Math.Pow(x[1] - samples[i].Longitude, 2));

            result[i, 0] = (x[0] - samples[i].Latitude) / (x[2] * distance);
            result[i, 1] = (x[1] - samples[i].Longitude) / (x[2] * distance);
            result[i, 2] = -1 / x[2];
        }

        return result;
    }

    public static Vector<double> Optimize(PossibleStrike[] samples, Vector<double> initialParams, int maxIterations, double tolerance)
    {
        var x = initialParams;
        var lambda = 0.001;

        for (int i = 0; i < maxIterations; i++)
        {
            var fx = Function(x, samples);
            var J = Jacobian(x, samples);
            var A = J.Transpose() * J;
            var g = J.Transpose() * fx;

            if (g.L2Norm() < tolerance) break;

            while (true)
            {
                var H = A + Matrix<double>.Build.DiagonalOfDiagonalVector(lambda * A.Diagonal().PointwiseMultiply(A.Diagonal()));
                var delta = H.Solve(g);
                var xNew = x - delta;

                if ((Function(xNew, samples) - fx).L2Norm() < tolerance)
                {
                    x = xNew;
                    lambda /= 10;
                    break;
                }
                else
                {
                    lambda *= 10;
                }
            }
        }

        return x;
    }
}

