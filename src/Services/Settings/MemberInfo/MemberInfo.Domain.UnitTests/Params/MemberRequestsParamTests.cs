using AlphaPlus.Settings.MemberInfo.Domain.Params;

namespace AlphaPlus.Settings.MemberInfo.Domain.UnitTests.Params;

public class MemberRequestsParamTests
{
    [Fact]
    public void Constructor_Should_Set_Properties_Correctly()
    {
        // Arrange
        var custId = 123;
        var productId = 456;

        // Act
        var param = new MemberRequestsParam(custId, productId);

        // Assert
        Assert.Equal(custId, param.CustId);
        Assert.Equal(productId, param.ProductId);
    }

    [Fact]
    public void Constructor_With_Zero_Values_Should_Work()
    {
        // Arrange
        var custId = 0;
        var productId = 0;

        // Act
        var param = new MemberRequestsParam(custId, productId);

        // Assert
        Assert.Equal(custId, param.CustId);
        Assert.Equal(productId, param.ProductId);
    }

    [Fact]
    public void Constructor_With_Negative_Values_Should_Work()
    {
        // Arrange
        var custId = -1;
        var productId = -1;

        // Act
        var param = new MemberRequestsParam(custId, productId);

        // Assert
        Assert.Equal(custId, param.CustId);
        Assert.Equal(productId, param.ProductId);
    }

    [Fact]
    public void CustId_Property_Should_Be_ReadOnly()
    {
        // Arrange
        var custId = 123;
        var productId = 456;

        // Act
        var param = new MemberRequestsParam(custId, productId);

        // Assert
        Assert.Equal(custId, param.CustId);
        
        // Verify property is read-only
        var custIdProperty = typeof(MemberRequestsParam).GetProperty(nameof(MemberRequestsParam.CustId));
        Assert.True(custIdProperty.CanRead);
        Assert.False(custIdProperty.CanWrite);
    }

    [Fact]
    public void ProductId_Property_Should_Be_ReadOnly()
    {
        // Arrange
        var custId = 123;
        var productId = 456;

        // Act
        var param = new MemberRequestsParam(custId, productId);

        // Assert
        Assert.Equal(productId, param.ProductId);
        
        // Verify property is read-only
        var productIdProperty = typeof(MemberRequestsParam).GetProperty(nameof(MemberRequestsParam.ProductId));
        Assert.True(productIdProperty.CanRead);
        Assert.False(productIdProperty.CanWrite);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(999, 888)]
    [InlineData(-1, -2)]
    [InlineData(int.MaxValue, int.MaxValue)]
    [InlineData(int.MinValue, int.MinValue)]
    public void Constructor_With_Various_Values_Should_Work(int custId, int productId)
    {
        // Act
        var param = new MemberRequestsParam(custId, productId);

        // Assert
        Assert.Equal(custId, param.CustId);
        Assert.Equal(productId, param.ProductId);
    }

    [Fact]
    public void Constructor_Should_Not_Modify_Input_Values()
    {
        // Arrange
        var originalCustId = 123;
        var originalProductId = 456;
        var custId = originalCustId;
        var productId = originalProductId;

        // Act
        var param = new MemberRequestsParam(custId, productId);

        // Assert
        Assert.Equal(originalCustId, custId);
        Assert.Equal(originalProductId, productId);
        Assert.Equal(originalCustId, param.CustId);
        Assert.Equal(originalProductId, param.ProductId);
    }

    [Fact]
    public void Multiple_Instances_Should_Be_Independent()
    {
        // Arrange & Act
        var param1 = new MemberRequestsParam(1, 10);
        var param2 = new MemberRequestsParam(2, 20);

        // Assert
        Assert.Equal(1, param1.CustId);
        Assert.Equal(10, param1.ProductId);
        Assert.Equal(2, param2.CustId);
        Assert.Equal(20, param2.ProductId);
    }
}
