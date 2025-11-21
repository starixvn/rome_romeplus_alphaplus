using AlphaPlus.Settings.MemberInfo.Domain.Models;

namespace AlphaPlus.Settings.MemberInfo.Domain.UnitTests.Models;

public class ProductTests
{
    [Fact]
    public void Product_Should_Initialize_With_Default_Values()
    {
        // Act
        var product = new Product();

        // Assert
        Assert.Equal(0, product.Id);
        Assert.Null(product.Name);
        Assert.False(product.IsDisable);
        Assert.Null(product.EndOfSupportDate);
        Assert.Equal(0, product.OrderNo);
        Assert.Equal(0, product.Type);
        Assert.Equal(0, product.GroupId);
    }

    [Fact]
    public void Product_Should_Set_Readable_Properties()
    {
        // Arrange
        var product = new Product();

        // Act
        product.Name = "Test Product";

        // Assert
        Assert.Equal("Test Product", product.Name);
    }

    [Fact]
    public void Product_Should_Have_Init_Only_Properties()
    {
        // Arrange
        var endOfSupportDate = new DateTime(2024, 12, 31);

        // Act
        var product = new Product
        {
            Id = 1,
            Name = "Sportsbook",
            IsDisable = false,
            EndOfSupportDate = endOfSupportDate,
            OrderNo = 1,
            Type = 1,
            GroupId = 1
        };

        // Assert
        Assert.Equal(1, product.Id);
        Assert.Equal("Sportsbook", product.Name);
        Assert.False(product.IsDisable);
        Assert.Equal(endOfSupportDate, product.EndOfSupportDate);
        Assert.Equal(1, product.OrderNo);
        Assert.Equal(1, product.Type);
        Assert.Equal(1, product.GroupId);
    }
}
