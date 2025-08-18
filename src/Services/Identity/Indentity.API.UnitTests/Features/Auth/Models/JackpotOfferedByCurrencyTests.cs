namespace AlphaPlus.Identity.API.UnitTests.Features.Auth.Models;

public class JackpotOfferedByCurrencyTests
{
    [Fact]
    public void JackpotOfferedByCurrency_WithValidData_ShouldCreateCorrectly()
    {
        // Arrange & Act
        var jackpotOfferedByCurrency = new JackpotOfferedByCurrency
        {
            Currency = 1,
            ProductId = 123
        };

        // Assert
        jackpotOfferedByCurrency.Currency.Should().Be(1);
        jackpotOfferedByCurrency.ProductId.Should().Be(123);
    }

    [Fact]
    public void JackpotOfferedByCurrency_WithZeroValues_ShouldSetCorrectly()
    {
        // Arrange & Act
        var jackpotOfferedByCurrency = new JackpotOfferedByCurrency
        {
            Currency = 0,
            ProductId = 0
        };

        // Assert
        jackpotOfferedByCurrency.Currency.Should().Be(0);
        jackpotOfferedByCurrency.ProductId.Should().Be(0);
    }

    [Fact]
    public void JackpotOfferedByCurrency_WithNegativeValues_ShouldSetCorrectly()
    {
        // Arrange & Act
        var jackpotOfferedByCurrency = new JackpotOfferedByCurrency
        {
            Currency = -1,
            ProductId = -123
        };

        // Assert
        jackpotOfferedByCurrency.Currency.Should().Be(-1);
        jackpotOfferedByCurrency.ProductId.Should().Be(-123);
    }

    [Fact]
    public void JackpotOfferedByCurrency_WithLargeValues_ShouldSetCorrectly()
    {
        // Arrange & Act
        var jackpotOfferedByCurrency = new JackpotOfferedByCurrency
        {
            Currency = int.MaxValue,
            ProductId = int.MaxValue
        };

        // Assert
        jackpotOfferedByCurrency.Currency.Should().Be(int.MaxValue);
        jackpotOfferedByCurrency.ProductId.Should().Be(int.MaxValue);
    }

    [Fact]
    public void JackpotOfferedByCurrency_Equals_ShouldWorkCorrectly()
    {
        // Arrange
        var jackpot1 = new JackpotOfferedByCurrency { Currency = 1, ProductId = 100 };
        var jackpot2 = new JackpotOfferedByCurrency { Currency = 1, ProductId = 100 };
        var jackpot3 = new JackpotOfferedByCurrency { Currency = 2, ProductId = 100 };

        // Assert
        jackpot1.Should().BeEquivalentTo(jackpot2);
        jackpot1.Should().NotBeEquivalentTo(jackpot3);
    }

    [Theory]
    [InlineData(1, 100)]
    [InlineData(2, 200)]
    [InlineData(3, 300)]
    [InlineData(0, 0)]
    [InlineData(-1, -100)]
    public void JackpotOfferedByCurrency_WithDifferentValues_ShouldSetCorrectly(int currency, int productId)
    {
        // Arrange & Act
        var jackpotOfferedByCurrency = new JackpotOfferedByCurrency
        {
            Currency = currency,
            ProductId = productId
        };

        // Assert
        jackpotOfferedByCurrency.Currency.Should().Be(currency);
        jackpotOfferedByCurrency.ProductId.Should().Be(productId);
    }
}
