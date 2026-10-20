
using FootballStandings.Csv;

namespace FootballStandings.Tests;

public class CsvMatchReaderTests
{
    private const string Header =
        "Date,HomeTeam,AwayTeam,HomeGoals,AwayGoals\n";

    private static CsvMatchReader CreateReader() => new();

    private static void ReadCsv(string content)
    {
        using var input = new StringReader(content);
        CreateReader().Read(input);
    }

    [Fact]
    public void Read_ValidCsv_ReturnsMatches()
    {
        string content = Header +
            "1974-08-17,Arsenal,Chelsea,2,1\n" +
            "1974-08-17,Liverpool,Everton,0,0\n";

        using var input = new StringReader(content);

        var matches = CreateReader().Read(input);

        Assert.Equal(2, matches.Count);
        Assert.Equal("Arsenal", matches[0].HomeTeam);
        Assert.Equal(2, matches[0].HomeGoals);
        Assert.Equal(1, matches[0].AwayGoals);
        Assert.Equal(new DateOnly(1974, 8, 17),
            matches[0].Date);
    }

    [Fact]
    public void Read_QuotedTeamName_HandlesComma()
    {
        string content = Header +
            "1974-08-17,\"Town, United\",Chelsea,2,1\n";

        using var input = new StringReader(content);

        var matches = CreateReader().Read(input);

        Assert.Equal("Town, United",
            Assert.Single(matches).HomeTeam);
    }

    [Fact]
    public void Read_ReorderedHeaders_ParsesCorrectly()
    {
        string content =
            "HomeTeam,Date,HomeGoals,AwayTeam,AwayGoals\n" +
            "Arsenal,1974-08-17,2,Chelsea,1\n";

        using var input = new StringReader(content);

        var match = Assert.Single(
            CreateReader().Read(input));

        Assert.Equal("Arsenal", match.HomeTeam);
        Assert.Equal("Chelsea", match.AwayTeam);
        Assert.Equal(2, match.HomeGoals);
    }

    [Theory]
    [InlineData("1974-20-17,Arsenal,Chelsea,2,1")]
    [InlineData("1974-08-17,Arsenal,Chelsea,abc,1")]
    [InlineData("1974-08-17,Arsenal,Chelsea,-2,1")]
    [InlineData("1974-08-17,,Chelsea,2,1")]
    [InlineData("1974-08-17,Arsenal,Arsenal,2,1")]
    public void Read_InvalidData_ThrowsException(string row)
    {
        Assert.Throws<InvalidDataException>(() =>
            ReadCsv(Header + row + "\n"));
    }

    [Theory]
    [InlineData("1974-08-17,Arsenal,Chelsea,2")]
    [InlineData("1974-08-17,Arsenal,Chelsea,2,1,Extra")]
    public void Read_WrongColumnCount_ThrowsException(
        string row)
    {
        Assert.Throws<InvalidDataException>(() =>
            ReadCsv(Header + row + "\n"));
    }

    [Fact]
    public void Read_DuplicateFixture_ThrowsException()
    {
        string content = Header +
            "1974-08-17,Arsenal,Chelsea,2,1\n" +
            "1974-08-17,ARSENAL,CHELSEA,1,1\n";

        Assert.Throws<InvalidDataException>(() =>
            ReadCsv(content));
    }

    [Fact]
    public void Read_InvalidHeaders_ThrowsException()
    {
        string content =
            "Date,Home,Away,HomeGoals,AwayGoals\n" +
            "1974-08-17,Arsenal,Chelsea,2,1\n";

        Assert.Throws<InvalidDataException>(() =>
            ReadCsv(content));
    }

    [Fact]
    public void Read_EmptyFile_ThrowsException()
    {
        Assert.Throws<InvalidDataException>(() =>
            ReadCsv(""));
    }

    [Fact]
    public void Read_HeaderOnly_ReturnsEmptyCollection()
    {
        using var input = new StringReader(Header);

        var matches = CreateReader().Read(input);

        Assert.Empty(matches);
    }

    [Fact]
    public void Read_MissingFile_ThrowsException()
    {
        string missingFile = Path.Combine(
            Path.GetTempPath(),
            Guid.NewGuid() + ".csv");

        Assert.Throws<FileNotFoundException>(() =>
            CreateReader().Read(missingFile));
    }
}
