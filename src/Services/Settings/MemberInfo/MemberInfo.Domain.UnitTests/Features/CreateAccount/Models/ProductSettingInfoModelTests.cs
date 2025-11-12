using AlphaPlus.Settings.MemberInfo.Domain.Features.CreateAccount.Models;

namespace AlphaPlus.Settings.MemberInfo.Domain.UnitTests.Features.CreateAccount.Models;

public class ProductSettingInfoModelTests
{
    [Fact]
    public void ProductSettingInfoModel_Should_Initialize_With_Default_Values()
    {
        // Act
        var model = new ProductSettingInfoModel();

        // Assert
        Assert.Equal(0, model.GroupId);
        Assert.Equal(0, model.SubGroupId);
        Assert.Equal(0, model.ProductId);
        Assert.Equal(0, model.RealProductId);
    }

    [Fact]
    public void GroupId_Property_Should_Be_Settable()
    {
        // Arrange
        var model = new ProductSettingInfoModel();
        var groupId = 123;

        // Act
        model.GroupId = groupId;

        // Assert
        Assert.Equal(groupId, model.GroupId);
    }

    [Fact]
    public void SubGroupId_Property_Should_Be_Settable()
    {
        // Arrange
        var model = new ProductSettingInfoModel();
        var subGroupId = 456;

        // Act
        model.SubGroupId = subGroupId;

        // Assert
        Assert.Equal(subGroupId, model.SubGroupId);
    }

    [Fact]
    public void ProductId_Property_Should_Be_Settable()
    {
        // Arrange
        var model = new ProductSettingInfoModel();
        var productId = 789;

        // Act
        model.ProductId = productId;

        // Assert
        Assert.Equal(productId, model.ProductId);
    }

    [Fact]
    public void RealProductId_Property_Should_Be_Settable()
    {
        // Arrange
        var model = new ProductSettingInfoModel();
        var realProductId = 101;

        // Act
        model.RealProductId = realProductId;

        // Assert
        Assert.Equal(realProductId, model.RealProductId);
    }

    [Fact]
    public void All_Properties_Should_Be_Independent()
    {
        // Arrange
        var model = new ProductSettingInfoModel();

        // Act
        model.GroupId = 1;
        model.SubGroupId = 2;
        model.ProductId = 3;
        model.RealProductId = 4;

        // Assert
        Assert.Equal(1, model.GroupId);
        Assert.Equal(2, model.SubGroupId);
        Assert.Equal(3, model.ProductId);
        Assert.Equal(4, model.RealProductId);
    }

    [Theory]
    [InlineData(0, 0, 0, 0)]
    [InlineData(1, 2, 3, 4)]
    [InlineData(-1, -2, -3, -4)]
    [InlineData(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue)]
    [InlineData(int.MinValue, int.MinValue, int.MinValue, int.MinValue)]
    public void Properties_Should_Accept_Various_Values(int groupId, int subGroupId, int productId, int realProductId)
    {
        // Arrange
        var model = new ProductSettingInfoModel();

        // Act
        model.GroupId = groupId;
        model.SubGroupId = subGroupId;
        model.ProductId = productId;
        model.RealProductId = realProductId;

        // Assert
        Assert.Equal(groupId, model.GroupId);
        Assert.Equal(subGroupId, model.SubGroupId);
        Assert.Equal(productId, model.ProductId);
        Assert.Equal(realProductId, model.RealProductId);
    }

    [Fact]
    public void Multiple_Instances_Should_Be_Independent()
    {
        // Arrange & Act
        var model1 = new ProductSettingInfoModel
        {
            GroupId = 1,
            SubGroupId = 2,
            ProductId = 3,
            RealProductId = 4
        };

        var model2 = new ProductSettingInfoModel
        {
            GroupId = 10,
            SubGroupId = 20,
            ProductId = 30,
            RealProductId = 40
        };

        // Assert
        Assert.Equal(1, model1.GroupId);
        Assert.Equal(2, model1.SubGroupId);
        Assert.Equal(3, model1.ProductId);
        Assert.Equal(4, model1.RealProductId);

        Assert.Equal(10, model2.GroupId);
        Assert.Equal(20, model2.SubGroupId);
        Assert.Equal(30, model2.ProductId);
        Assert.Equal(40, model2.RealProductId);
    }

    [Fact]
    public void Properties_Should_Have_Getters_And_Setters()
    {
        // Arrange
        var type = typeof(ProductSettingInfoModel);

        // Act & Assert
        var properties = new[] { "GroupId", "SubGroupId", "ProductId", "RealProductId" };
        
        foreach (var propertyName in properties)
        {
            var property = type.GetProperty(propertyName);
            Assert.NotNull(property);
            Assert.True(property.CanRead, $"{propertyName} should be readable");
            Assert.True(property.CanWrite, $"{propertyName} should be writable");
            Assert.Equal(typeof(int), property.PropertyType);
        }
    }
}
