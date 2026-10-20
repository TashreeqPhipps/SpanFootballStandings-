# SPAN Football Standings Calculator

A C#/.NET command-line application developed for the SPAN Digital Associate Software Engineer coding assessment.

The application reads historical football match results from a CSV file, calculates league standings using the historical ranking rules, and exports the completed league table to another CSV file.

The primary objective is to calculate the English First Division standings at the end of week 10 of the 1974/75 season.

## Technology Stack

- **Language:** C#
- **Framework:** .NET 9
- **CSV Processing:** CsvHelper 33.1.0
- **Testing:** xUnit
- **Version Control:** Git and GitHub
- **Development Environments:** Windows and macOS
- **IDE:** Visual Studio Code

## Requirements

The application requires the .NET 9 SDK.

Download: https://dotnet.microsoft.com/en-us/download/dotnet/9.0

Verify the installation using:

`dotnet --version`

The application is cross-platform and has been successfully built and tested on both Windows and macOS.

No database, API keys, external services or additional application servers are required to run the application. NuGet packages are restored during the build process.

## Getting Started

### 1. Clone the Repository

Open a terminal and run:

`git clone https://github.com/TashreeqPhipps/SpanFootballStandings-.git`

Navigate into the project:

`cd SpanFootballStandings-`

All subsequent commands should be executed from the repository's root directory.

### 2. Restore Dependencies

`dotnet restore`

This downloads the required NuGet dependencies.

### 3. Build the Application

`dotnet build -c Release`

The application and test project should build successfully without errors.

### 4. Run Automated Tests

`dotnet test -c Release`

**Latest verified result:**

- Total tests: 61
- Passed: 61
- Failed: 0
- Skipped: 0

All 61 tests passed on both Windows and macOS during development.

## Running the Football Standings Application

The application accepts two command-line arguments:

1. The path to the input CSV containing match results.
2. The path where the calculated standings CSV should be saved.

### Run on Windows

Open PowerShell or the VS Code integrated terminal from the project directory.

Execute:

`dotnet run --project .\src\FootballStandings -c Release -- .\data\1974-75-week10-results.csv .\data\1974-75-week10-standings.csv`

The application processes the historical matches and generates the standings CSV.

**Expected output:**

```text
Standings calculated successfully.
Matches processed: 107
Teams ranked: 22
Output file: [absolute path to output CSV]
```

The generated file is located at:

`data/1974-75-week10-standings.csv`

### Display the League Table on Windows

Run the following PowerShell command:

`Import-Csv .\data\1974-75-week10-standings.csv | Format-Table -AutoSize`

This displays the complete 22-team league table in a readable column format in the PowerShell terminal.

Expand the terminal window if the columns are too narrow.

### Run on macOS

Open Terminal or the VS Code integrated terminal and navigate to the project directory.

Execute:

`dotnet run --project src/FootballStandings -c Release -- data/1974-75-week10-results.csv data/1974-75-week10-standings.csv`

**Expected output:**

```text
Standings calculated successfully.
Matches processed: 107
Teams ranked: 22
Output file: [absolute path to output CSV]
```

### Display the League Table on macOS

Run:

`column -s, -t data/1974-75-week10-standings.csv`

This displays the complete historical standings table in the terminal.

The `-s,` argument specifies the CSV comma delimiter, while `-t` formats the results into aligned columns.

The output includes all 22 teams, their match statistics and league positions.

**Important:** The C# application itself produces the standings CSV and displays a processing summary. The PowerShell `Import-Csv` and macOS `column` commands are separate terminal utilities used to display the generated CSV as a table. No additional C# implementation is needed to view the table.

## Expected Historical Results

The application processes 107 match results and generates standings for all 22 English First Division clubs.

The top five calculated positions are:

| Position | Team | Played | Points |
|---|---|---|---|
| 1 | Ipswich Town | 10 | 16 |
| 2 | Manchester City | 10 | 14 |
| 3 | Liverpool | 10 | 13 |
| 4 | Everton | 10 | 13 |
| 5 | Sheffield United | 10 | 13 |

