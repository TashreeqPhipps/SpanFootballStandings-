
using FootballStandings.Models;

namespace FootballStandings.Tests;

public class TeamStandingTests
{
    [Fact]
    public void NewTeam_HasZeroStatistics()
    {
        var team = new TeamStanding("Arsenal");

        Assert.Equal("Arsenal", team.TeamName);
        Assert.Equal(0, team.Played);
        Assert.Equal(0, team.Won);
        Assert.Equal(0, team.Drawn);
        Assert.Equal(0, team.Lost);
        Assert.Equal(0, team.GoalsFor);
        Assert.Equal(0, team.GoalsAgainst);
        Assert.Equal(0, team.Points);
    }

    [Fact]
    public void RecordResult_Win_AwardsTwoPoints()
    {
        var team = new TeamStanding("Arsenal");

        team.RecordResult(2, 1);

        Assert.Equal(1, team.Played);
        Assert.Equal(1, team.Won);
        Assert.Equal(2, team.Points);
        Assert.Equal(2, team.GoalsFor);
        Assert.Equal(1, team.GoalsAgainst);
    }

    [Fact]
    public void RecordResult_Draw_AwardsOnePoint()
    {
        var team = new TeamStanding("Arsenal");

        team.RecordResult(0, 0);

        Assert.Equal(1, team.Played);
        Assert.Equal(1, team.Drawn);
        Assert.Equal(1, team.Points);
    }

    [Fact]
    public void RecordResult_Loss_AwardsZeroPoints()
    {
        var team = new TeamStanding("Arsenal");

        team.RecordResult(1, 3);

        Assert.Equal(1, team.Played);
        Assert.Equal(1, team.Lost);
        Assert.Equal(0, team.Points);
    }

    [Fact]
    public void RecordResult_MultipleMatches_AccumulatesStatistics()
    {
        var team = new TeamStanding("Arsenal");

        team.RecordResult(2, 1);
        team.RecordResult(0, 0);
        team.RecordResult(1, 3);

        Assert.Equal(3, team.Played);
        Assert.Equal(1, team.Won);
        Assert.Equal(1, team.Drawn);
        Assert.Equal(1, team.Lost);
        Assert.Equal(3, team.GoalsFor);
        Assert.Equal(4, team.GoalsAgainst);
        Assert.Equal(3, team.Points);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    public void RecordResult_NegativeGoals_ThrowsException(
        int goalsFor, int goalsAgainst)
    {
        var team = new TeamStanding("Arsenal");

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            team.RecordResult(goalsFor, goalsAgainst));

        Assert.Equal(0, team.Played);
        Assert.Equal(0, team.GoalsFor);
        Assert.Equal(0, team.GoalsAgainst);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_InvalidTeamName_ThrowsException(
        string teamName)
    {
        Assert.Throws<ArgumentException>(() =>
            new TeamStanding(teamName));
    }

    [Fact]
    public void Constructor_TrimsTeamName()
    {
        var team = new TeamStanding(" Arsenal ");

        Assert.Equal("Arsenal", team.TeamName);
    }
}
