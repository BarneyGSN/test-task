using MathNet.Spatial.Euclidean;

namespace zad_test.Fitting;
using zad_test.Models;

public class RectangleFitting
{
    public FittingResult FitRectangle(List<Point3D> section)
    {
        double minX = double.MaxValue;
        double maxX = double.MinValue;
        double minY = double.MaxValue;
        double maxY = double.MinValue;
        int n = section.Count;
        
        double meanX = 0, meanY = 0;
        foreach (var p in section)
        {
            meanX += p.X;
            meanY += p.Y;
        }
       
        meanX /= n;
        meanY /= n;

        double cXX = 0, cYY = 0, cXY = 0;
        foreach (var p in section)
        {
            double dx = p.X - meanX;
            double dy = p.Y - meanY;
            
            cXX += dx * dx;
            cYY += dy * dy;
            cXY += dx * dy;
        }

        cXX /= n;
        cYY /= n;
        cXY /= n;

        double angle = 0.5 * Math.Atan2(2 * cXY, cXX - cYY);
        double cosA = Math.Cos(-angle);
        double sinA = Math.Sin(-angle);
        
        List <Vector2D> rotatedPoints = new List<Vector2D>();
        foreach (var p in section)
        {
            double dx = p.X - meanX;
            double dy = p.Y - meanY;
            
            double rx = dx * cosA - dy * sinA;
            double ry = dx * sinA + dy * cosA;
            
            rotatedPoints.Add(new Vector2D(rx, ry));
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
        double fittedWidth = maxX - minX;
        double fittedHeight = maxY - minY;
        
        double rotCenterX = minX + fittedWidth / 2;
        double rotCenterY = minY + fittedHeight / 2;

        double cosA_inv = Math.Cos(angle);
        double sinA_inv = Math.Sin(angle);
        
        double localCenterX = meanX + (rotCenterX * cosA_inv - rotCenterY * sinA_inv);
        double localCenterY = meanY + (rotCenterX * sinA_inv + rotCenterY * cosA_inv);
        
        double sumSquaredErrors = 0;
        foreach (var p in rotatedPoints)
        {
            double distToLeft = Math.Abs(p.X - minX);
            double distToRight = Math.Abs(p.X - maxX);
            double distToBottom = Math.Abs(p.Y - minY);
            double distToTop = Math.Abs(p.Y - maxY);
            
            double minDist = Math.Min(Math.Min(distToLeft, distToRight), Math.Min(distToBottom, distToTop));
            sumSquaredErrors += minDist * minDist;
        }
        double rmse = Math.Sqrt(sumSquaredErrors / n);

        return new FittingResult
        {
            ElementType = "Rectangle",
            Width = fittedWidth,
            Height = fittedHeight,
            Angle= angle,
            CenterX = localCenterX,
            CenterY = localCenterY,
            Rmse = (float)rmse,
            PointsCount = n
        };
    } 
}