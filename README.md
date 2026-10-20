# SpanFootballStandings-
# SPAN Football Standings Calculator

A C#/.NET command-line application developed for the SPAN Digital Associate Software Engineer coding assessment.

The application reads historical football match results from a CSV file, calculates league standings, applies historical ranking rules, and exports the ranked table to another CSV file.

## Technology

- C# and .NET 9
- CsvHelper 33.1.0
- xUnit for automated testing
- Git for version control

## Requirements

Install the .NET 9 SDK on Windows, macOS or Linux.

The application is written using cross-platform .NET APIs. Development and local testing were performed on Windows. Native macOS execution has not yet been verified.

No database, API keys or external services are required to run the application.

## Building the application

From the repository root:

1. Restore dependencies: `dotnet restore`
2. Build the solution: `dotnet build`
3. Run the tests: `dotnet test`

NuGet dependencies are restored automatically.

## Running the application

Run the application with an input CSV path followed by an output CSV path:

`dotnet run --project src/FootballStandings -- data/1974-75-week10-results.csv data/1974-75-week10-standings.csv`

The application reports the number of matches processed, the number of teams ranked and the location of the generated CSV.

### Input format

The required columns are:

`Date,HomeTeam,AwayTeam,HomeGoals,AwayGoals`

Each row represents one completed match. Dates use `yyyy-MM-dd` formatting, and scores must be nonnegative integers.

The CSV reader validates the input, including required headers, malformed results, missing team names and duplicate fixtures.

### Output format

The generated CSV contains:

`Position,Team,Played,Won,Drawn,Lost,GoalsFor,GoalsAgainst,GoalAverage,Points`

The output is sorted according to the implemented historical ranking rules.

## Historical football rules

The 1974/75 English First Division used the following points system:

- Win: 2 points
- Draw: 1 point
- Loss: 0 points

Teams are ranked primarily by points, followed by goal average and goals scored.

Goal average is calculated as goals scored divided by goals conceded, rather than using modern goal difference.

Ratios are compared without rounding. Three-decimal formatting is used only in the exported CSV.

### Edge cases and assumptions

The implementation treats a positive number of goals scored with zero conceded as an infinite goal average. Zero goals scored and zero conceded are represented as an average of 1.

These exceptional cases follow documented implementation conventions rather than independently verified historical Football League regulations.

Remaining ties use a deterministic alphabetical fallback. This is not claimed to be an official historical tie-breaker; historical head-to-head resolution remains an area for final review.

## Historical dataset

The supplied dataset covers English First Division matches played from 17 August through 28 September 1974, the completion of the tenth listed matchweek.

It contains:

- 107 completed matches
- 22 participating teams
- 16 teams with 10 matches played and 6 teams with 9 matches played

Source: [Sports Reference / FBref — 1974/75 First Division Scores & Fixtures](https://fbref.com/en/comps/9/1974-1975/schedule/1974-1975-Premier-League-Scores-and-Fixtures).

The input was converted from the source's fixture and score format into the five-column format expected by the application.

The generated standings were also cross-checked against historical records. One reference differs in its ordering of Arsenal and Tottenham; the implementation places Arsenal above Tottenham on goal average. This discrepancy is retained as a documented verification note.

## Project structure

- `src/FootballStandings/Models/` — match results and team standing models
- `src/FootballStandings/Services/` — standings aggregation and historical ranking
- `src/FootballStandings/Csv/` — CSV reading and writing
- `src/FootballStandings/Program.cs` — command-line entry point and application coordination
- `tests/FootballStandings.Tests/` — unit and integration tests
- `data/` — historical results, expected output and illustrative sample files
- `ai/` — AI collaboration records, to be finalized

## Testing

Run all tests with:

`dotnet test`

At the latest verified development milestone, 61 tests passed on Windows, including a historical integration test.

The tests cover match validation, statistics calculations, historical ranking, CSV handling, error cases and processing the supplied 107-match dataset.

## AI collaboration

ChatGPT was used during requirements analysis, implementation, testing, debugging and technical review.

Genuine collaboration evidence and the developer's reflection will be supplied separately as required by the assessment. No Claude Code session data is claimed to have been generated.

## Known limitations and final verification

Before submission, the project requires final review of historical tie-breaking assumptions, additional command-line failure testing, confirmation of the AI artifact equivalence requirements and cross-platform execution validation.
