using AlphaPlus.BetLists.Shared.Criteria;

namespace AlphaPlus.BetLists.UnitTests.Shared.Criteria;

[Trait("Category", "BetLists")]
public class GetBalanceUpDayAndMoveBetsDayCriteriaTest
{
    private readonly GetBalanceUpDayAndMoveBetsDayCriteria _criteria;

    public GetBalanceUpDayAndMoveBetsDayCriteriaTest()
    {
        _criteria = new GetBalanceUpDayAndMoveBetsDayCriteria();
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Act
        var result = _criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_SelBalanceUpDay", result);
    }

    [Fact]
    public void IsValid_ShouldAlwaysReturnTrue()
    {
        // Act
        var result = _criteria.IsValid();

        // Assert
        Assert.True(result);
    }
}