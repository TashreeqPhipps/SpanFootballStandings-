
namespace FootballStandings.Models;

public sealed record MatchResult
{
    public DateOnly Date { get; }
    public string HomeTeam { get; }
    public string AwayTeam { get; }
    public int HomeGoals { get; }
    public int AwayGoals { get; }

    public MatchResult(
        DateOnly date,
        string homeTeam,
        string awayTeam,
        int homeGoals,
        int awayGoals)
    {
        if (date == default)
            throw new ArgumentException(
                "A match date is required.", nameof(date));

        if (string.IsNullOrWhiteSpace(homeTeam))
            throw new ArgumentException(
                "Home team is required.", nameof(homeTeam));

        if (string.IsNullOrWhiteSpace(awayTeam))
            throw new ArgumentException(
                "Away team is required.", nameof(awayTeam));

        if (homeGoals < 0)
            throw new ArgumentOutOfRangeException(
                nameof(homeGoals), "Goals cannot be negative.");

        if (awayGoals < 0)
            throw new ArgumentOutOfRangeException(
                nameof(awayGoals), "Goals cannot be negative.");

        homeTeam = homeTeam.Trim();
        awayTeam = awayTeam.Trim();

        if (string.Equals(
            homeTeam,
            awayTeam,
            StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException(
                "A team cannot play against itself.",
                nameof(awayTeam));
        }

        Date = date;
        HomeTeam = homeTeam;
        AwayTeam = awayTeam;
        HomeGoals = homeGoals;
        AwayGoals = awayGoals;
    }
}
