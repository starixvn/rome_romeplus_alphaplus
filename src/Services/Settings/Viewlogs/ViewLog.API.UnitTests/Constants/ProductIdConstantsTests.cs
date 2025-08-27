using AlphaPlus.Settings.ViewLogs.API.Constants;

namespace AlphaPlus.Settings.ViewLogs.API.UnitTests.Constants;

public class ProductIdConstantsTests
{
    [Fact]
    public void ProductIdConstants_Should_Have_Correct_VirtualSports_Value()
    {
        // Act & Assert
        Assert.Equal(5, AlphaPlus.Settings.ViewLogs.API.Constants.ProductIdConstants.VirtualSports);
    }

    [Fact]
    public void ProductIdConstants_Should_Have_Correct_Sportsbook2_Value()
    {
        // Act & Assert
        Assert.Equal(16, AlphaPlus.Settings.ViewLogs.API.Constants.ProductIdConstants.Sportsbook2);
    }

    [Fact]
    public void ProductIdConstants_Should_Have_Positive_ProductIds()
    {
        // Act & Assert
        Assert.True(AlphaPlus.Settings.ViewLogs.API.Constants.ProductIdConstants.VirtualSports > 0);
        Assert.True(AlphaPlus.Settings.ViewLogs.API.Constants.ProductIdConstants.Sportsbook2 > 0);
    }

    [Fact]
    public void ProductIdConstants_Should_Have_Unique_Values()
    {
        // Arrange
        var values = new[]
        {
            AlphaPlus.Settings.ViewLogs.API.Constants.ProductIdConstants.VirtualSports,
            AlphaPlus.Settings.ViewLogs.API.Constants.ProductIdConstants.Sportsbook2
        };

        // Act & Assert
        Assert.Equal(values.Length, values.Distinct().Count());
    }

    [Fact]
    public void ProductIdConstants_Should_Have_Reasonable_ProductId_Values()
    {
        // Act & Assert
        Assert.True(AlphaPlus.Settings.ViewLogs.API.Constants.ProductIdConstants.VirtualSports >= 1 && AlphaPlus.Settings.ViewLogs.API.Constants.ProductIdConstants.VirtualSports <= 100);
        Assert.True(AlphaPlus.Settings.ViewLogs.API.Constants.ProductIdConstants.Sportsbook2 >= 1 && AlphaPlus.Settings.ViewLogs.API.Constants.ProductIdConstants.Sportsbook2 <= 100);
    }
}
