namespace AlphaPlus.Identity.API.UnitTests.Features.Auth.Models;

public class ProductSettingTests
{
    [Fact]
    public void ProductSetting_WithValidData_ShouldCreateCorrectly()
    {
        // Arrange & Act
        var productSetting = new ProductSetting
        {
            ProductId = 123,
            GroupId = 456,
            SportType = 1,
            BetType = 2,
            IsHidden = false,
            IsDisabled = true
        };

        // Assert
        productSetting.ProductId.Should().Be(123);
        productSetting.GroupId.Should().Be(456);
        productSetting.SportType.Should().Be(1);
        productSetting.BetType.Should().Be(2);
        productSetting.IsHidden.Should().BeFalse();
        productSetting.IsDisabled.Should().BeTrue();
    }

    [Fact]
    public void ProductSetting_WithNullGroupId_ShouldBeAllowed()
    {
        // Arrange & Act
        var productSetting = new ProductSetting
        {
            ProductId = 123,
            GroupId = null,
            SportType = 1,
            BetType = 2,
            IsHidden = false,
            IsDisabled = false
        };

        // Assert
        productSetting.GroupId.Should().BeNull();
        productSetting.ProductId.Should().Be(123);
    }

    [Fact]
    public void ProductSetting_WithZeroValues_ShouldBeValid()
    {
        // Arrange & Act
        var productSetting = new ProductSetting
        {
            ProductId = 0,
            GroupId = 0,
            SportType = 0,
            BetType = 0,
            IsHidden = false,
            IsDisabled = false
        };

        // Assert
        productSetting.ProductId.Should().Be(0);
        productSetting.GroupId.Should().Be(0);
        productSetting.SportType.Should().Be(0);
        productSetting.BetType.Should().Be(0);
    }

    [Fact]
    public void ProductSetting_WithNegativeValues_ShouldBeValid()
    {
        // Arrange & Act
        var productSetting = new ProductSetting
        {
            ProductId = -1,
            GroupId = -2,
            SportType = -3,
            BetType = -4,
            IsHidden = true,
            IsDisabled = true
        };

        // Assert
        productSetting.ProductId.Should().Be(-1);
        productSetting.GroupId.Should().Be(-2);
        productSetting.SportType.Should().Be(-3);
        productSetting.BetType.Should().Be(-4);
        productSetting.IsHidden.Should().BeTrue();
        productSetting.IsDisabled.Should().BeTrue();
    }

    [Theory]
    [InlineData(1, 1, 1, 1, false, false)]
    [InlineData(999, 888, 777, 666, true, true)]
    [InlineData(0, null, 0, 0, false, true)]
    public void ProductSetting_WithDifferentCombinations_ShouldSetCorrectly(
        int productId, int? groupId, int sportType, int betType, bool isHidden, bool isDisabled)
    {
        // Arrange & Act
        var productSetting = new ProductSetting
        {
            ProductId = productId,
            GroupId = groupId,
            SportType = sportType,
            BetType = betType,
            IsHidden = isHidden,
            IsDisabled = isDisabled
        };

        // Assert
        productSetting.ProductId.Should().Be(productId);
        productSetting.GroupId.Should().Be(groupId);
        productSetting.SportType.Should().Be(sportType);
        productSetting.BetType.Should().Be(betType);
        productSetting.IsHidden.Should().Be(isHidden);
        productSetting.IsDisabled.Should().Be(isDisabled);
    }
}
