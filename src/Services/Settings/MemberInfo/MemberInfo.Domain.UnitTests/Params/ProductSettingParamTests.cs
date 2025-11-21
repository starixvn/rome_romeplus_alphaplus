using AlphaPlus.Settings.MemberInfo.Domain.Params;

namespace AlphaPlus.Settings.MemberInfo.Domain.UnitTests.Params;

public class ProductSettingParamTests
{
    [Fact]
    public void Constructor_Should_Set_Properties_Correctly()
    {
        // Arrange
        var productId = 123;
        var custId = 456;
        var custName = "testuser";

        // Act
        var param = new ProductSettingParam(productId, custId, custName);

        // Assert
        Assert.Equal(productId, param.ProductId);
        Assert.Equal(custId, param.CustId);
        Assert.Equal(custName, param.CustName);
    }

    [Fact]
    public void Constructor_With_Zero_Values_Should_Work()
    {
        // Arrange
        var productId = 0;
        var custId = 0;
        var custName = "user";

        // Act
        var param = new ProductSettingParam(productId, custId, custName);

        // Assert
        Assert.Equal(productId, param.ProductId);
        Assert.Equal(custId, param.CustId);
        Assert.Equal(custName, param.CustName);
    }

    [Fact]
    public void Constructor_With_Negative_Values_Should_Work()
    {
        // Arrange
        var productId = -1;
        var custId = -2;
        var custName = "user";

        // Act
        var param = new ProductSettingParam(productId, custId, custName);

        // Assert
        Assert.Equal(productId, param.ProductId);
        Assert.Equal(custId, param.CustId);
        Assert.Equal(custName, param.CustName);
    }

    [Fact]
    public void Constructor_With_Null_CustName_Should_Work()
    {
        // Arrange
        var productId = 123;
        var custId = 456;
        string custName = null;

        // Act
        var param = new ProductSettingParam(productId, custId, custName);

        // Assert
        Assert.Equal(productId, param.ProductId);
        Assert.Equal(custId, param.CustId);
        Assert.Null(param.CustName);
    }

    [Fact]
    public void Constructor_With_Empty_CustName_Should_Work()
    {
        // Arrange
        var productId = 123;
        var custId = 456;
        var custName = string.Empty;

        // Act
        var param = new ProductSettingParam(productId, custId, custName);

        // Assert
        Assert.Equal(productId, param.ProductId);
        Assert.Equal(custId, param.CustId);
        Assert.Equal(string.Empty, param.CustName);
    }

    [Fact]
    public void All_Properties_Should_Be_ReadOnly()
    {
        // Arrange
        var type = typeof(ProductSettingParam);

        // Act & Assert
        var properties = type.GetProperties();
        
        foreach (var property in properties)
        {
            Assert.True(property.CanRead, $"{property.Name} should be readable");
            Assert.False(property.CanWrite, $"{property.Name} should be read-only");
        }
    }

    [Theory]
    [InlineData(0, 0, "")]
    [InlineData(1, 1, "user1")]
    [InlineData(999, 888, "testuser")]
    [InlineData(-1, -2, null)]
    [InlineData(int.MaxValue, int.MaxValue, "maxuser")]
    [InlineData(int.MinValue, int.MinValue, "minuser")]
    public void Constructor_With_Various_Values_Should_Work(int productId, int custId, string custName)
    {
        // Act
        var param = new ProductSettingParam(productId, custId, custName);

        // Assert
        Assert.Equal(productId, param.ProductId);
        Assert.Equal(custId, param.CustId);
        Assert.Equal(custName, param.CustName);
    }

    [Fact]
    public void Constructor_Should_Not_Modify_Input_Values()
    {
        // Arrange
        var originalProductId = 123;
        var originalCustId = 456;
        var originalCustName = "testuser";
        var productId = originalProductId;
        var custId = originalCustId;
        var custName = originalCustName;

        // Act
        var param = new ProductSettingParam(productId, custId, custName);

        // Assert
        Assert.Equal(originalProductId, productId);
        Assert.Equal(originalCustId, custId);
        Assert.Equal(originalCustName, custName);
        Assert.Equal(originalProductId, param.ProductId);
        Assert.Equal(originalCustId, param.CustId);
        Assert.Equal(originalCustName, param.CustName);
    }

    [Fact]
    public void Multiple_Instances_Should_Be_Independent()
    {
        // Arrange & Act
        var param1 = new ProductSettingParam(1, 10, "user1");
        var param2 = new ProductSettingParam(2, 20, "user2");

        // Assert
        Assert.Equal(1, param1.ProductId);
        Assert.Equal(10, param1.CustId);
        Assert.Equal("user1", param1.CustName);
        
        Assert.Equal(2, param2.ProductId);
        Assert.Equal(20, param2.CustId);
        Assert.Equal("user2", param2.CustName);
    }

    [Fact]
    public void Constructor_With_Long_CustName_Should_Work()
    {
        // Arrange
        var productId = 123;
        var custId = 456;
        var custName = new string('a', 1000); // Very long string

        // Act
        var param = new ProductSettingParam(productId, custId, custName);

        // Assert
        Assert.Equal(productId, param.ProductId);
        Assert.Equal(custId, param.CustId);
        Assert.Equal(custName, param.CustName);
        Assert.Equal(1000, param.CustName.Length);
    }

    [Fact]
    public void Constructor_With_Special_Characters_In_CustName_Should_Work()
    {
        // Arrange
        var productId = 123;
        var custId = 456;
        var custName = "test@user!#$%^&*()_+{}|:<>?[];',./";

        // Act
        var param = new ProductSettingParam(productId, custId, custName);

        // Assert
        Assert.Equal(productId, param.ProductId);
        Assert.Equal(custId, param.CustId);
        Assert.Equal(custName, param.CustName);
    }
}
