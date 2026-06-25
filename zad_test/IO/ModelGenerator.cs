using System.Globalization;
using System.Numerics;
using MathNet.Spatial.Euclidean;
using zad_test.Models;
namespace zad_test.IO;
using zad_test.Fitting;
using zad_test.Geometry;

public class ModelGenerator
{
    public static void GenerateModel(FittingResult fittingResult, CoordinateSystem localToGlobal, double length,
        string outputPath)
    {
        double hw = fittingResult.Width / 2;
        double hh = fittingResult.Height / 2;

        double cx = fittingResult.CenterX;
        double cy = fittingResult.CenterY;
        
        Point3D[] localVertices = new Point3D[8]
        {
            new Point3D(cx - hw, cy - hh, 0),
            new Point3D(cx + hw, cy - hh, 0),
            new Point3D(cx + hw, cy + hh, 0),
            new Point3D(cx - hw, cy + hh, 0),
            new Point3D(cx - hw, cy - hh, length),
            new Point3D(cx + hw, cy - hh, length),
            new Point3D(cx + hw, cy + hh, length),
            new Point3D(cx - hw, cy + hh, length),
        };

        Point3D[] globalVertices = new Point3D[8];

        for (int i = 0; i < 8; i++)
        {
            globalVertices[i] = localToGlobal.Transform(localVertices[i]);
        }

        using (StreamWriter writer = new StreamWriter(outputPath))
        {
            writer.WriteLine("# Wygenerowany model 3D wspornika");
            writer.WriteLine($"# Liczba punktów dopasowania: {fittingResult.PointsCount}, RMSE:  {fittingResult.Rmse}");

            var culture = CultureInfo.InvariantCulture;

            foreach (var v in globalVertices)
            {
                writer.WriteLine(string.Format(culture, "v {0:F4} {1:F4} {2:F4}", v.X, v.Y, v.Z));
            }

            writer.WriteLine();

            writer.WriteLine("f 1 4 3 2");
            writer.WriteLine("f 5 6 7 8");
            writer.WriteLine("f 1 2 6 5");
            writer.WriteLine("f 2 3 7 6");
            writer.WriteLine("f 3 4 8 7");
            writer.WriteLine("f 4 1 5 8");
        }
    }

    public static void GenerateCylinderModel(FittingResult fittingResult, CoordinateSystem localToGlobal, double length,
        string outputPath)
    {
        int segments = 16;
        double r = fittingResult.Radius;
        double cx = fittingResult.CenterX;
        double cy = fittingResult.CenterY;

        List<Point3D> localVertices = new List<Point3D>();
        for (int i = 0; i < segments; i++)
        {
            double angle = 2 * Math.PI * i / segments;
            double x = r * Math.Cos(angle);
            double y = r * Math.Sin(angle);

            localVertices.Add(new Point3D(cx + x, cy + y, 0));
        }
        
        for (int i = 0; i < segments; i++)
        {
            double angle = 2 * Math.PI * i / segments;
            double x = r * Math.Cos(angle);
            double y = r * Math.Sin(angle);

            localVertices.Add(new Point3D(cx + x, cy + y, length));
        }
        
        Point3D[] globalVertices = new Point3D[localVertices.Count];
        for (int i = 0; i < localVertices.Count; i++)
        {
            globalVertices[i] = localToGlobal.Transform(localVertices[i]);
        }

        using (StreamWriter writer = new StreamWriter(outputPath))
        {
            writer.WriteLine("# Wygenerowany model 3D rury montażowej");
            writer.WriteLine($"#Liczba punktów dopasowania:  {fittingResult.PointsCount}, RMSE:  {fittingResult.Rmse}");

            var culture = CultureInfo.InvariantCulture;
            foreach (var v in globalVertices)
            {
                writer.WriteLine(string.Format(culture, "v {0:F4} {1:F4} {2:F4}", v.X, v.Y, v.Z));
            }

            writer.WriteLine();

            for (int i = 0; i < segments; i++)
            {
                int v1 = i + 1;
                int v2 = ((i + 1) % segments) + 1;
                int v3 = v2 + segments;
                int v4 = v1 + segments;

                writer.WriteLine($"f {v1} {v4} {v3} {v2}");
            }
        }
    }
}