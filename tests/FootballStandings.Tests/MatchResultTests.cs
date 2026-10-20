
using FootballStandings.Models;

namespace FootballStandings.Tests;

public class MatchResultTests
{
    [Fact]
    public void Constructor_WithValidData_CreatesMatch()
    {
        var date = new DateOnly(1974, 8, 17);

        var match = new MatchResult(
            date, "Arsenal", "Chelsea", 2, 1);

        Assert.Equal(date, match.Date);
        Assert.Equal("Arsenal", match.HomeTeam);
        Assert.Equal("Chelsea", match.AwayTeam);
        Assert.Equal(2, match.HomeGoals);
        Assert.Equal(1, match.AwayGoals);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    public void Constructor_WithNegativeGoals_ThrowsException(
        int homeGoals, int awayGoals)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new MatchResult(
                new DateOnly(1974, 8, 17),
                "Arsenal",
                "Chelsea",
                homeGoals,
                awayGoals));
    }

    [Theory]
    [InlineData("", "Chelsea")]
    [InlineData("Arsenal", "   ")]
    public void Constructor_WithEmptyTeamName_ThrowsException(
        string homeTeam, string awayTeam)
    {
        Assert.Throws<ArgumentException>(() =>
            new MatchResult(
                new DateOnly(1974, 8, 17),
                homeTeam,
                awayTeam,
                2,
                1));
    }

    [Fact]
    public void Constructor_WithSameTeam_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() =>
            new MatchResult(
                new DateOnly(1974, 8, 17),
                " Arsenal ",
                "arsenal",
                2,
                1));
    }

    [Fact]
    public void Constructor_TrimsTeamNames()
    {
        var match = new MatchResult(
            new DateOnly(1974, 8, 17),
            " Arsenal ",
            " Chelsea ",
            2,
            1);

        Assert.Equal("Arsenal", match.HomeTeam);
        Assert.Equal("Chelsea", match.AwayTeam);
    }
}
