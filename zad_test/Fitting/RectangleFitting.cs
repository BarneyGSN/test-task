using System.Numerics;
namespace zad_test.Fitting;
using zad_test.Models;

public class RectangleFitting
{
    public FittingResult FitRectangle(List<Vector3> section)
    {
        float minX = float.MaxValue;
        float maxX = float.MinValue;
        float minY = float.MaxValue;
        float maxY = float.MinValue;
        int n = section.Count;
        float meanX = 0, meanY = 0;
        
        foreach (var p in section)
        {
            meanX += p.X;
            meanY += p.Y;
        }
       
        meanX /= n;
        meanY /= n;

        float cXX = 0, cYY = 0, cXY = 0;
        foreach (var p in section)
        {
            float dx = p.X - meanX;
            float dy = p.Y - meanY;
            
            cXX += dx * dx;
            cYY += dy * dy;
            cXY += dx * dy;
        }

        cXX /= n;
        cYY /= n;
        cXY /= n;

        float angle = 0.5f * MathF.Atan2(2 * cXY, cXX - cYY);
        float cosA = MathF.Cos(-angle);
        float sinA = MathF.Sin(-angle);
        
        List <Vector2> rotatedPoints = new List<Vector2>();
        foreach (var p in section)
        {
            float dx = p.X - meanX;
            float dy = p.Y - meanY;
            
            float rx = dx * cosA - dy * sinA;
            float ry = dx * sinA + dx * sinA;
            
            rotatedPoints.Add(new Vector2(rx, ry));
        }

        foreach (var p in rotatedPoints)
        {
            if (p.X < minX)
            {
                minX = p.X;
            }

            if (p.X > maxX)
            {
                maxX = p.X;
            }

            if (p.Y < minY)
            {
                minY = p.Y;
            }

            if (p.Y > maxY)
            {
                maxY = p.Y;
            }
        }
        float fittedWidth = maxX - minX;
        float fittedHeight = maxY - minY;
    } 
}