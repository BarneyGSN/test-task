namespace zad_test.IO;
using zad_test.Models;
using System.Text.Json;
public class ResultExporter
{
    public void ExportToJson(FittingResult fittingResult, string OutputPath)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        
        string outputJson = JsonSerializer.Serialize(fittingResult, options);
        File.WriteAllText(OutputPath, outputJson);
    }
}