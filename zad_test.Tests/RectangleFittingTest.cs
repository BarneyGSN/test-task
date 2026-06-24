using System;
using System.Collections.Generic;
using MathNet.Spatial.Euclidean;
using Xunit;
using zad_test.Fitting;
using zad_test.Models;

namespace zad_test.Tests;

public class RectangleFittingTests
{
    [Fact] 
    public void FitRectangle_PerfectSquare_ReturnsExactGeometryAndZeroRmse()
    {
        var fitter = new RectangleFitting();
        var sectionPoints = new List<Point3D>
        {
            new Point3D(-0.2, -0.1, 0.0),
            new Point3D( 0.2, -0.1, 0.0),
            new Point3D( 0.2,  0.1, 0.0),
            new Point3D(-0.2,  0.1, 0.0)
        };
        
        FittingResult result = fitter.FitRectangle(sectionPoints);
        
        Assert.Equal("Rectangle", result.ElementType);
        Assert.Equal(0.4, result.Width, precision: 4);
        Assert.Equal(0.2, result.Height, precision: 4);
        Assert.Equal(0.0, result.Angle, precision: 4);
        Assert.Equal(0.0, result.Rmse, precision: 4);
        Assert.Equal(4, result.PointsCount);
    }

    [Fact]
    public void FitRectangle_EmptySection_ReturnsEmptyResultOrDoesNotThrow()
    {
        var fitter = new RectangleFitting();
        var emptyList = new List<Point3D>();
        
        var result = fitter.FitRectangle(emptyList);
        
        Assert.NotNull(result);
        Assert.Equal(0, result.PointsCount);
    }
}