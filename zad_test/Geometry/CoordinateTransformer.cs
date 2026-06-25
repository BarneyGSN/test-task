using MathNet.Spatial.Euclidean;
namespace zad_test.Geometry;
using System;
public class CoordinateTransformer
{
    public CoordinateSystem globalToLocal (Point3D p1, Point3D p2, out double length)
    {
        Vector3D axisZ = p1.VectorTo(p2);
        length = axisZ.Length;
        UnitVector3D zDirection = axisZ.Normalize();

        Vector3D reference = Math.Abs(zDirection.X) < 0.9 ? new  Vector3D(1, 0, 0) : new Vector3D(0, 1, 0);
        UnitVector3D xDirection = reference.CrossProduct(zDirection).Normalize();
        UnitVector3D yDirection = xDirection.CrossProduct(zDirection);
        
       var localToGlobalSystem = new CoordinateSystem (p1, xDirection, yDirection, zDirection);
       return localToGlobalSystem.Invert();
    }

    public CoordinateSystem LocalToGlobal(CoordinateSystem globalToLocal)
    {
        return globalToLocal.Invert();
    }
}