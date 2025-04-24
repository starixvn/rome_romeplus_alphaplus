using AlphaPlus.Reports.Features.Statement;

namespace AlphaPlus.Reports.UnitTests.Features.Statement;

[Trait("Category", "Statement")]
public class GetStatementCriteriaTest
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        int custId = 123;
        var fromDate = new DateTime(2023, 1, 5);
        var toDate = new DateTime(2023, 1, 10);
        var storeMappingSuffix = "_TestSuffix";

        // Act
        var criteria = new GetStatementCriteria(custId, fromDate, toDate, storeMappingSuffix);

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(fromDate, criteria.FromDate);
        Assert.Equal(toDate, criteria.ToDate);
        Assert.Equal(storeMappingSuffix, criteria.StoreMappingSuffix);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var storeMappingSuffix = "_TestSuffix";
        var criteria = new GetStatementCriteria(123, DateTime.Now, DateTime.Now, storeMappingSuffix);
        var expectedKey = $"Statement_Report_GetList{storeMappingSuffix}";

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, result);
    }

    [Fact]
    public void IsValid_WhenValidParameters_ReturnsTrue()
    {
        // Arrange
        var criteria = new GetStatementCriteria(123, new DateTime(2023, 1, 1), new DateTime(2023, 1, 2), "_Test");

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValid_WhenCustIdIsZero_ReturnsFalse()
    {
        // Arrange
        var criteria = new GetStatementCriteria(0, new DateTime(2023, 1, 1), new DateTime(2023, 1, 2), "_Test");

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_WhenFromDateIsAfterToDate_ReturnsFalse()
    {
        // Arrange
        var criteria = new GetStatementCriteria(123, new DateTime(2023, 1, 2), new DateTime(2023, 1, 1), "_Test");

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }
}