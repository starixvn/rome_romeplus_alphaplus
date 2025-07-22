using AlphaPlus.CashService.API.Features.Shared.Criteria;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Shared.Criteria;

public class GetSetupCheckListCriteriaTests
{
    [Fact]
    public void Constructor_WhenGivenValidCustId_SetsCustIdProperty()
    {
        // Arrange
        const int expectedCustId = 12345;

        // Act
        var criteria = new GetSetupCheckListCriteria(expectedCustId);

        // Assert
        Assert.Equal(expectedCustId, criteria.CustId);
    }

    [Fact]
    public void Constructor_WhenGivenZeroCustId_SetsCustIdProperty()
    {
        // Arrange
        const int expectedCustId = 0;

        // Act
        var criteria = new GetSetupCheckListCriteria(expectedCustId);

        // Assert
        Assert.Equal(expectedCustId, criteria.CustId);
    }

    [Fact]
    public void Constructor_WhenGivenNegativeCustId_SetsCustIdProperty()
    {
        // Arrange
        const int expectedCustId = -1;

        // Act
        var criteria = new GetSetupCheckListCriteria(expectedCustId);

        // Assert
        Assert.Equal(expectedCustId, criteria.CustId);
    }

    [Fact]
    public void GetSettingKey_WhenCalled_ReturnsAgeCashServiceConfigureSignUpChecklist()
    {
        // Arrange
        var criteria = new GetSetupCheckListCriteria(12345);
        const string expectedKey = "Age_CashService_ConfigureSignUpChecklist";

        // Act
        var actualKey = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, actualKey);
    }

    [Fact]
    public void IsValid_WhenCustIdIsPositive_ReturnsTrue()
    {
        // Arrange
        var criteria = new GetSetupCheckListCriteria(12345);

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void IsValid_WhenCustIdIsZero_ReturnsFalse()
    {
        // Arrange
        var criteria = new GetSetupCheckListCriteria(0);

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void IsValid_WhenCustIdIsNegative_ReturnsFalse()
    {
        // Arrange
        var criteria = new GetSetupCheckListCriteria(-1);

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.False(isValid);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(999999)]
    public void IsValid_WhenCustIdIsAnyPositiveValue_ReturnsTrue(int custId)
    {
        // Arrange
        var criteria = new GetSetupCheckListCriteria(custId);

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.True(isValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void IsValid_WhenCustIdIsZeroOrNegative_ReturnsFalse(int custId)
    {
        // Arrange
        var criteria = new GetSetupCheckListCriteria(custId);

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.False(isValid);
    }
} 