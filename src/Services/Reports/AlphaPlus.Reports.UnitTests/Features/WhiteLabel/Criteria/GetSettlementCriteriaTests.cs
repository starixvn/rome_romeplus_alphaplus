using System.Data.SqlTypes;
using AlphaPlus.Reports.Features.WhiteLabel;

namespace AlphaPlus.Reports.UnitTests.Features.WhiteLabel;

[Trait("Category", "WhiteLabel")]
public class GetSettlementCriteriaTests
{
    [Fact]
    public void Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        int custId = 123;
        var fromDate = new DateTime(2023, 1, 1);
        var toDate = new DateTime(2023, 1, 10);
        string whiteLabels = "Label1,Label2";
        string username = "testuser";

        // Act
        var criteria = new GetSettlementCriteria(
            custId,
            fromDate,
            toDate,
            whiteLabels,
            username);

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(fromDate, criteria.FromDate);
        Assert.Equal(toDate, criteria.ToDate);
        Assert.Equal(whiteLabels, criteria.WhiteLabelList);
        Assert.Equal(username, criteria.Username);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new GetSettlementCriteria(1, DateTime.Now, DateTime.Now, string.Empty, string.Empty);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_Rpt_WhiteLabelSettlement", result);
    }

    [Theory]
    [InlineData(1, true)]  // Valid CustId
    [InlineData(0, false)] // Invalid CustId (0)
    [InlineData(-1, false)] // Invalid CustId (negative)
    public void IsValid_ShouldValidateCustId(int custId, bool expected)
    {
        // Arrange
        var fromDate = new DateTime(2023, 1, 1);
        var toDate = new DateTime(2023, 1, 10);
        var criteria = new GetSettlementCriteria(
            custId,
            fromDate,
            toDate,
            string.Empty,
            string.Empty);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_WhenFromDateIsAfterToDate()
    {
        // Arrange
        var fromDate = new DateTime(2023, 1, 10);
        var toDate = new DateTime(2023, 1, 1);
        var criteria = new GetSettlementCriteria(
            1,
            fromDate,
            toDate,
            string.Empty,
            string.Empty);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_WhenFromDateIsLessThanSqlMinValue()
    {
        // Arrange
        var fromDate = SqlDateTime.MinValue.Value.AddDays(-1);
        var toDate = DateTime.Now;
        var criteria = new GetSettlementCriteria(
            1,
            fromDate,
            toDate,
            string.Empty,
            string.Empty);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_ShouldReturnTrue_WhenAllParametersAreValid()
    {
        // Arrange
        int custId = 123;
        var fromDate = new DateTime(2023, 1, 1);
        var toDate = new DateTime(2023, 1, 10);
        string whiteLabels = "Label1,Label2";
        string username = "testuser";

        var criteria = new GetSettlementCriteria(
            custId,
            fromDate,
            toDate,
            whiteLabels,
            username);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }
}