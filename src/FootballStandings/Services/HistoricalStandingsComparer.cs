
using FootballStandings.Models;

namespace FootballStandings.Services;

public sealed class HistoricalStandingsComparer
    : IComparer<TeamStanding>
{
    public int Compare(TeamStanding? left, TeamStanding? right)
    {
        if (ReferenceEquals(left, right))
            return 0;

        if (left is null)
            return 1;

        if (right is null)
            return -1;

        // Rule 1: Teams with more points rank higher.
        int pointsComparison =
            right.Points.CompareTo(left.Points);

        if (pointsComparison != 0)
            return pointsComparison;

        // Rule 2: Compare historical goal averages.
        int goalAverageComparison =
            CompareGoalAverage(left, right);

        if (goalAverageComparison != 0)
            return goalAverageComparison;

        // Rule 3: Teams with more goals scored rank higher.
        int goalsComparison =
            right.GoalsFor.CompareTo(left.GoalsFor);

        if (goalsComparison != 0)
            return goalsComparison;

        // Remaining ties require separate handling.
        return 0;
    }

    private static int CompareGoalAverage(
        TeamStanding left,
        TeamStanding right)
    {
        // Positive goals scored, zero goals conceded:
        // Treat goal average as infinity.
        bool leftInfinite =
            left.GoalsAgainst == 0 && left.GoalsFor > 0;

        bool rightInfinite =
            right.GoalsAgainst == 0 && right.GoalsFor > 0;

        if (leftInfinite || rightInfinite)
        {
            if (leftInfinite && rightInfinite)
                return 0;

            return leftInfinite ? -1 : 1;
        }

        // Both teams scored zero and conceded goals.
        // Fewer goals conceded ranks higher.
        bool leftZeroScored =
            left.GoalsFor == 0 && left.GoalsAgainst > 0;

        bool rightZeroScored =
            right.GoalsFor == 0 && right.GoalsAgainst > 0;

        if (leftZeroScored || rightZeroScored)
        {
            if (leftZeroScored && rightZeroScored)
            {
                return left.GoalsAgainst.CompareTo(
                    right.GoalsAgainst);
            }

            return leftZeroScored ? 1 : -1;
        }

        // Treat 0 goals scored and 0 conceded
        // as a goal average of 1.
        long leftFor =
            left.GoalsAgainst == 0 ? 1 : left.GoalsFor;

        long leftAgainst =
            left.GoalsAgainst == 0 ? 1 : left.GoalsAgainst;

        long rightFor =
            right.GoalsAgainst == 0 ? 1 : right.GoalsFor;

        long rightAgainst =
            right.GoalsAgainst == 0 ? 1 : right.GoalsAgainst;

        // Compare fractions by cross multiplication.
        // Using long avoids integer overflow.
        long leftProduct = leftFor * rightAgainst;
        long rightProduct = rightFor * leftAgainst;

        return rightProduct.CompareTo(leftProduct);
    }
}
