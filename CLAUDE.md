# AI Project Instructions — SPAN Football Standings

## AI Tool Disclosure

This project was developed with assistance from OpenAI ChatGPT, not Claude Code.

This file documents the project instructions and development principles followed during ChatGPT-assisted development. It was prepared as an equivalent project-instructions artifact for the SPAN Digital assessment after the development work had progressed.

No native Claude Code sessions or configuration are claimed.

## Project Overview

Develop a production-ready command-line application using C# and .NET 9 that calculates English First Division league standings for week 10 of the 1974/75 season.

The application must read football match results from a CSV file, calculate team statistics according to the historical football rules, and produce a ranked standings CSV file.

The application must run in a Unix-like environment, including macOS.

## Development Guidelines

- Use C# and .NET 9.
- Keep the application simple, maintainable and testable.
- Follow separation of concerns.
- Use meaningful names and appropriate error handling.
- Avoid unnecessary abstractions and overengineering.
- Explain important engineering decisions and implementation trade-offs.
- Implement functionality incrementally.
- Verify code through compilation and automated tests.
- Do not claim that code works unless the relevant tests have been run.
- Use Git commits to preserve meaningful development milestones.

## Application Architecture

The project uses the following components:

- `Models/MatchResult.cs` — represents and validates individual match results.
- `Models/TeamStanding.cs` — stores accumulated statistics for a football club.
- `Services/StandingsCalculator.cs` — processes match results and calculates standings.
- `Services/HistoricalStandingsComparer.cs` — compares teams using the implemented historical ranking criteria.
- `Csv/CsvMatchReader.cs` — reads and validates CSV input.
- `Csv/CsvStandingsWriter.cs` — exports the ranked league table.
- `Program.cs` — handles command-line arguments and coordinates execution.

## Historical Football Rules

The 1974/75 English First Division used two points for a win, one point for a draw and zero points for a loss.

The implemented ranking criteria are:

1. Total points, descending.
2. Goal average, descending.
3. Goals scored, descending.

Goal average is calculated using goals scored divided by goals conceded, rather than modern goal difference.

The implementation compares ratios without rounding. Exceptional zero-goal cases follow documented conventions.

An alphabetical fallback is currently used for unresolved ties. This is an implementation decision rather than a verified official historical tie-breaker.

## CSV Requirements

The input CSV contains:

`Date,HomeTeam,AwayTeam,HomeGoals,AwayGoals`

The output CSV contains:

`Position,Team,Played,Won,Drawn,Lost,GoalsFor,GoalsAgainst,GoalAverage,Points`

The historical dataset includes 107 fixtures involving 22 teams through 28 September 1974, based on the source's first ten matchweek labels and actual match dates.

## Testing Requirements

Use xUnit to test:

- Match result validation.
- Points calculations.
- Wins, draws and losses.
- Goal accumulation.
- Historical ranking rules.
- Goal-average edge cases.
- CSV parsing and validation.
- CSV output formatting.
- Integration between the CSV reader, calculator and writer.
- Historical dataset processing.

Run the test suite using:

`dotnet test`

## AI Collaboration Principles

ChatGPT was used as a technical development assistant.

The developer remained responsible for reviewing suggestions, implementing changes, running tests and verifying results.

AI-generated code and explanations were treated as suggestions requiring verification rather than automatically correct solutions.

The project was developed incrementally, with build and test results reviewed before proceeding to subsequent components.

## AI Collaboration Evidence

The project includes an `AI_REFLECTION.md` file and an `ai/README.md` linking to the genuine ChatGPT development conversation.

Native Claude Code session files were not generated because Claude Code was not used.

AI collaboration records should remain authentic and must not be fabricated or represented as originating from tools that were not used.