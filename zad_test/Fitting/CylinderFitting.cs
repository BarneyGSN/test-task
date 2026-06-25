namespace zad_test.Fitting;
using System;
using System.Collections.Generic;
using MathNet.Spatial.Euclidean;
using zad_test.Models;

public class CylinderFitting
{
    public FittingResult FitCylinder(List<Point3D> section)
    {
        int n = section.Count;

        double meanX = 0, meanY = 0;
        foreach (var p in section)
        {
            meanX += p.X;
            meanY += p.Y;
        }

        meanX /= n;
        meanY /= n;

        double sumRadius = 0;
        foreach (var p in section)
        {
            double dx = p.X - meanX;
            double dy = p.Y - meanY;

            sumRadius += Math.Sqrt(dx * dx + dy * dy);
        }

        double fittedRadius = sumRadius / n;

        double sumSqrErrors = 0;
        foreach (var p in section)
        {
            double dx = p.X - meanX;
            double dy = p.Y - meanY;

            double distToCenter = Math.Sqrt(dx * dx + dy * dy);
            double error = distToCenter - fittedRadius;
            sumSqrErrors += error * error;
        }

        double rmse = Math.Sqrt(sumSqrErrors / n);

        return new FittingResult
        {
            ElementType = "Cylinder",
            Radius = fittedRadius,
            Width = fittedRadius * 2,
            Height = fittedRadius * 2,
            Rmse = rmse,
            PointsCount = n,
            CenterX = meanX,
            CenterY = meanY,
        };
    }
}