The complete table contains statistics for every participating team.

## CSV Input Format

The application expects a CSV containing these columns:

`Date,HomeTeam,AwayTeam,HomeGoals,AwayGoals`

Example:

```csv
Date,HomeTeam,AwayTeam,HomeGoals,AwayGoals
1974-08-17,Sheffield United,QPR,1,1
1974-08-17,Birmingham City,Middlesbrough,0,3
1974-08-17,Everton,Derby County,0,0
```

Each row represents one completed football match.

Input validation covers required columns, valid dates, team names, goal values and duplicate fixture records.

Dates use the `yyyy-MM-dd` format, and goal values must be nonnegative integers.

## CSV Output Format

The application generates the following columns:

`Position,Team,Played,Won,Drawn,Lost,GoalsFor,GoalsAgainst,GoalAverage,Points`

Example:

```csv
Position,Team,Played,Won,Drawn,Lost,GoalsFor,GoalsAgainst,GoalAverage,Points
1,Ipswich Town,10,8,0,2,18,6,3.000,16
2,Manchester City,10,6,2,2,14,11,1.273,14
3,Liverpool,10,6,1,3,17,8,2.125,13
```

The output represents the ranked league table, with statistics accumulated from the input match results.

## Historical Football Rules

The application follows the points system used in the English First Division during the 1974/75 season.

| Result | Points |
|---|---|
| Win | 2 |
| Draw | 1 |
| Loss | 0 |

Unlike modern Premier League standings, teams were historically separated by goal average rather than goal difference.

**Goal average formula:**

Goal Average = Goals For / Goals Against

The implemented ranking criteria are:

1. Total points, highest first.
2. Goal average, highest first.
3. Goals scored, highest first.
4. Alphabetical ordering as a deterministic fallback for unresolved ties.

The application compares goal-average ratios without rounding to preserve accuracy. Three-decimal formatting is applied when exporting the standings CSV.

### Historical Rule Assumptions

The implementation treats a positive number of goals scored with zero goals conceded as an infinite goal average.

A team with zero goals scored and zero conceded is assigned a goal average of 1.

These exceptional cases are implementation conventions and are not claimed to be independently verified official 1974/75 Football League regulations.

The final alphabetical fallback is also an implementation decision rather than an official historical tie-breaker.

## Historical Dataset and Sources

The historical dataset represents completed English First Division fixtures from 17 August 1974 through 28 September 1974, corresponding to the first ten matchweeks in the source's fixture schedule.

The dataset includes:

- 107 completed matches
- 22 participating clubs
- 16 clubs with 10 matches played
- 6 clubs with 9 matches played

### Primary Data Source

