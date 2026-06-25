using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Parsing;
using MathNet.Spatial.Euclidean;
using zad_test.Fitting;
using zad_test.Geometry;
using zad_test.IO;
using zad_test.Models;

namespace zad_test;

class Program
{
    static async Task<int> Main(string[] args)
    {
        var inputOption = new Option<FileInfo>("--input")
        {
            Aliases = { "-i" },
            Description = "Input file path.",
            Required =  true
        };

        var outputOption = new Option<FileInfo>("--output")
        {
            Aliases = { "-o" },
            Description = "Output path for JSON and OBJ files.",
            Required = true
        };

        var P1Option = new Option<Vector3D>("--p1")
        {
            Description = "Współrzędne punktu P1 w formacie X, Y, Z",
            Required = true,
            HelpName = "X,Y,Z",
            CustomParser = result =>
            {
                string text = result.Tokens[0].Value;
                string[] parts = text.Split(',', ';');

                if (parts.Length != 3)
                {
                    result.AddError("Punkt musi mieć dokładnie 3 współrzędne oddzielone przecinkiem lub średnikiem");
                    return default(Vector3D);
                }

                double x = double.Parse(parts[0], System.Globalization.CultureInfo.InvariantCulture);
                double y = double.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture);
                double z = double.Parse(parts[2], System.Globalization.CultureInfo.InvariantCulture);

                return new Vector3D(x, y, z);
            }
        };
       
