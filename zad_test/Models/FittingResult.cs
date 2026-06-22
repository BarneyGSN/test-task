namespace zad_test.Models;

public class FittingResult
{
    public string ElementType { get; set; } = string.Empty;
    public float CalculatedLength { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }
    public float Radius { get; set; }
    public float OrientationDegrees { get; set; }
    public float Rmse { get; set; }
}