[FBref / Sports Reference — 1974/75 First Division Scores & Fixtures](https://fbref.com/en/comps/9/1974-1975/schedule/1974-1975-Premier-League-Scores-and-Fixtures)

The source's fixture dates, teams and match scores were converted into the five-column CSV format used by this application.

The original matchweek labels were used to identify the appropriate fixture period. The application processes the supplied completed matches rather than interpreting matchweek labels itself.

### Historical Verification

The results were cross-checked against:

[LFChistory — Sheffield United vs Liverpool, 28 September 1974](https://www.lfchistory.net/games/818)

The historical reference agrees with the calculated match statistics and the first 19 league positions.

One discrepancy exists in the ordering of Arsenal and Tottenham. The reference places Tottenham above Arsenal, while the application places Arsenal higher because its goal average is 0.750 compared with Tottenham's approximately 0.733.

The application retains the ordering consistent with its documented goal-average calculation.

## Application Architecture

The solution separates responsibilities into models, calculation services and CSV processing classes.

### Models

`src/FootballStandings/Models/MatchResult.cs`

Represents an individual football match, including date, home and away teams, and final scores.

`src/FootballStandings/Models/TeamStanding.cs`

Maintains each team's accumulated league statistics, including matches played, wins, draws, losses, goals and points.

### Services

`src/FootballStandings/Services/StandingsCalculator.cs`

Processes match results, updates the standings for both participating teams and produces the ranked league table.

`src/FootballStandings/Services/HistoricalStandingsComparer.cs`

Implements the historical points and goal-average comparison rules used to order the standings.

### CSV Processing

`src/FootballStandings/Csv/CsvMatchReader.cs`

Reads and validates historical match data from CSV files.

`src/FootballStandings/Csv/CsvStandingsWriter.cs`

Writes the calculated league standings to an output CSV file.

### Application Entry Point

`src/FootballStandings/Program.cs`

Handles command-line arguments and coordinates CSV reading, standings calculations and output generation.

### Automated Tests

`tests/FootballStandings.Tests/`

Contains unit and integration tests for the application's components.

The historical integration test verifies that the supplied match dataset produces the expected league standings.

## Testing and Cross-Platform Verification

The application was tested using .NET 9 on both Windows and macOS.

| Verification | Windows | macOS |
|---|---|---|
| Dependency restoration | Passed | Passed |
| Release build | Passed | Passed |
| 61 automated tests | Passed | Passed |
| Historical CSV processing | Passed | Passed |
| 107 matches processed | Passed | Passed |
| 22 teams ranked | Passed | Passed |
| CSV output generation | Passed | Passed |

### Output Consistency

The standings CSV generated on macOS was compared against the committed output previously generated on Windows using:

`diff -u data/1974-75-week10-standings.csv data/mac-standings.csv`

The comparison returned exit code `0`, confirming that both files were identical.

This verifies consistent historical standings output across the two tested operating systems.

## Project Structure

```text
SpanFootballStandings-/
├── .claude/
│   └── README.md
├── ai/
│   └── README.md
├── data/
│   ├── 1974-75-week10-results.csv
│   ├── 1974-75-week10-standings.csv
│   ├── sample-results.csv
│   └── sample-standings.csv
├── src/
│   └── FootballStandings/
│       ├── Csv/
│       ├── Models/
│       ├── Services/
│       ├── FootballStandings.csproj
│       └── Program.cs
├── tests/
│   └── FootballStandings.Tests/
├── .gitignore
├── AI_REFLECTION.md
├── CLAUDE.md
├── README.md
└── SpanFootballStandings.sln
```

## AI Collaboration

OpenAI ChatGPT was used as a development assistant during requirements analysis, application design, implementation, debugging, automated testing, historical verification and documentation.

The project was developed incrementally, with changes reviewed, tested and committed through Git.

The repository includes:

- `CLAUDE.md` — A retrospective record of the project instructions and development guidelines used with ChatGPT.
- `AI_REFLECTION.md` — A reflection on the development process, engineering decisions and AI collaboration.
- `ai/README.md` — AI collaboration evidence and a shared ChatGPT conversation link.
- `.claude/README.md` — An explanation of the absence of native Claude Code session files.

### ChatGPT Development Conversation

[View the ChatGPT Development Conversation](https://chatgpt.com/share/6ad78201-66c0-83e9-afff-2869ae3ea626)

ChatGPT was used instead of Claude Code. No Claude Code session files have been fabricated.

The shared conversation link is provided as genuine collaboration evidence. A local export has been requested but is not yet included.

Acceptance of this equivalent conversation evidence remains subject to confirmation by SPAN Digital.

## Known Limitations and Assumptions

- Historical zero-goal goal-average conventions are documented implementation choices.
- Unresolved ranking ties use alphabetical ordering rather than a verified official historical tie-breaker.
- The application calculates standings from completed matches supplied in the input CSV; it does not automatically download results or select fixtures by week.
- The league table is exported to CSV. A terminal formatting utility is used when a human-readable display is desired.
- The application has been verified on Windows and macOS. Linux execution has not been separately verified.
- The remaining submission checks include command-line failure handling and confirmation that the supplied ChatGPT evidence is accepted as equivalent to the requested Claude Code artifacts.

## Assessment

Developed for the SPAN Digital Associate Software Engineer coding assessment.

The project demonstrates C# application development, historical rules implementation, CSV processing, automated testing, separation of concerns, Git version control and responsible AI-assisted software engineering.
