using AlphaPlus.Reports.Features.Shared.Criteria;

namespace AlphaPlus.Reports.UnitTests.Features.Shared.Criteria;

[Trait("Category", "Shared")]
public class GetUMInfoListCriteriaTest
{
    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new GetUMInfoListCriteria("TestSite");

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Acc_CheckUM_ByReport", result);
    }

    [Fact]
    public void IsValid_WithValidSiteName_ShouldReturnTrue()
    {
        // Arrange
        var criteria = new GetUMInfoListCriteria("TestSite");

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValid_WithNullSiteName_ShouldReturnFalse()
    {
        // Arrange
        var criteria = new GetUMInfoListCriteria(null);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_WithEmptySiteName_ShouldReturnFalse()
    {
        // Arrange
        var criteria = new GetUMInfoListCriteria(string.Empty);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_WithWhitespaceSiteName_ShouldReturnFalse()
    {
        // Arrange
        var criteria = new GetUMInfoListCriteria("   ");

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Constructor_ShouldSetSiteName()
    {
        // Arrange & Act
        var siteName = "TestSite";
        var criteria = new GetUMInfoListCriteria(siteName);

        // Assert
        Assert.Equal(siteName, criteria.SiteName);
    }
}