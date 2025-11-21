using AlphaPlus.Settings.MemberInfo.Domain.Constants;

namespace AlphaPlus.Settings.MemberInfo.Domain.UnitTests.Constants;

public class ProductIdTests
{
    [Fact]
    public void ProductId_Should_Have_Correct_Sportsbook_Value()
    {
        // Act & Assert
        Assert.Equal(1, ProductId.Sportsbook);
    }

    [Fact]
    public void ProductId_Should_Have_Correct_MacauGames_Value()
    {
        // Act & Assert
        Assert.Equal(14, ProductId.MacauGames);
    }

    [Fact]
    public void ProductId_Should_Have_Correct_Pragmatic_Value()
    {
        // Act & Assert
        Assert.Equal(32, ProductId.Pragmatic);
    }

    [Fact]
    public void ProductId_Should_Have_Unique_Values()
    {
        // Arrange
        var values = new[]
        {
            ProductId.Sportsbook,
            ProductId.MacauGames,
            ProductId.Pragmatic
        };

        // Act & Assert
        Assert.Equal(values.Length, values.Distinct().Count());
    }

    [Fact]
    public void ProductId_Should_Have_Positive_Values()
    {
        // Act & Assert
        Assert.True(ProductId.Sportsbook > 0);
        Assert.True(ProductId.MacauGames > 0);
        Assert.True(ProductId.Pragmatic > 0);
    }
}
