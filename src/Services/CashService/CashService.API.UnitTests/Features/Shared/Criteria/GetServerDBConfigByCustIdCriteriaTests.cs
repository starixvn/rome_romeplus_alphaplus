using AlphaPlus.CashService.API.Features.Shared.Criteria;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Shared.Criteria;

public class GetServerDBConfigByCustIdCriteriaTests
{
    [Fact]
    public void GetSettingKey_WhenCalled_ReturnsCorrectKey()
    {
        // Arrange
        var criteria = new GetServerDBConfigByCustIdCriteria();

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Acc_ServerDBConfigSelByCust", result);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    public void IsValid_WhenCustIdProvided_ReturnsExpectedResult(int custId, bool expectedResult)
    {
        // Arrange
        var criteria = new GetServerDBConfigByCustIdCriteria
        {
            CustId = custId
        };

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expectedResult, result);
    }
} 