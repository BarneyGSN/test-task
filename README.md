# Automatyczny Filtr i Generator Obiektów 3D z Chmur Punktów

Projekt dedykowany do automatycznej filtracji chmur punktów (w formatach `.las` oraz `.laz`) oraz automatycznego dopasowywania geometrii struktur masztowych (wsporników oraz rur montażowych). 

System wykorzystuje zaawansowane transformacje macierzowe do przejścia między geodezyjnym układem współrzędnych a lokalnym układem odniesienia elementu konstrukcyjnego. Wycięte sekcje chmury są pasowane algorytmami geometrycznymi, a wyniki eksportowane do modeli trójwymiarowych `.obj` oraz raportów `.json`.

## Wymagania Technologiczne

* **Środowisko uruchomieniowe:** .NET 10.0 (C#)
* **Biblioteki i pakiety NuGet:**
  * `MathNet.Spatial` – operacje na wektorach, macierzach oraz reprezentacja jednorodnych układów współrzędnych (`CoordinateSystem`).
  * `System.CommandLine` – parsowanie argumentów wiersza poleceń z walidacją typów.
  * `Laszip.Net` / Narzędzie `laszip` – automatyczna dekompresja plików LAZ w locie.
  * `xUnit` – framework do automatycznych testów jednostkowych.

---

## Instalacja i Uruchomienie

Przed uruchomieniem upewnij się, że stoisz w terminalu w głównym folderze projektu (tam, gdzie znajduje się plik `.sln` oraz podfoldery `input` i `Results`).

### 1. Przygotowanie danych
Stwórz strukturę folderów i umieść plik chmury w katalogu `input`:
```text
zad_test/
├── input/
│   └── maszt_bez_anten.las  (lub .laz)
└── Results/
```
### 2. Budowanie projektu
```bash
dotnet build
```
### 3. Przykładowe wywołanie
```bash
dotnet run --project zad_test -i "input/maszt_bez_anten.las" -o "Results/wynik" --p1 "7507337.656498;5779803.817752;187.037247" --p2 "7507336.957098;5779803.172852;187.036652" --p3 "7507335.416298,5779804.849152,187.325348" --p4 "7507335.417298;5779804.846352;184.439651" --sw 0.2 --sh 0.2 --r 0.2
```
| Flaga | Pełna nazwa | Opis | Format | Wymagany |
| :--- | :--- | :--- | :--- | :--- |
| `-i` | `--input` | Ścieżka do pliku chmury punktów | `.las` lub `.laz` | **Tak** |
| `-o` | `--output` | Bazowa ścieżka i nazwa plików wynikowych | np. `Results/wynik` | **Tak** |
| | `--p1` | Współrzędne początku (punktu P1) wspornika | `"X;Y;Z"` | **Tak** |
| | `--p2` | Współrzędne końca (punktu P2) wspornika | `"X;Y;Z"` | **Tak** |
| | `--p3` | Współrzędne początku (punktu P3) rury | `"X;Y;Z"` | **Tak** |
| | `--p4` | Współrzędne końca (punktu P4) rury | `"X;Y;Z"` | **Tak** |
| | `--sw` | Szerokość (Width) okna filtracji wspornika | Liczba (np. `0.2`) | **Tak** |
| | `--sh` | Wysokość (Height) okna filtracji wspornika | Liczba (np. `0.2`) | **Tak** |
| `-r` | `--r` | Promień walca filtracji rury montażowej | Liczba (np. `0.2`) | **Tak** |
