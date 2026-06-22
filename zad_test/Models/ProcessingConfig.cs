namespace zad_test.Models;
using System.Numerics;

public class ProcessingConfig
{
    public string FilePath {get; set;} = string.Empty;
    
    public Vector3 P1 {get; set;}
    public Vector3 P2 {get; set;}
    public Vector3 P3 {get; set;}
    public Vector3 P4 {get; set;}
    
    public float SupportWidth {get; set;}
    public float SupportHeight {get; set;}
    public float PipeRadius {get; set;}
}