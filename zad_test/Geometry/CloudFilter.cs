using System.Numerics;
using zad_test.Models;
namespace zad_test.Geometry;


public class CloudFilter
{
    public List<Vector3> filteredAndSectionPoints(IEnumerable<Point3D> points, Matrix4x4 globalToLocal, float width, float height, float length)
    {
        var filtered = new List<Vector3>();
        var section = new List<Vector3>();
        float halfWidth = width / 2f;
        float halfHeight = height / 2f;
        float dZ = 0.05f;
        foreach (var p in points)
        {
            Vector3 globalVec = new Vector3(p.X, p.Y, p.Z);
            Vector3 localVec = Vector3.Transform(globalVec, globalToLocal);

            if (localVec.Z >= 0 && localVec.Z <= length &&
                localVec.X >= -halfWidth && localVec.X <= halfWidth &&
                localVec.Y >= -halfHeight && localVec.Y <= halfHeight)
            {
                filtered.Add(localVec);
            }
        }

        foreach (var p in filtered)
        {
            if (length / 2 - dZ <=p.Z && length / 2 + dZ >= p.Z)
            {
                section.Add(p);
            }
        }
        return filtered;
        return section;
    }
}