using AlphaPlus.Identity.API.Features.Auth;
using FluentAssertions;
using Xunit;

namespace AlphaPlus.Identity.API.UnitTests.Features.Auth.Models;

public class SecurityCodeFunctionTests
{
    [Fact]
    public void SecurityCodeFunction_WithValidData_ShouldCreateCorrectly()
    {
        // Arrange & Act
        var securityCodeFunction = new SecurityCodeFunction
        {
            BetList = true,
            Statement = false
        };

        // Assert
        securityCodeFunction.BetList.Should().BeTrue();
        securityCodeFunction.Statement.Should().BeFalse();
    }

    [Fact]
    public void SecurityCodeFunction_WithBothTrue_ShouldSetCorrectly()
    {
        // Arrange & Act
        var securityCodeFunction = new SecurityCodeFunction
        {
            BetList = true,
            Statement = true
        };

        // Assert
        securityCodeFunction.BetList.Should().BeTrue();
        securityCodeFunction.Statement.Should().BeTrue();
    }

    [Fact]
    public void SecurityCodeFunction_WithBothFalse_ShouldSetCorrectly()
    {
        // Arrange & Act
        var securityCodeFunction = new SecurityCodeFunction
        {
            BetList = false,
            Statement = false
        };

        // Assert
        securityCodeFunction.BetList.Should().BeFalse();
        securityCodeFunction.Statement.Should().BeFalse();
    }

    [Fact]
    public void SecurityCodeFunction_WithMixedValues_ShouldSetCorrectly()
    {
        // Arrange & Act
        var securityCodeFunction = new SecurityCodeFunction
        {
            BetList = false,
            Statement = true
        };

        // Assert
        securityCodeFunction.BetList.Should().BeFalse();
        securityCodeFunction.Statement.Should().BeTrue();
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(false, false)]
    public void SecurityCodeFunction_WithDifferentCombinations_ShouldSetCorrectly(bool betList, bool statement)
    {
        // Arrange & Act
        var securityCodeFunction = new SecurityCodeFunction
        {
            BetList = betList,
            Statement = statement
        };

        // Assert
        securityCodeFunction.BetList.Should().Be(betList);
        securityCodeFunction.Statement.Should().Be(statement);
    }

    [Fact]
    public void SecurityCodeFunction_DefaultValues_ShouldBeFalse()
    {
        // Arrange & Act
        var securityCodeFunction = new SecurityCodeFunction();

        // Assert
        securityCodeFunction.BetList.Should().BeFalse();
        securityCodeFunction.Statement.Should().BeFalse();
    }
}
