using System.Drawing;
using MathNet.Spatial.Euclidean;
using zad_test.Geometry;

namespace zad_test.Tests;

public class CloudFilterTest
{
    [Fact]
    public void FilterCylindricalPoints()
    {
        var filter = new CloudFilter();
        var localSystem = new CoordinateSystem();
        double searchRadius = 0.4;
        double length = 6.0;

        var points = new List<Point3D>()
        {
            new Point3D(0.1, 0.1, 3.0),
            new Point3D(0.7, 0.1, 3.0),
            new Point3D(0.0, 0.0, 8.0),
            new Point3D(0.0, 0.0, -2.0),
        };
        
        ExtractionResult result = filter.cylinderFilteredAndSectionPoints(points, localSystem, searchRadius, length);
        
        Assert.Single(result.FilteredPoints);
        Assert.Equal(3.0, result.FilteredPoints[0].Z);
    }
}