
using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using FootballStandings.Models;

namespace FootballStandings.Csv;

public sealed class CsvStandingsWriter
{
    private static readonly string[] Headers =
    {
        "Position",
        "Team",
        "Played",
        "Won",
        "Drawn",
        "Lost",
        "GoalsFor",
        "GoalsAgainst",
        "GoalAverage",
        "Points"
    };

    public void Write(
        string filePath,
        IReadOnlyList<TeamStanding> standings)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        ArgumentNullException.ThrowIfNull(standings);

        using var writer = new StreamWriter(
            filePath,
            false,
            new UTF8Encoding(false));

        Write(writer, standings);
    }

    public void Write(
        TextWriter writer,
        IReadOnlyList<TeamStanding> standings)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(standings);

        var configuration = new CsvConfiguration(
            CultureInfo.InvariantCulture)
        {
            NewLine = "\n"
        };

        using var csv = new CsvWriter(
            writer,
            configuration,
            leaveOpen: true);

        // Write column headers.
        foreach (var header in Headers)
        {
            csv.WriteField(header);
        }

        csv.NextRecord();

        // Write teams in their existing ranked order.
        for (int i = 0; i < standings.Count; i++)
        {
            var team = standings[i];

            csv.WriteField(i + 1);
            csv.WriteField(team.TeamName);
            csv.WriteField(team.Played);
            csv.WriteField(team.Won);
            csv.WriteField(team.Drawn);
            csv.WriteField(team.Lost);
            csv.WriteField(team.GoalsFor);
            csv.WriteField(team.GoalsAgainst);
            csv.WriteField(FormatGoalAverage(team));
            csv.WriteField(team.Points);

            csv.NextRecord();
        }
    }

    private static string FormatGoalAverage(
        TeamStanding team)
    {
        if (team.GoalsAgainst == 0)
        {
            return team.GoalsFor > 0
                ? "INF"
                : "1.000";
        }

        decimal average =
            (decimal)team.GoalsFor / team.GoalsAgainst;

        return average.ToString(
            "F3",
            CultureInfo.InvariantCulture);
    }
}
