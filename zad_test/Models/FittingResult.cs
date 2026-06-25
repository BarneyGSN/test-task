namespace zad_test.Models;

public class FittingResult
{
    public string ElementType { get; set; } = string.Empty;
    //public float Length { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public double Radius { get; set; }
    public double Angle { get; set; }
    public double CenterX { get; set; }
    public double CenterY { get; set; }
    public double Rmse { get; set; }
    public double PointsCount { get; set; }
    
}