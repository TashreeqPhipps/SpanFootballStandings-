
using FootballStandings.Models;

namespace FootballStandings.Services;

public sealed class StandingsCalculator
{
    public IReadOnlyList<TeamStanding> Calculate(
        IEnumerable<MatchResult> matches)
    {
        ArgumentNullException.ThrowIfNull(matches);

        var standings = new Dictionary<string, TeamStanding>(
            StringComparer.OrdinalIgnoreCase);

        foreach (var match in matches)
        {
            ArgumentNullException.ThrowIfNull(match);

            // Find or create the home team's standing.
            var homeStanding = GetOrCreate(
                standings, match.HomeTeam);

            // Find or create the away team's standing.
            var awayStanding = GetOrCreate(
                standings, match.AwayTeam);

            // Record the result for the home team.
            homeStanding.RecordResult(
                match.HomeGoals, match.AwayGoals);

            // Record the reversed result for the away team.
            awayStanding.RecordResult(
                match.AwayGoals, match.HomeGoals);
        }

        // Sort teams using the historical league ranking:
        // 1. Points (highest first)
        // 2. Goal average (highest first)
        // 3. Goals scored (highest first)
        // 4. Alphabetical fallback for unresolved ties
        return standings.Values
            .OrderBy(
                team => team,
                new HistoricalStandingsComparer())
            .ThenBy(
                team => team.TeamName,
                StringComparer.Ordinal)
            .ToList();
    }

    private static TeamStanding GetOrCreate(
        Dictionary<string, TeamStanding> standings,
        string teamName)
    {
        if (!standings.TryGetValue(teamName, out var standing))
        {
            standing = new TeamStanding(teamName);
            standings.Add(teamName, standing);
        }

        return standing;
    }
}
