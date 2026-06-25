using System.Drawing;
using MathNet.Spatial.Euclidean;

namespace zad_test.IO;
using LasSharp;
using System.Collections.Generic;
using zad_test.Models;

public class LasReader
{
    public IEnumerable<Point3D> ReadPoints(string path)
    {
        var reader = new LasSharp.LasReader(path);
        LasPoint lasPoint;
        
        while (reader.MoveToNextPoint())
        {
            lasPoint = reader.CurrentPoint;
            yield return new Point3D(lasPoint.X, lasPoint.Y, lasPoint.Z);
        }
        reader.Close();
    }
}   