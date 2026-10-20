
using FootballStandings.Models;
using FootballStandings.Services;

namespace FootballStandings.Tests;

public class StandingsCalculatorTests
{
    private readonly StandingsCalculator _calculator = new();

    private static MatchResult Match(
        string home,
        string away,
        int homeGoals,
        int awayGoals)
    {
        return new MatchResult(
            new DateOnly(1974, 8, 17),
            home,
            away,
            homeGoals,
            awayGoals);
    }

    [Fact]
    public void Calculate_OneMatch_UpdatesBothTeams()
    {
        var matches = new[]
        {
            Match("Arsenal", "Chelsea", 2, 1)
        };

        var standings = _calculator.Calculate(matches);

        Assert.Equal(2, standings.Count);

        var arsenal = Assert.Single(
            standings, t => t.TeamName == "Arsenal");

        var chelsea = Assert.Single(
            standings, t => t.TeamName == "Chelsea");

        Assert.Equal(1, arsenal.Won);
        Assert.Equal(2, arsenal.Points);
        Assert.Equal(2, arsenal.GoalsFor);
        Assert.Equal(1, arsenal.GoalsAgainst);

        Assert.Equal(1, chelsea.Lost);
        Assert.Equal(0, chelsea.Points);
        Assert.Equal(1, chelsea.GoalsFor);
        Assert.Equal(2, chelsea.GoalsAgainst);
    }

    [Fact]
    public void Calculate_MultipleMatches_AggregatesStatistics()
    {
        var matches = new[]
        {
            Match("Arsenal", "Chelsea", 2, 1),
            Match("Liverpool", "Arsenal", 0, 0),
            Match("Everton", "Arsenal", 3, 1)
        };

        var standings = _calculator.Calculate(matches);

        var arsenal = Assert.Single(
            standings, t => t.TeamName == "Arsenal");

        Assert.Equal(3, arsenal.Played);
        Assert.Equal(1, arsenal.Won);
        Assert.Equal(1, arsenal.Drawn);
        Assert.Equal(1, arsenal.Lost);
        Assert.Equal(3, arsenal.GoalsFor);
        Assert.Equal(4, arsenal.GoalsAgainst);
        Assert.Equal(3, arsenal.Points);
    }

    [Fact]
    public void Calculate_TeamNamesIgnoreCase()
    {
        var matches = new[]
        {
            Match("Arsenal", "Chelsea", 2, 1),
            Match("CHELSEA", "ARSENAL", 0, 1)
        };

        var standings = _calculator.Calculate(matches);

        Assert.Equal(2, standings.Count);

        var arsenal = Assert.Single(
            standings, t => t.TeamName == "Arsenal");

        Assert.Equal(2, arsenal.Played);
        Assert.Equal(2, arsenal.Won);
        Assert.Equal(4, arsenal.Points);
    }

    [Fact]
    public void Calculate_NoMatches_ReturnsEmptyStandings()
    {
        var standings = _calculator.Calculate(
            Array.Empty<MatchResult>());

        Assert.Empty(standings);
    }

    [Fact]
    public void Calculate_NullMatches_ThrowsException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            _calculator.Calculate(null!));
    }
}
