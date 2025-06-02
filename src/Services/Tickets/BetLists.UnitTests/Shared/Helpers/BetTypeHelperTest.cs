using AlphaPlus.BetLists.Shared.Helpers;

namespace AlphaPlus.BetLists.UnitTests.Shared.Helpers;

[Trait("Category", "BetLists")]
public class BetTypeHelperTest
{
    [Fact]
    public void IsOutRight_ShouldReturnTrue_WhenBetTypeIsOutright()
    {
        // Arrange
        int betType = 10;

        // Act
        bool result = BetTypeHelper.IsOutRight(betType);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsOutRight_ShouldReturnFalse_WhenBetTypeIsNotOutright()
    {
        // Arrange
        int betType = 11;

        // Act
        bool result = BetTypeHelper.IsOutRight(betType);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsLeapGaming_ShouldReturnTrue_WhenBetTypeIsLeapGaming()
    {
        // Arrange
        int betType = 5301;

        // Act
        bool result = BetTypeHelper.IsLeapGaming(betType);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsLeapGaming_ShouldReturnFalse_WhenBetTypeIsNotLeapGaming()
    {
        // Arrange
        int betType = 5300;

        // Act
        bool result = BetTypeHelper.IsLeapGaming(betType);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(3601)]
    [InlineData(3610)]
    [InlineData(3602)]
    public void IsHappy5LinkedGames_ShouldReturnTrue_WhenBetTypeIsInRange(int betType)
    {
        // Act
        bool result = BetTypeHelper.IsHappy5LinkedGames(betType);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(3600)]
    [InlineData(3611)]
    public void IsHappy5LinkedGames_ShouldReturnFalse_WhenBetTypeIsOutOfRange(int betType)
    {
        // Act
        bool result = BetTypeHelper.IsHappy5LinkedGames(betType);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(1231)]
    [InlineData(1232)]
    [InlineData(1233)]
    public void IsHorseRacing_ShouldReturnTrue_WhenBetTypeIsHorseRacing(int betType)
    {
        // Act
        bool result = BetTypeHelper.IsHorseRacing(betType);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsHorseRacing_ShouldReturnFalse_WhenBetTypeIsNotHorseRacing()
    {
        // Arrange
        int betType = -999; // Some value not in the horse racing array

        // Act
        bool result = BetTypeHelper.IsHorseRacing(betType);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(2198)]
    [InlineData(2398)]
    [InlineData(2998)]
    [InlineData(4297)]
    [InlineData(4599)]
    public void IsJackpotContribution_ShouldReturnTrue_WhenBetTypeIsJackpotContribution(int betType)
    {
        // Act
        bool result = BetTypeHelper.IsJackpotContribution(betType);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsJackpotContribution_ShouldReturnFalse_WhenBetTypeIsNotJackpotContribution()
    {
        // Arrange
        int betType = -999; // Some value not in the jackpot contribution array

        // Act
        bool result = BetTypeHelper.IsJackpotContribution(betType);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(1, 5)]
    [InlineData(0, 1)]
    [InlineData(2, 3)]
    [InlineData(3, 1)]
    [InlineData(99, 1)]
    public void GetEventType_ShouldReturnCorrectEventType(byte leagueType, byte expectedEventType)
    {
        // Act
        byte result = BetTypeHelper.GetEventType(leagueType);

        // Assert
        Assert.Equal(expectedEventType, result);
    }
}