using AlphaPlus.Reports.Features.Statement;

namespace AlphaPlus.Reports.UnitTests.Features.Statement;

[Trait("Category", "Statement")]
public class StatementModelTest
{
    [Fact]
    public void IsNotedBalance_ShouldReturnFalse_WhenPreviousStatementIsNull()
    {
        // Arrange
        var currentStatement = new StatementModel
        {
            OpeningBalance = 100
        };

        // Act
        var result = currentStatement.IsNotedBalance(null);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsNotedBalance_ShouldReturnFalse_WhenPreviousClosingBalanceMatchesCurrentOpeningBalance()
    {
        // Arrange
        var previousStatement = new StatementModel
        {
            ClosingBalance = 100
        };

        var currentStatement = new StatementModel
        {
            OpeningBalance = 100
        };

        // Act
        var result = currentStatement.IsNotedBalance(previousStatement);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsNotedBalance_ShouldReturnTrue_WhenPreviousClosingBalanceDoesNotMatchCurrentOpeningBalance()
    {
        // Arrange
        var previousStatement = new StatementModel
        {
            ClosingBalance = 100
        };

        var currentStatement = new StatementModel
        {
            OpeningBalance = 150
        };

        // Act
        var result = currentStatement.IsNotedBalance(previousStatement);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsNotedBalance_ShouldReturnTrue_WhenPreviousClosingBalanceIsNull()
    {
        // Arrange
        var previousStatement = new StatementModel
        {
            ClosingBalance = null
        };

        var currentStatement = new StatementModel
        {
            OpeningBalance = 100
        };

        // Act
        var result = currentStatement.IsNotedBalance(previousStatement);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsNotedBalance_ShouldReturnTrue_WhenCurrentOpeningBalanceIsNull()
    {
        // Arrange
        var previousStatement = new StatementModel
        {
            ClosingBalance = 100
        };

        var currentStatement = new StatementModel
        {
            OpeningBalance = null
        };

        // Act
        var result = currentStatement.IsNotedBalance(previousStatement);

        // Assert
        Assert.True(result);
    }
}