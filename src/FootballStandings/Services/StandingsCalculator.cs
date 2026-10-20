
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

            var homeStanding = GetOrCreate(
                standings, match.HomeTeam);

            var awayStanding = GetOrCreate(
                standings, match.AwayTeam);

            homeStanding.RecordResult(
                match.HomeGoals, match.AwayGoals);

            awayStanding.RecordResult(
                match.AwayGoals, match.HomeGoals);
        }

        return standings.Values
            .OrderBy(team => team.TeamName, StringComparer.Ordinal)
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
