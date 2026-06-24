using MathNet.Spatial.Euclidean;
using Xunit;
using zad_test.Geometry;

namespace zad_test.Tests;

public class CoordinateTransformerTest
{
    [Fact]
    public void GlobalToLocal_VerticalVector_CorrectZAxisAndLength()
    {
        var transformer = new CoordinateTransformer();
        Point3D p1 = new Point3D(1000, 2000, 100);
        Point3D p2 = new Point3D(1000, 2000, 105);
        
        CoordinateSystem localSystem = transformer.globalToLocal(p1, p2, out double calculatedLength);
        
        Assert.Equal(5, calculatedLength, precision: 4);
        
        Point3D localP1 = localSystem.Transform(p1);
        Assert.Equal(0.0, localP1.X, precision: 4);
        Assert.Equal(0.0, localP1.Y, precision: 4);
        Assert.Equal(0.0, localP1.Z, precision: 4);
        
        Point3D localP2 = localSystem.Transform(p2);
        Assert.Equal(0.0, localP2.X, precision: 4);
        Assert.Equal(0.0, localP2.Y, precision: 4);
        Assert.Equal(5.0, localP2.Z, precision: 4);
    }
    
}