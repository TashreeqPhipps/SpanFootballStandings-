
using CsvHelper;
using FootballStandings.Csv;
using FootballStandings.Services;

// The application requires two command-line arguments:
// 1. Input CSV path
// 2. Output CSV path

if (args.Length != 2)
{
    Console.Error.WriteLine(
        "Usage: FootballStandings <input.csv> <output.csv>");

    return 2;
}

try
{
    string inputPath = Path.GetFullPath(args[0]);
    string outputPath = Path.GetFullPath(args[1]);

    // Prevent accidentally overwriting the input CSV.
    var pathComparison = OperatingSystem.IsWindows()
        ? StringComparison.OrdinalIgnoreCase
        : StringComparison.Ordinal;

    if (string.Equals(
        inputPath,
        outputPath,
        pathComparison))
    {
        Console.Error.WriteLine(
            "Error: Input and output paths must be different.");

        return 2;
    }

    // Step 1: Read match results from the input CSV.
    var csvReader = new CsvMatchReader();
    var matches = csvReader.Read(inputPath);

    if (matches.Count == 0)
    {
        Console.Error.WriteLine(
            "Error: The input CSV contains no match results.");

        return 1;
    }

    // Step 2: Calculate and rank league standings.
    var calculator = new StandingsCalculator();
    var standings = calculator.Calculate(matches);

    // Step 3: Write standings to the output CSV.
    var csvWriter = new CsvStandingsWriter();
    csvWriter.Write(outputPath, standings);

    // Step 4: Report successful completion.
    Console.WriteLine("Standings calculated successfully.");
    Console.WriteLine($"Matches processed: {matches.Count}");
    Console.WriteLine($"Teams ranked: {standings.Count}");
    Console.WriteLine($"Output file: {outputPath}");

    return 0;
}
catch (Exception ex) when (
    ex is IOException or
    UnauthorizedAccessException or
    CsvHelperException or
    ArgumentException)
{
    Console.Error.WriteLine($"Error: {ex.Message}");

    return 1;
}