        var P2Option = new Option<Vector3D>("--p2")
        {
            Description = "Współrzędne punktu P2 w formacie X, Y, Z",
            Required = true,
            HelpName = "X,Y,Z",
            CustomParser = result =>
            {
                string text = result.Tokens[0].Value;
                string[] parts = text.Split(',', ';');

                if (parts.Length != 3)
                {
                    result.AddError("Punkt musi mieć dokładnie 3 współrzędne oddzielone przecinkiem lub średnikiem");
                    return default(Vector3D);
                }

                double x = double.Parse(parts[0], System.Globalization.CultureInfo.InvariantCulture);
                double y = double.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture);
                double z = double.Parse(parts[2], System.Globalization.CultureInfo.InvariantCulture);

                return new Vector3D(x, y, z);
            }
        };
        
        var P3Option = new Option<Vector3D>("--p3")
        {
            Description = "Współrzędne punktu P3 w formacie X, Y, Z",
            Required = true,
            HelpName = "X,Y,Z",
            CustomParser = result =>
            {
                string text = result.Tokens[0].Value;
                string[] parts = text.Split(',', ';');

                if (parts.Length != 3)
                {
                    result.AddError("Punkt musi mieć dokładnie 3 współrzędne oddzielone przecinkiem lub średnikiem");
                    return default(Vector3D);
                }

                double x = double.Parse(parts[0], System.Globalization.CultureInfo.InvariantCulture);
                double y = double.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture);
                double z = double.Parse(parts[2], System.Globalization.CultureInfo.InvariantCulture);

                return new Vector3D(x, y, z);
            }
        };
        
        var P4Option = new Option<Vector3D>("--p4")
        {
            Description = "Współrzędne punktu P4 w formacie X, Y, Z",
            Required = true,
            HelpName = "X,Y,Z",
            CustomParser = result =>
            {
                string text = result.Tokens[0].Value;
                string[] parts = text.Split(',', ';');

                if (parts.Length != 3)
                {
                    result.AddError("Punkt musi mieć dokładnie 3 współrzędne oddzielone przecinkiem lub średnikiem");
                    return default(Vector3D);
                }

                double x = double.Parse(parts[0], System.Globalization.CultureInfo.InvariantCulture);
                double y = double.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture);
                double z = double.Parse(parts[2], System.Globalization.CultureInfo.InvariantCulture);

                return new Vector3D(x, y, z);
            }
        };

        var swOption = new Option<double>("--sw")
        {
            Description = "Szerokość obszaru zainteresowania",
            Required = true,
        };
        
        var shOption = new Option<double>("--sh")
        {
            Description = "Wysokość obszaru zainteresowania",
            Required = true,
        };

        var rOption = new Option<double>("--r")
        {
            Description = "Promień obszaru zainteresowania",
            Required = true,
        };
        
        var rootCommand = new RootCommand("Aplikacja do automatycznego generowania i dopasowania wsporników masztu oraz rur montażowych.");
        
        rootCommand.Add(inputOption);
        rootCommand.Add(outputOption);
        rootCommand.Add(P1Option);
        rootCommand.Add(P2Option);
        rootCommand.Add(P3Option);
        rootCommand.Add(P4Option);
        rootCommand.Add(swOption);
        rootCommand.Add(shOption);
        rootCommand.Add(rOption);
        
        var parseResult = rootCommand.Parse(args);

        if (parseResult.Errors.Count > 0)
        {
            foreach (var error in parseResult.Errors)
            {
                Console.WriteLine($"Błąd CLI: {error.Message}");
            }
            return 1;
        }

        Console.WriteLine("ROZPOCZĘCIE PRZETWARZANIA CHMURY PUNKTÓW");
        
        FileInfo input = parseResult.GetValue(inputOption);
        FileInfo output = parseResult.GetValue(outputOption);
        Vector3D v1 = parseResult.GetValue(P1Option);
        Vector3D v2 = parseResult.GetValue(P2Option);
        Vector3D v3 = parseResult.GetValue(P3Option);
        Vector3D v4 = parseResult.GetValue(P4Option);
        double sw = parseResult.GetValue(swOption);
        double sh = parseResult.GetValue(shOption);
        double r = parseResult.GetValue(rOption);
        
        string initialInputPath = input.FullName;
        string lasFilePath = initialInputPath;
        bool isTempFile = false;

        string baseDir = output.DirectoryName;
        string baseName = Path.GetFileNameWithoutExtension(output.Name);
        string supportObjPath = Path.Combine(baseDir, $"{baseName}_wspornik.obj");
        string supportJsonPath = Path.Combine(baseDir, $"{baseName}_wspornik.json");
        string cylinderObjPath = Path.Combine(baseDir, $"{baseName}_rura.obj");
        string cylinderJsonPath = Path.Combine(baseDir, $"{baseName}_rura.json");
        
        Point3D p1Global = new Point3D(v1.X, v1.Y, v1.Z); 
        Point3D p2Global = new Point3D(v2.X, v2.Y, v2.Z);
        
        double searchWidth = sw;   
        double searchHeight = sh;  

        try
        {
            lasFilePath = DecompressLaz(initialInputPath, out isTempFile);

            Console.WriteLine("Wczytywanie punktów z pliku LAS");
            var lasReader = new LasReader();
            IEnumerable<Point3D> rawCloudPoints = lasReader.ReadPoints(lasFilePath);
            
            var transformer = new CoordinateTransformer();
            CoordinateSystem localSystem = transformer.globalToLocal(p1Global, p2Global, out double supportLength);

            Console.WriteLine($"\nZbudowano lokalny układ współrzędnych");
            
            Console.WriteLine("\nFiltracja punktów i wycinanie przekroju poprzecznego");
            var supportFilter = new CloudFilter();
            ExtractionResult supportExtractionResult = supportFilter.supportFilteredAndSectionPoints(
                rawCloudPoints, 
                localSystem, 
                searchWidth, 
                searchHeight, 
                supportLength
            );
            
            Console.WriteLine($"Punkty wewnątrz wspornika (Filtered): {supportExtractionResult.FilteredPoints.Count}");
            Console.WriteLine($"Punkty w plastrze przekroju (Section):  {supportExtractionResult.SectionPoints.Count}");

            if (supportExtractionResult.FilteredPoints.Count == 0)
            {
                Console.WriteLine("\nLiczba punktów wynosi 0. Sprawdź poprawność zasięgu lub pliku LAS.");
            }
            
            Console.WriteLine("\nUruchamianie algorytmu dopasowania prostokąta");
            var supportFitter = new RectangleFitting();
            FittingResult supportFittingResult = supportFitter.FitRectangle(supportExtractionResult.SectionPoints);
            
            Console.WriteLine("Generowanie modelu 3D wspornika");
            ModelGenerator.GenerateModel(supportFittingResult, transformer.LocalToGlobal(localSystem), supportLength, supportObjPath);
            Console.WriteLine($"Zapisano model OBJ wspornika: {supportObjPath}");
            
            Console.WriteLine("Zapisywanie parametrów geometrii do pliku JSON");
            var supportExporter = new ResultExporter();
            supportExporter.ExportToJson(supportFittingResult, supportJsonPath);
            Console.WriteLine($"Zapisano plik JSON rury montażowej: {supportJsonPath}");
            
            Console.WriteLine("\n URUCHAMIANIE WERSJI ROZSZERZONEJ: GENEROWANIE RURY MONTAŻOWEJ");
            
            Point3D p3Global = new Point3D(v3.X, v3.Y, v3.Z);
            Point3D p4Global = new Point3D(v4.X, v4.Y, v4.Z);
            CoordinateSystem localSystemCylinder = transformer.globalToLocal(p3Global, p4Global, out double cylinderLength);

            var cylinderFilter = new CloudFilter();
            ExtractionResult cylinderExtractionResult = cylinderFilter.cylinderFilteredAndSectionPoints(
                rawCloudPoints,
                localSystemCylinder,
                r,
                cylinderLength
            );
            Console.WriteLine($"Punkty wewnątrz rury montażowej (Filtered): {cylinderExtractionResult.FilteredPoints.Count}");
            Console.WriteLine($"Punkty w plastrze rury montażowej (Section):  {cylinderExtractionResult.SectionPoints.Count}");
            
            if (cylinderExtractionResult.FilteredPoints.Count == 0)
            {
                Console.WriteLine("\nLiczba punktów wynosi 0. Sprawdź poprawność zasięgu lub pliku LAS.");
            }
            
            var cylinderFitter = new CylinderFitting();
            FittingResult cylinderFittingResult = cylinderFitter.FitCylinder(cylinderExtractionResult.SectionPoints);
            
            Console.WriteLine("Generowanie modelu 3D rury montażowej");
            ModelGenerator.GenerateCylinderModel(cylinderFittingResult, transformer.LocalToGlobal(localSystemCylinder), cylinderLength, cylinderObjPath);
            Console.WriteLine($"Zapisano model OBJ rury montażowej: {cylinderObjPath}");
            
            Console.WriteLine("Zapisywanie parametrów geometrii do pliku JSON");
            var exporter = new ResultExporter();
            exporter.ExportToJson(cylinderFittingResult, cylinderJsonPath);
            Console.WriteLine($"Zapisano plik JSON rury montażowej: {cylinderJsonPath}");
            
            Console.WriteLine("\n PRZETWARZANIE ZAKOŃCZONE SUKCESEM");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n Wystąpił błąd podczas przetwarzania: {ex.Message}");
            return 1;
        }
        finally
        {
            if (isTempFile && File.Exists(lasFilePath))
            {
                try
                {
                    File.Delete(lasFilePath);
                    Console.WriteLine("\nUsunięto tymczasowy plik LAS.");
                }
                catch (IOException ioEx)
                {
                    Console.WriteLine($"\nNie udało się usunąć pliku tymczasowego: {ioEx.Message}");
                }
            }
        }

        return 0;
    }
    private static string DecompressLaz(string inputFilePath, out bool isTemporary)
    {
        string extension = Path.GetExtension(inputFilePath).ToLower();

        if (extension == ".las")
        {
            isTemporary = false;
            return inputFilePath;
        }

        if (extension == ".laz")
        {
            string tempLasPath = Path.ChangeExtension(inputFilePath, $"temp.las");
            isTemporary = true;

            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "laszip", 
                Arguments = $"-i \"{inputFilePath}\" -o \"{tempLasPath}\"",
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            using (var process = System.Diagnostics.Process.Start(startInfo))
            {
                if (process == null)
                {
                    throw new Exception("Nie udało się uruchomić narzędzia laszip. Upewnij się, że jest zainstalowane w systemie.");
                }

                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    string error = process.StandardError.ReadToEnd();
                    throw new Exception($"Błąd dekompresji LAZ przez laszip: {error}");
                }
            }

            Console.WriteLine($"Plik pomyślnie rozpakowany tymczasowo do: {Path.GetFileName(tempLasPath)}");
            return tempLasPath;
        }

        throw new ArgumentException($"Niewłaściwy format pliku: {extension}. Dozwolone formaty to .las lub .laz");
    }
}