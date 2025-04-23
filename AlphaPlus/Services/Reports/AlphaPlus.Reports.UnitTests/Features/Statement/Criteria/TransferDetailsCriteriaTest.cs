using AlphaPlus.Reports.Features.Statement;

namespace AlphaPlus.Reports.UnitTests.Features.Statement;

[Trait("Category", "Statement")]
public class TransferDetailsCriteriaTest
{
    [Fact]
    public void Constructor_ShouldInitializeProperties_WithCorrectValues()
    {
        // Arrange
        var custId = 123;
        var transferDate = new DateTime(2023, 1, 15);
        var storeMappingSuffix = "_Warehouse";

        // Act
        var criteria = new TransferDetailsCriteria(custId, transferDate, storeMappingSuffix);

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(transferDate, criteria.TransferDate);
        Assert.Equal(storeMappingSuffix, criteria.StoreMappingSuffix);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey_WithSuffix()
    {
        // Arrange
        var custId = 123;
        var transferDate = new DateTime(2023, 1, 15);
        var storeMappingSuffix = "_Warehouse";
        var criteria = new TransferDetailsCriteria(custId, transferDate, storeMappingSuffix);
        var expectedKey = "Statement_Report_TransferDetails_Warehouse";

        // Act
        var actualKey = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, actualKey);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey_WithEmptySuffix()
    {
        // Arrange
        var custId = 123;
        var transferDate = new DateTime(2023, 1, 15);
        var storeMappingSuffix = string.Empty;
        var criteria = new TransferDetailsCriteria(custId, transferDate, storeMappingSuffix);
        var expectedKey = "Statement_Report_TransferDetails";

        // Act
        var actualKey = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, actualKey);
    }

    [Fact]
    public void IsValid_ShouldReturnTrue_WhenCustIdGreaterThanZero()
    {
        // Arrange
        var custId = 123;
        var transferDate = new DateTime(2023, 1, 15);
        var storeMappingSuffix = "_Warehouse";
        var criteria = new TransferDetailsCriteria(custId, transferDate, storeMappingSuffix);

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_WhenCustIdIsZero()
    {
        // Arrange
        var custId = 0;
        var transferDate = new DateTime(2023, 1, 15);
        var storeMappingSuffix = "_Warehouse";
        var criteria = new TransferDetailsCriteria(custId, transferDate, storeMappingSuffix);

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_WhenCustIdIsNegative()
    {
        // Arrange
        var custId = -1;
        var transferDate = new DateTime(2023, 1, 15);
        var storeMappingSuffix = "_Warehouse";
        var criteria = new TransferDetailsCriteria(custId, transferDate, storeMappingSuffix);

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.False(isValid);
    }
}