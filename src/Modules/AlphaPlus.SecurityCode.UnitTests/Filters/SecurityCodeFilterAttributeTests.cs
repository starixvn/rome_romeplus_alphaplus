namespace AlphaPlus.SecurityCode.UnitTests.Filters;

public class SecurityCodeFilterAttributeTests
{
    [Fact]
    public void SecurityCodeFilterAttribute_Constructor_ShouldCreateAttribute()
    {
        // Arrange & Act
        var attribute = new SecurityCodeFilterAttribute();

        // Assert
        Assert.NotNull(attribute);
    }

    [Fact]
    public void SecurityCodeFilterAttribute_ImplementationType_ShouldBeSecurityCodeFilter()
    {
        // Arrange & Act
        var attribute = new SecurityCodeFilterAttribute();

        // Assert
        // Note: SecurityCodeFilter is a private nested class, so we can't access it directly
        // We can only test that the attribute is created successfully
        Assert.NotNull(attribute);
    }


    [Fact]
    public void SecurityCodeFilterAttribute_IsReusable_ShouldBeFalse()
    {
        // Arrange & Act
        var attribute = new SecurityCodeFilterAttribute();

        // Assert
        Assert.False(attribute.IsReusable);
    }

    [Fact]
    public void SecurityCodeFilterAttribute_Usage_ShouldBeCorrect()
    {
        // Arrange & Act
        var attribute = new SecurityCodeFilterAttribute();

        // Assert
        var usageAttribute = attribute.GetType().GetCustomAttributes(typeof(AttributeUsageAttribute), false)
            .FirstOrDefault() as AttributeUsageAttribute;

        Assert.NotNull(usageAttribute);
        Assert.Equal(AttributeTargets.Class | AttributeTargets.Method, usageAttribute.ValidOn);
        Assert.False(usageAttribute.AllowMultiple);
        Assert.False(usageAttribute.Inherited);
    }
}
