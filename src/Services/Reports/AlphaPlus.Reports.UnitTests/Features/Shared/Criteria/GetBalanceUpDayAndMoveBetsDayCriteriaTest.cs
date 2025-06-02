using AlphaPlus.Reports.Features.Shared.Criteria;

namespace AlphaPlus.Reports.UnitTests.Features.Shared.Criteria;

[Trait("Category", "Shared")]
public class GetBalanceUpDayAndMoveBetsDayCriteriaTest
{
    private readonly GetBalanceUpDayAndMoveBetsDayCriteria criteria;

    public GetBalanceUpDayAndMoveBetsDayCriteriaTest()
    {
        criteria = new GetBalanceUpDayAndMoveBetsDayCriteria();
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_SelBalanceUpDay", result);
    }

    [Fact]
    public void IsValid_ShouldReturnTrue()
    {
        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }
}