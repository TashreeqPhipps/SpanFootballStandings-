
namespace FootballStandings.Models;

public sealed class TeamStanding
{
    public string TeamName { get; }

    public int Won { get; private set; }
    public int Drawn { get; private set; }
    public int Lost { get; private set; }

    public int GoalsFor { get; private set; }
    public int GoalsAgainst { get; private set; }

    public int Played => Won + Drawn + Lost;

    public int Points => (Won * 2) + Drawn;

    public TeamStanding(string teamName)
    {
        if (string.IsNullOrWhiteSpace(teamName))
        {
            throw new ArgumentException(
                "Team name cannot be empty.",
                nameof(teamName));
        }

        TeamName = teamName.Trim();
    }

    public void RecordResult(int goalsFor, int goalsAgainst)
    {
        if (goalsFor < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(goalsFor),
                "Goals cannot be negative.");
        }

        if (goalsAgainst < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(goalsAgainst),
                "Goals cannot be negative.");
        }

        GoalsFor += goalsFor;
        GoalsAgainst += goalsAgainst;

        if (goalsFor > goalsAgainst)
        {
            Won++;
        }
        else if (goalsFor == goalsAgainst)
        {
            Drawn++;
        }
        else
        {
            Lost++;
        }
    }
}
