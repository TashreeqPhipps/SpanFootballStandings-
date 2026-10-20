
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using FootballStandings.Models;

namespace FootballStandings.Csv;

public sealed class CsvMatchReader
{
    private static readonly string[] RequiredHeaders =
    {
        "Date",
        "HomeTeam",
        "AwayTeam",
        "HomeGoals",
        "AwayGoals"
    };

    public IReadOnlyList<MatchResult> Read(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        using var reader = new StreamReader(filePath);
        return Read(reader);
    }

    public IReadOnlyList<MatchResult> Read(TextReader reader)
    {
        ArgumentNullException.ThrowIfNull(reader);

        var configuration = new CsvConfiguration(
            CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true
        };

        using var csv = new CsvReader(reader, configuration);

        if (!csv.Read())
        {
            throw new InvalidDataException(
                "The CSV file is empty.");
        }

        csv.ReadHeader();

        var headers = csv.HeaderRecord ?? Array.Empty<string>();

        if (headers.Length != RequiredHeaders.Length ||
            RequiredHeaders.Any(header =>
                !headers.Contains(header, StringComparer.Ordinal)))
        {
            throw new InvalidDataException(
                "Invalid CSV headers. Expected: " +
                string.Join(", ", RequiredHeaders));
        }

        var matches = new List<MatchResult>();

        var seenFixtures =
            new HashSet<(DateOnly, string, string)>();

        while (csv.Read())
        {
            int row = csv.Parser.RawRow;

            if (csv.Parser.Count != RequiredHeaders.Length)
            {
                throw new InvalidDataException(
                    $"CSV row {row}: Expected five fields.");
            }

            string dateText = csv.GetField("Date")!.Trim();

            if (!DateOnly.TryParseExact(
                dateText,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var date))
            {
                throw new InvalidDataException(
                    $"CSV row {row}: Invalid match date.");
            }

            if (!int.TryParse(
                csv.GetField("HomeGoals")?.Trim(),
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out int homeGoals))
            {
                throw new InvalidDataException(
                    $"CSV row {row}: Invalid home goals.");
            }

            if (!int.TryParse(
                csv.GetField("AwayGoals")?.Trim(),
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out int awayGoals))
            {
                throw new InvalidDataException(
                    $"CSV row {row}: Invalid away goals.");
            }

            MatchResult match;

            try
            {
                match = new MatchResult(
                    date,
                    csv.GetField("HomeTeam")!,
                    csv.GetField("AwayTeam")!,
                    homeGoals,
                    awayGoals);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidDataException(
                    $"CSV row {row}: {ex.Message}", ex);
            }

            var fixtureKey = (
                match.Date,
                match.HomeTeam.ToUpperInvariant(),
                match.AwayTeam.ToUpperInvariant());

            if (!seenFixtures.Add(fixtureKey))
            {
                throw new InvalidDataException(
                    $"CSV row {row}: Duplicate fixture.");
            }

            matches.Add(match);
        }

        return matches;
    }
}
