
using FootballStandings.Csv;
using FootballStandings.Services;

namespace FootballStandings.Tests;

public class HistoricalStandingsIntegrationTests
{
    [Fact]
    public void HistoricalWeek10_ProducesExpectedStandings()
    {
        // Arrange: Locate the historical CSV dataset.
        string inputPath = Path.Combine(
            AppContext.BaseDirectory,
            "TestData",
            "1974-75-week10-results.csv");

        var reader = new CsvMatchReader();
        var calculator = new StandingsCalculator();
        var writer = new CsvStandingsWriter();

        // Act: Execute the complete CSV-to-standings workflow.
        var matches = reader.Read(inputPath);
        var standings = calculator.Calculate(matches);

        using var output = new StringWriter();
        writer.Write(output, standings);

        string csvOutput = output.ToString();

        // Assert: Confirm the historical dataset was processed.
        Assert.Equal(107, matches.Count);
        Assert.Equal(22, standings.Count);

        // Every match contributes two team appearances.
        Assert.Equal(214, standings.Sum(t => t.Played));

        // Validate the leading team and its statistics.
        var ipswich = standings[0];

        Assert.Equal("Ipswich Town", ipswich.TeamName);
        Assert.Equal(10, ipswich.Played);
        Assert.Equal(8, ipswich.Won);
        Assert.Equal(0, ipswich.Drawn);
        Assert.Equal(2, ipswich.Lost);
        Assert.Equal(18, ipswich.GoalsFor);
        Assert.Equal(6, ipswich.GoalsAgainst);
        Assert.Equal(16, ipswich.Points);

        // Confirm important historical positions.
        Assert.Equal("Manchester City", standings[1].TeamName);
        Assert.Equal("Liverpool", standings[2].TeamName);
        Assert.Equal("QPR", standings[21].TeamName);

        // Verify the generated CSV structure and first result.
        var lines = csvOutput.Split(
            '\n',
            StringSplitOptions.RemoveEmptyEntries);

        Assert.Equal(23, lines.Length);

        Assert.Equal(
            "Position,Team,Played,Won,Drawn,Lost," +
            "GoalsFor,GoalsAgainst,GoalAverage,Points",
            lines[0]);

        Assert.Equal(
            "1,Ipswich Town,10,8,0,2,18,6,3.000,16",
            lines[1]);
    }
}
