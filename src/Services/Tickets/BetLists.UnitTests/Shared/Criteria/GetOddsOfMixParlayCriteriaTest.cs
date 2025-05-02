using AlphaPlus.BetLists.Shared.Criteria;

namespace AlphaPlus.BetLists.UnitTests.Shared.Criteria;

[Trait("Category", "BetLists")]
public class GetOddsOfMixParlayCriteriaTest
{
    [Fact]
    public void Constructor_Should_Set_TransIdList_Property()
    {
        // Arrange
        var transIds = "123,456,789";

        // Act
        var criteria = new GetOddsOfMixParlayCriteria(transIds);

        // Assert
        Assert.Equal(transIds, criteria.TransIdList);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Expected_Value()
    {
        // Arrange
        var criteria = new GetOddsOfMixParlayCriteria("123");

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_BetList_MixParlay_OddsSel", result);
    }

    [Theory]
    [InlineData("123,456", true)]
    [InlineData("", false)]
    [InlineData(null, false)]
    [InlineData("   ", false)]
    public void IsValid_Should_Return_Expected_Result(string transIds, bool expectedResult)
    {
        // Arrange
        var criteria = new GetOddsOfMixParlayCriteria(transIds);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expectedResult, result);
    }
}
