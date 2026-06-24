using zad_test.Fitting;
using System;
using System.Collections.Generic;
using Xunit;
using MathNet.Spatial.Euclidean;
using zad_test.Models;
namespace zad_test.Tests;

public class CylinderFittingTest
{
    [Fact]
    public void FitCylinder_PerfectCircle_ReturnsExactGeometryAndZeroRmse()
    {
        var fitter = new CylinderFitting();
        var section = new List<Point3D>();
        double expectedX = 0;
        double expectedY = 0;
        double expectedRadius = 2;
        int segments = 8;

        for (int i = 0; i < segments; i++)
        {
            double angle = (2 * Math.PI * i) / segments;
            double x = expectedX + expectedRadius * Math.Cos(angle);
            double y = expectedY + expectedRadius * Math.Sin(angle);
            section.Add(new Point3D(x, y, 0));
        }
        
        FittingResult result = fitter.FitCylinder(section);
        
        Assert.Equal("Cylinder", result.ElementType);
        Assert.Equal(expectedRadius, result.Radius, precision: 4);
        Assert.Equal(expectedX, result.CenterX, precision: 4);
        Assert.Equal(expectedY, result.CenterY, precision: 4);
        Assert.Equal(0.0, result.Rmse, precision: 4);
    }

    [Fact]
    public void FitCylinder_NoisyPoints()
    {
        var fitter = new CylinderFitting();
        var section = new List<Point3D>()
        {
            new Point3D(1.02, 0.0, 0.0),
            new Point3D(-0.98, 0.0, 0.0),
            new Point3D(0.0, 1.02, 0.0),
            new Point3D(0.0, -0.98, 0.0)
        };
        
        FittingResult result = fitter.FitCylinder(section);
        
        Assert.Equal("Cylinder", result.ElementType);
        Assert.Equal(0.01, result.CenterX, precision: 2);
        Assert.Equal(0.01, result.CenterY, precision: 2);
        Assert.Equal(1.0, result.Radius, precision: 2);
        Assert.Equal(0.01, result.Rmse, precision: 2);
    }
}