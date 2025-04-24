using AlphaPlus.Reports.Features.WhiteLabel;

namespace AlphaPlus.Reports.UnitTests.Features.WhiteLabel;

[Trait("Category", "WhiteLabel")]
public class GetWhiteLabelsCriteriaTest
{
    [Fact]
    public void Constructor_ShouldSetCustIdProperty_WhenProvided()
    {
        // Arrange
        var custId = 123;

        // Act
        var criteria = new GetWhiteLabelsCriteria(custId);

        // Assert
        Assert.Equal(custId, criteria.CustId);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new GetWhiteLabelsCriteria(1);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_WhiteLabelListSel", result);
    }

    [Fact]
    public void IsValid_ShouldReturnTrue_WhenCustIdIsGreaterThanZero()
    {
        // Arrange
        var criteria = new GetWhiteLabelsCriteria(1);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_WhenCustIdIsZero()
    {
        // Arrange
        var criteria = new GetWhiteLabelsCriteria(0);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_WhenCustIdIsNegative()
    {
        // Arrange
        var criteria = new GetWhiteLabelsCriteria(-1);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }
}