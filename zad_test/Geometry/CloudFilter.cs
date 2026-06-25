using System.Numerics;
using MathNet.Spatial.Euclidean;
using zad_test.Models;
namespace zad_test.Geometry;

public class ExtractionResult
{
    public List<Point3D> FilteredPoints { get; set; } = new List<Point3D>();
    public List<Point3D> SectionPoints { get; set; } = new List<Point3D>();
}
public class CloudFilter
{
    public ExtractionResult supportFilteredAndSectionPoints(IEnumerable<Point3D> points, CoordinateSystem globalToLocal, double width, double height, double length)
    {
        var filtered = new List<Point3D>();
        var section = new List<Point3D>();
        double halfWidth = width / 2;
        double halfHeight = height / 2;
        double dZ = 0.05;
       
        //CoordinateSystem localToGlobalSystem = globalToLocal.Invert();
        
        foreach (var p in points)
        {
            Point3D localPoint = globalToLocal.Transform(p);

            if (localPoint.Z >= 0 && localPoint.Z <= length &&
                localPoint.X >= -halfWidth && localPoint.X <= halfWidth &&
                localPoint.Y >= -halfHeight && localPoint.Y <= halfHeight)
            {
                filtered.Add(localPoint);
            }
        }

        foreach (var p in filtered)
        {
            if ((length / 2.0 - dZ) <=p.Z && (length / 2.0 + dZ) >= p.Z)
            {
                section.Add(p);
            }
        }

        return new ExtractionResult
        {
            FilteredPoints = filtered,
            SectionPoints = section
        };
    }

    public ExtractionResult cylinderFilteredAndSectionPoints(IEnumerable<Point3D> points,
        CoordinateSystem globalToLocal, double radius, double length)
    {
        var filtered = new List<Point3D>();
        var section = new List<Point3D>();
        double sectionZ = length / 2;
        double dZ = 0.05;
        
        //CoordinateSystem localToGlobalSystem = globalToLocal.Invert();
        
        foreach (var p in points)
        {
            Point3D localPoint = globalToLocal.Transform(p);

            if (localPoint.Z >= 0 && localPoint.Z <= length)
            {
                double distToRadius = Math.Sqrt(localPoint.X * localPoint.X + localPoint.Y * localPoint.Y);
                if (distToRadius <= radius)
                {
                    filtered.Add(localPoint);

                    if (Math.Abs(localPoint.Z - sectionZ) <= dZ / 2.0)
                    {
                        section.Add(localPoint);
                    }
                }
            }
            
        }

        return new ExtractionResult
        {
            FilteredPoints = filtered,
            SectionPoints = section
        };
    }
}