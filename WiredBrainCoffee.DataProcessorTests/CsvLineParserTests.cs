namespace WiredBrainCoffee.DataProcessor.Parsing;

public class CsvLineParserTest
{
    [Fact]
    public void ShouldParseValidLine()
    {
        // arrange
        string[] csvLines = new[] { "Cappuccino;10/27/2022 8:06:04 AM" };

        // act
        var machineDataItems = CsvLineParser.Parse(csvLines);

        // assert
        Assert.NotNull(machineDataItems);
        Assert.Single(machineDataItems);
        Assert.Equal("Cappuccino", machineDataItems[0].CoffeeType);
        Assert.Equal(new DateTime(2022, 10, 27, 8, 6, 4), machineDataItems[0].CreatedAt);
    }

    [Fact]
    public void ShouldSkipEmptyLines()
    {
        // arrange
        string[] csvLines = new[] { "" };

        // act
        var machineDataItems = CsvLineParser.Parse(csvLines);

        // assert
        Assert.NotNull(machineDataItems);
        Assert.Empty(machineDataItems);

    }

}