
using FootballStandings.Csv;
using FootballStandings.Models;

namespace FootballStandings.Tests;

public class CsvStandingsWriterTests
{
    private readonly CsvStandingsWriter _writer = new();

    private const string Header =
        "Position,Team,Played,Won,Drawn,Lost," +
        "GoalsFor,GoalsAgainst,GoalAverage,Points\n";

    private static TeamStanding CreateTeam(
        string name,
        params (int GoalsFor, int GoalsAgainst)[] results)
    {
        var team = new TeamStanding(name);

        foreach (var (goalsFor, goalsAgainst) in results)
        {
            team.RecordResult(goalsFor, goalsAgainst);
        }

        return team;
    }

    private string WriteToString(
        params TeamStanding[] standings)
    {
        using var output = new StringWriter();

        _writer.Write(output, standings);

        return output.ToString();
    }

    [Fact]
    public void Write_ValidStandings_ProducesCorrectCsv()
    {
        var arsenal = CreateTeam(
            "Arsenal", (2, 1), (0, 0));

        var chelsea = CreateTeam(
            "Chelsea", (1, 2), (0, 0));

        string csv = WriteToString(arsenal, chelsea);

        string expected = Header +
            "1,Arsenal,2,1,1,0,2,1,2.000,3\n" +
            "2,Chelsea,2,0,1,1,1,2,0.500,1\n";

        Assert.Equal(expected, csv);
    }

    [Fact]
    public void Write_TeamNameWithComma_IsQuoted()
    {
        var team = CreateTeam(
            "Town, United", (0, 0));

        string csv = WriteToString(team);

        Assert.Contains(
            "1,\"Town, United\",1,0,1,0,0,0,1.000,1",
            csv);
    }

    [Fact]
    public void Write_ZeroGoalsAgainst_UsesInfinity()
    {
        var team = CreateTeam(
            "Arsenal", (2, 0));

        string csv = WriteToString(team);

        Assert.Contains(
            "1,Arsenal,1,1,0,0,2,0,INF,2",
            csv);
    }

    [Fact]
    public void Write_ZeroZeroGoals_UsesAverageOfOne()
    {
        var team = CreateTeam(
            "Arsenal", (0, 0));

        string csv = WriteToString(team);

        Assert.Contains(
            "1,Arsenal,1,0,1,0,0,0,1.000,1",
            csv);
    }

    [Fact]
    public void Write_EmptyStandings_WritesHeaderOnly()
    {
        string csv = WriteToString();

        Assert.Equal(Header, csv);
    }

    [Fact]
    public void Write_FilePath_CreatesCsvFile()
    {
        string path = Path.Combine(
            Path.GetTempPath(),
            $"standings-{Guid.NewGuid():N}.csv");

        try
        {
            var team = CreateTeam(
                "Arsenal", (2, 1));

            _writer.Write(path, new[] { team });

            Assert.True(File.Exists(path));

            string content = File.ReadAllText(path);

            Assert.Equal(
                Header + "1,Arsenal,1,1,0,0,2,1,2.000,2\n",
                content);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    [Fact]
    public void Write_NullStandings_ThrowsException()
    {
        using var output = new StringWriter();

        Assert.Throws<ArgumentNullException>(() =>
            _writer.Write(output, null!));
    }

    [Fact]
    public void Write_NullTextWriter_ThrowsException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            _writer.Write(
                (TextWriter)null!,
                Array.Empty<TeamStanding>()));
    }

    [Fact]
    public void Write_EmptyFilePath_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() =>
            _writer.Write(
                " ",
                Array.Empty<TeamStanding>()));
    }
}
