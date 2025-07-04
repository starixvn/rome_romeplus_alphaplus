using AlphaPlus.CashService.API.Features.Shared.Models;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Shared.Models;

public class SetupCheckListModelTests
{
    [Fact]
    public void Constructor_WhenCalledWithNoParameters_ShouldInitializeWithDefaultValues()
    {
        // Act
        var model = new SetupCheckListModel();

        // Assert
        Assert.False(model.HasDepositMethods);
        Assert.False(model.HasWithdrawalMethods);
        Assert.False(model.HasLineID);
        Assert.False(model.IsCompleteAll);
    }

    [Fact]
    public void Constructor_WhenCalledWithAllTrueParameters_ShouldInitializeWithCorrectValues()
    {
        // Arrange
        bool hasDepositMethods = true;
        bool hasWithdrawalMethods = true;
        bool hasLineID = true;

        // Act
        var model = new SetupCheckListModel(hasDepositMethods, hasWithdrawalMethods, hasLineID);

        // Assert
        Assert.True(model.HasDepositMethods);
        Assert.True(model.HasWithdrawalMethods);
        Assert.True(model.HasLineID);
        Assert.True(model.IsCompleteAll);
    }

    [Fact]
    public void Constructor_WhenCalledWithAllFalseParameters_ShouldInitializeWithCorrectValues()
    {
        // Arrange
        bool hasDepositMethods = false;
        bool hasWithdrawalMethods = false;
        bool hasLineID = false;

        // Act
        var model = new SetupCheckListModel(hasDepositMethods, hasWithdrawalMethods, hasLineID);

        // Assert
        Assert.False(model.HasDepositMethods);
        Assert.False(model.HasWithdrawalMethods);
        Assert.False(model.HasLineID);
        Assert.False(model.IsCompleteAll);
    }

    [Theory]
    [InlineData(true, false, false)]
    [InlineData(false, true, false)]
    [InlineData(false, false, true)]
    [InlineData(true, true, false)]
    [InlineData(true, false, true)]
    [InlineData(false, true, true)]
    public void Constructor_WhenCalledWithMixedParameters_ShouldInitializeWithCorrectValues(
        bool hasDepositMethods, bool hasWithdrawalMethods, bool hasLineID)
    {
        // Act
        var model = new SetupCheckListModel(hasDepositMethods, hasWithdrawalMethods, hasLineID);

        // Assert
        Assert.Equal(hasDepositMethods, model.HasDepositMethods);
        Assert.Equal(hasWithdrawalMethods, model.HasWithdrawalMethods);
        Assert.Equal(hasLineID, model.HasLineID);
        Assert.False(model.IsCompleteAll);
    }

    [Fact]
    public void IsCompleteAll_WhenAllPropertiesAreTrue_ShouldReturnTrue()
    {
        // Arrange
        var model = new SetupCheckListModel(true, true, true);

        // Act
        bool result = model.IsCompleteAll;

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(false, true, true)]
    [InlineData(true, false, true)]
    [InlineData(true, true, false)]
    [InlineData(false, false, true)]
    [InlineData(false, true, false)]
    [InlineData(true, false, false)]
    [InlineData(false, false, false)]
    public void IsCompleteAll_WhenAnyPropertyIsFalse_ShouldReturnFalse(
        bool hasDepositMethods, bool hasWithdrawalMethods, bool hasLineID)
    {
        // Arrange
        var model = new SetupCheckListModel(hasDepositMethods, hasWithdrawalMethods, hasLineID);

        // Act
        bool result = model.IsCompleteAll;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Properties_WhenSetIndividually_ShouldUpdateCorrectly()
    {
        // Arrange
        var model = new SetupCheckListModel();

        // Act & Assert
        model.HasDepositMethods = true;
        Assert.True(model.HasDepositMethods);
        Assert.False(model.IsCompleteAll);

        model.HasWithdrawalMethods = true;
        Assert.True(model.HasWithdrawalMethods);
        Assert.False(model.IsCompleteAll);

        model.HasLineID = true;
        Assert.True(model.HasLineID);
        Assert.True(model.IsCompleteAll);
    }

    [Fact]
    public void Properties_WhenSetToFalse_ShouldUpdateCorrectly()
    {
        // Arrange
        var model = new SetupCheckListModel(true, true, true);

        // Act & Assert
        model.HasDepositMethods = false;
        Assert.False(model.HasDepositMethods);
        Assert.False(model.IsCompleteAll);

        model.HasWithdrawalMethods = false;
        Assert.False(model.HasWithdrawalMethods);
        Assert.False(model.IsCompleteAll);

        model.HasLineID = false;
        Assert.False(model.HasLineID);
        Assert.False(model.IsCompleteAll);
    }

    [Fact]
    public void IsCompleteAll_WhenPropertiesAreChangedDynamically_ShouldUpdateCorrectly()
    {
        // Arrange
        var model = new SetupCheckListModel(false, false, false);

        // Act & Assert - Start with all false
        Assert.False(model.IsCompleteAll);

        // Set one to true
        model.HasDepositMethods = true;
        Assert.False(model.IsCompleteAll);

        // Set second to true
        model.HasWithdrawalMethods = true;
        Assert.False(model.IsCompleteAll);

        // Set third to true - now all should be complete
        model.HasLineID = true;
        Assert.True(model.IsCompleteAll);

        // Set one back to false
        model.HasDepositMethods = false;
        Assert.False(model.IsCompleteAll);
    }
} 