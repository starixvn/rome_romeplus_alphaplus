using AlphaPlus.Settings.MemberInfo.Domain.Constants;

namespace AlphaPlus.Settings.MemberInfo.Domain.UnitTests.Constants;

public class ProductSubGroupConstantsTests
{
    [Fact]
    public void ProductSubGroupConstants_Should_Have_Correct_PTAllbetAndSAGaming_Value()
    {
        // Act & Assert
        Assert.Equal(5, ProductSubGroupConstants.PTAllbetAndSAGaming);
    }

    [Fact]
    public void ProductSubGroupConstants_Should_Have_Correct_PTOTherLiveCasinoAndRNGGames_Value()
    {
        // Act & Assert
        Assert.Equal(6, ProductSubGroupConstants.PTOTherLiveCasinoAndRNGGames);
    }

    [Fact]
    public void ProductSubGroupConstants_Should_Have_Positive_Values()
    {
        // Act & Assert
        Assert.True(ProductSubGroupConstants.PTAllbetAndSAGaming > 0);
        Assert.True(ProductSubGroupConstants.PTOTherLiveCasinoAndRNGGames > 0);
    }

    [Fact]
    public void ProductSubGroupConstants_Should_Have_Unique_Values()
    {
        // Arrange
        var values = new[]
        {
            ProductSubGroupConstants.PTAllbetAndSAGaming,
            ProductSubGroupConstants.PTOTherLiveCasinoAndRNGGames
        };

        // Act & Assert
        Assert.Equal(values.Length, values.Distinct().Count());
    }

    [Fact]
    public void ProductSubGroupConstants_Should_Have_Sequential_Values()
    {
        // Arrange
        var expectedValues = new[] { 5, 6 };
        var actualValues = new[]
        {
            ProductSubGroupConstants.PTAllbetAndSAGaming,
            ProductSubGroupConstants.PTOTherLiveCasinoAndRNGGames
        };

        // Act & Assert
        Assert.Equal(expectedValues, actualValues);
    }

    [Fact]
    public void ProductSubGroupConstants_PTOTherLiveCasinoAndRNGGames_Should_Be_Greater_Than_PTAllbetAndSAGaming()
    {
        // Act & Assert
        Assert.True(ProductSubGroupConstants.PTOTherLiveCasinoAndRNGGames > ProductSubGroupConstants.PTAllbetAndSAGaming);
    }
}
