using AlphaPlus.BetLists.Shared.Criteria;

namespace AlphaPlus.BetLists.UnitTests.Shared.Criteria;

[Trait("Category", "BetLists")]
public class GetIPV6InfoCriteriaTest
{
    [Fact]
    public void Constructor_Should_Set_IPList_Property()
    {
        // Arrange
        var ipList = "2001:db8::1,2001:db8::2";

        // Act
        var criteria = new GetIPV6InfoCriteria(ipList);

        // Assert
        Assert.Equal(ipList, criteria.IPList);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Correct_Key()
    {
        // Arrange
        var criteria = new GetIPV6InfoCriteria("2001:db8::1");

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_IPRangesByCountrySelByIPV6List", result);
    }

    [Fact]
    public void IsValid_Should_Return_True_When_IPList_Is_Not_Empty()
    {
        // Arrange
        var criteria = new GetIPV6InfoCriteria("2001:db8::1");

        // Act & Assert
        Assert.True(criteria.IsValid());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void IsValid_Should_Return_False_When_IPList_Is_Empty_Or_Whitespace(string ipList)
    {
        // Arrange
        var criteria = new GetIPV6InfoCriteria(ipList);

        // Act & Assert
        Assert.False(criteria.IsValid());
    }
}