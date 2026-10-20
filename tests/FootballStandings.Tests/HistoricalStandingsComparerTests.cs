
using FootballStandings.Models;
using FootballStandings.Services;

namespace FootballStandings.Tests;

public class HistoricalStandingsComparerTests
{
    private readonly HistoricalStandingsComparer _comparer = new();

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

    [Fact]
    public void Compare_HigherPoints_RanksFirst()
    {
        var arsenal = CreateTeam(
            "Arsenal", (1, 0), (0, 4));

        var chelsea = CreateTeam(
            "Chelsea", (0, 0));

        Assert.True(
            _comparer.Compare(arsenal, chelsea) < 0);
    }

    [Fact]
    public void Compare_EqualPoints_UsesGoalAverage()
    {
        var arsenal = CreateTeam(
            "Arsenal", (3, 2));

        var chelsea = CreateTeam(
            "Chelsea", (7, 5));

        Assert.True(
            _comparer.Compare(arsenal, chelsea) < 0);
    }

    [Fact]
    public void Compare_EqualGoalAverage_UsesGoalsScored()
    {
        var arsenal = CreateTeam(
            "Arsenal", (4, 2));

        var chelsea = CreateTeam(
            "Chelsea", (2, 1));

        Assert.True(
            _comparer.Compare(arsenal, chelsea) < 0);
    }

    [Fact]
    public void Compare_UsesExactRatiosWithoutRounding()
    {
        // Synthetic values chosen to test precision.
        // Both averages round to 1.001.
        var higherAverage = CreateTeam(
            "Higher Average", (1000, 999));

        var lowerAverage = CreateTeam(
            "Lower Average", (1001, 1000));

        Assert.True(
            _comparer.Compare(higherAverage, lowerAverage) < 0);
    }

    [Fact]
    public void Compare_ZeroConceded_RanksAsInfinity()
    {
        var arsenal = CreateTeam(
            "Arsenal", (1, 0));

        var chelsea = CreateTeam(
            "Chelsea", (10, 1));

        Assert.True(
            _comparer.Compare(arsenal, chelsea) < 0);
    }

    [Fact]
    public void Compare_ZeroZero_UsesRatioOfOne()
    {
        var arsenal = CreateTeam(
            "Arsenal", (0, 0));

        var chelsea = CreateTeam(
            "Chelsea", (1, 1));

        // Both have a goal average of 1.
        // Chelsea ranks higher on goals scored.
        Assert.True(
            _comparer.Compare(chelsea, arsenal) < 0);
    }

    [Fact]
    public void Compare_ZeroGoalsScored_FavoursFewerConceded()
    {
        var arsenal = CreateTeam(
            "Arsenal", (0, 1));

        var chelsea = CreateTeam(
            "Chelsea", (0, 3));

        Assert.True(
            _comparer.Compare(arsenal, chelsea) < 0);
    }

    [Fact]
    public void Compare_IdenticalStatistics_ReturnsZero()
    {
        var arsenal = CreateTeam(
            "Arsenal", (2, 1));

        var chelsea = CreateTeam(
            "Chelsea", (2, 1));

        Assert.Equal(
            0, _comparer.Compare(arsenal, chelsea));
    }

    [Fact]
    public void Compare_CanSortTeamsByHistoricalRules()
    {
        var arsenal = CreateTeam(
            "Arsenal", (1, 0));

        var chelsea = CreateTeam(
            "Chelsea", (1, 1));

        var everton = CreateTeam(
            "Everton", (0, 1));

        var standings = new List<TeamStanding>
        {
            everton,
            chelsea,
            arsenal
        };

        standings.Sort(_comparer);

        Assert.Equal(
            new[] { "Arsenal", "Chelsea", "Everton" },
            standings.Select(team => team.TeamName));
    }
}
