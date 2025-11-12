using AlphaPlus.Settings.MemberInfo.Domain.Params;

namespace AlphaPlus.Settings.MemberInfo.Domain.UnitTests.Params;

public class CheckUMParamTests
{
    [Fact]
    public void Constructor_Should_Set_Properties_Correctly()
    {
        // Arrange
        var custId = 123;
        var productId = (byte)1;
        var language = "en";

        // Act
        var param = new CheckUMParam(custId, productId, language);

        // Assert
        Assert.Equal(custId, param.CustId);
        Assert.Equal(productId, param.ProductId);
        Assert.Equal(language, param.Language);
    }

    [Fact]
    public void Constructor_With_Zero_CustId_Should_Work()
    {
        // Arrange
        var custId = 0;
        var productId = (byte)1;
        var language = "en";

        // Act
        var param = new CheckUMParam(custId, productId, language);

        // Assert
        Assert.Equal(custId, param.CustId);
        Assert.Equal(productId, param.ProductId);
        Assert.Equal(language, param.Language);
    }

    [Fact]
    public void Constructor_With_Null_Language_Should_Work()
    {
        // Arrange
        var custId = 123;
        var productId = (byte)1;
        string language = null;

        // Act
        var param = new CheckUMParam(custId, productId, language);

        // Assert
        Assert.Equal(custId, param.CustId);
        Assert.Equal(productId, param.ProductId);
        Assert.Null(param.Language);
    }

    [Fact]
    public void Constructor_With_Empty_Language_Should_Work()
    {
        // Arrange
        var custId = 123;
        var productId = (byte)1;
        var language = string.Empty;

        // Act
        var param = new CheckUMParam(custId, productId, language);

        // Assert
        Assert.Equal(custId, param.CustId);
        Assert.Equal(productId, param.ProductId);
        Assert.Equal(string.Empty, param.Language);
    }

    [Fact]
    public void Language_Property_Should_Be_Settable()
    {
        // Arrange
        var custId = 123;
        var productId = (byte)1;
        var initialLanguage = "en";
        var newLanguage = "vi";

        // Act
        var param = new CheckUMParam(custId, productId, initialLanguage);
        param.Language = newLanguage;

        // Assert
        Assert.Equal(newLanguage, param.Language);
    }

    [Fact]
    public void CustId_Property_Should_Be_ReadOnly()
    {
        // Arrange
        var custId = 123;
        var productId = (byte)1;
        var language = "en";

        // Act
        var param = new CheckUMParam(custId, productId, language);

        // Assert
        Assert.Equal(custId, param.CustId);
        
        // Verify property is read-only by checking it doesn't have a setter
        var custIdProperty = typeof(CheckUMParam).GetProperty(nameof(CheckUMParam.CustId));
        Assert.True(custIdProperty.CanRead);
        Assert.False(custIdProperty.CanWrite);
    }

    [Fact]
    public void ProductId_Property_Should_Be_ReadOnly()
    {
        // Arrange
        var custId = 123;
        var productId = (byte)1;
        var language = "en";

        // Act
        var param = new CheckUMParam(custId, productId, language);

        // Assert
        Assert.Equal(productId, param.ProductId);
        
        // Verify property is read-only
        var productIdProperty = typeof(CheckUMParam).GetProperty(nameof(CheckUMParam.ProductId));
        Assert.True(productIdProperty.CanRead);
        Assert.False(productIdProperty.CanWrite);
    }

    [Theory]
    [InlineData(0, 0, "")]
    [InlineData(1, 1, "en")]
    [InlineData(999, 255, "vietnamese")]
    [InlineData(-1, 0, null)]
    public void Constructor_With_Various_Values_Should_Work(int custId, byte productId, string language)
    {
        // Act
        var param = new CheckUMParam(custId, productId, language);

        // Assert
        Assert.Equal(custId, param.CustId);
        Assert.Equal(productId, param.ProductId);
        Assert.Equal(language, param.Language);
    }
}
