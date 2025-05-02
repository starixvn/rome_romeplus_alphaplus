using AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;
using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.UnitTests.Forecast.Criteria;

[Trait("Category", "BetsForecast")]
public class GetScoreMapBetTypeCriteriaTest
{
    private readonly GetScoreMapBetTypeCriteria _criteria;

    public GetScoreMapBetTypeCriteriaTest()
    {
        _criteria = new GetScoreMapBetTypeCriteria();
    }

    [Fact]
    public void GetSettingKey_Should_Return_Correct_Key()
    {
        // Act
        var result = _criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_ScoreMap_SelBettype", result);
    }

    [Fact]
    public void IsValid_Should_Always_Return_True()
    {
        // Act
        var result = _criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Should_Inherit_From_CriteriaBase()
    {
        // Assert
        Assert.True(_criteria is CriteriaBase);
    }
}
