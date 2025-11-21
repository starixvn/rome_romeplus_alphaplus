using AlphaPlus.Settings.MemberInfo.Domain.Constants;

namespace AlphaPlus.Settings.MemberInfo.Domain.UnitTests.Constants;

public class ProductGroupConstantsTests
{
    [Fact]
    public void ProductGroupConstants_Should_Have_Correct_Values()
    {
        // Assert
        Assert.Equal(1, ProductGroupConstants.Sportsbook);
        Assert.Equal(2, ProductGroupConstants.RNGGames);
        Assert.Equal(3, ProductGroupConstants.LiveAndRNGGames);
        Assert.Equal(4, ProductGroupConstants.NG2);
    }

    [Fact]
    public void ProductGroupConstants_Should_Be_Integer_Type()
    {
        // Assert
        Assert.IsType<int>(ProductGroupConstants.Sportsbook);
        Assert.IsType<int>(ProductGroupConstants.RNGGames);
        Assert.IsType<int>(ProductGroupConstants.LiveAndRNGGames);
        Assert.IsType<int>(ProductGroupConstants.NG2);
    }

    [Fact]
    public void ProductGroupConstants_Should_Be_Unique()
    {
        // Arrange
        var values = new[]
        {
            ProductGroupConstants.Sportsbook,
            ProductGroupConstants.RNGGames,
            ProductGroupConstants.LiveAndRNGGames,
            ProductGroupConstants.NG2
        };

        // Act
        var uniqueValues = values.Distinct().ToArray();

        // Assert
        Assert.Equal(values.Length, uniqueValues.Length);
    }

    [Fact]
    public void ProductGroupConstants_Should_Be_Positive_Values()
    {
        // Assert
        Assert.True(ProductGroupConstants.Sportsbook > 0);
        Assert.True(ProductGroupConstants.RNGGames > 0);
        Assert.True(ProductGroupConstants.LiveAndRNGGames > 0);
        Assert.True(ProductGroupConstants.NG2 > 0);
    }

    [Fact]
    public void ProductGroupConstants_Should_Be_Sequential_Values()
    {
        // Assert
        Assert.Equal(1, ProductGroupConstants.Sportsbook);
        Assert.Equal(2, ProductGroupConstants.RNGGames);
        Assert.Equal(3, ProductGroupConstants.LiveAndRNGGames);
        Assert.Equal(4, ProductGroupConstants.NG2);
    }
}
