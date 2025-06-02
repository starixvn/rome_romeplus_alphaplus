using AlphaPlus.Reports.Features.Shared.Criteria;

namespace AlphaPlus.Reports.UnitTests.Features.Shared.Criteria;

[Trait("Category", "Shared")]
public class ReportProductCriteriaTest
{
    private class ConcreteReportProductCriteria : ReportProductCriteria
    {
        public ConcreteReportProductCriteria(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            DateTime balanceUpDay,
            DateTime moveBetsDay,
            string productIds) : base(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds)
        {
        }

        override public string GetSettingKey() => "TestSettingKey";
    }

    [Fact]
    public void Constructor_ShouldInitializeProperties_WhenCalledWithValidParameters()
    {
        // Arrange
        int custId = 123;
        DateTime fromDate = new DateTime(2023, 1, 1);
        DateTime toDate = new DateTime(2023, 1, 31);
        DateTime balanceUpDay = new DateTime(2023, 1, 15);
        DateTime moveBetsDay = new DateTime(2023, 1, 20);
        string productIds = "1,2,3";

        // Act
        var criteria = new ConcreteReportProductCriteria(
            custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds);

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(fromDate, criteria.FromDate);
        Assert.Equal(toDate, criteria.ToDate);
        Assert.Equal(balanceUpDay, criteria.BalanceUpDay);
        Assert.Equal(moveBetsDay, criteria.MoveBetsDay);
        Assert.Equal(productIds, criteria.ProductIds);
    }

    [Fact]
    public void IsValid_ShouldReturnTrue_WhenAllParametersAreValid()
    {
        // Arrange
        var criteria = new ConcreteReportProductCriteria(
            123,
            new DateTime(2023, 1, 1),
            new DateTime(2023, 1, 31),
            new DateTime(2023, 1, 15),
            new DateTime(2023, 1, 20),
            "1,2,3");

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_WhenProductIdsIsNull()
    {
        // Arrange
        var criteria = new ConcreteReportProductCriteria(
            123,
            new DateTime(2023, 1, 1),
            new DateTime(2023, 1, 31),
            new DateTime(2023, 1, 15),
            new DateTime(2023, 1, 20),
            null);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_WhenProductIdsIsEmpty()
    {
        // Arrange
        var criteria = new ConcreteReportProductCriteria(
            123,
            new DateTime(2023, 1, 1),
            new DateTime(2023, 1, 31),
            new DateTime(2023, 1, 15),
            new DateTime(2023, 1, 20),
            string.Empty);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_WhenProductIdsIsWhitespace()
    {
        // Arrange
        var criteria = new ConcreteReportProductCriteria(
            123,
            new DateTime(2023, 1, 1),
            new DateTime(2023, 1, 31),
            new DateTime(2023, 1, 15),
            new DateTime(2023, 1, 20),
            "  ");

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_WhenCustIdIsInvalid()
    {
        // Arrange
        var criteria = new ConcreteReportProductCriteria(
            0,
            new DateTime(2023, 1, 1),
            new DateTime(2023, 1, 31),
            new DateTime(2023, 1, 15),
            new DateTime(2023, 1, 20),
            "1,2,3");

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_WhenFromDateIsAfterToDate()
    {
        // Arrange
        var criteria = new ConcreteReportProductCriteria(
            123,
            new DateTime(2023, 1, 31),
            new DateTime(2023, 1, 1),
            new DateTime(2023, 1, 15),
            new DateTime(2023, 1, 20),
            "1,2,3");

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }
}