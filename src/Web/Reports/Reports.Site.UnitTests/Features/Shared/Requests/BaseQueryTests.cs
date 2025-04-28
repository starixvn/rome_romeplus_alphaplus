using AlphaPlus.Reports.Site.Features.Shared.Requests;

namespace AlphaPlus.Reports.Site.UnitTests.Features.Shared.Requests;

[Trait("Category", "Shared")]
public class BaseQueryTests
{
    [Fact]
    public void Constructor_WithNoParams_ShouldInitializeWithDefaultValues()
    {
        // Act
        var userSession = GlobalMockUserSession.CreateMockUserSessionDefault();
        var query = new BaseQuery(userSession.Object);

        // Assert
        Assert.Equal(123, query.ViewerId);
        Assert.Equal(4, query.ViewerLevelId);
        Assert.NotNull(query.ViewerLevel);
        Assert.Equal("TestSuper", query.CustName);
        Assert.Equal(123, query.CustId);
        Assert.Equal(4, query.CustLevelId);
        Assert.NotNull(query.CustLevel);
        Assert.Equal(DateTime.MinValue, query.MinDate);
        Assert.True(query.IsFilterVisible);
    }

    [Fact]
    public void Constructor_WithUserSession_ShouldInitializeWithUserSessionValues()
    {
        // Arrange
        var limitDate = new DateTime(2023, 1, 1);
        var jackpotIds = new List<int> { 1, 2, 3 };
        var userSession = GlobalMockUserSession.CreateMockUserSession(
            123, "TestUser", 2, "testSite", 456, "en", limitDate);
        userSession.Setup(u => u.GetProperty<IList<int>>("GameEnable_Jackpot", null)).Returns(jackpotIds);

        // Act
        var query = new BaseQuery(userSession.Object);

        // Assert
        Assert.Equal(123, query.ViewerId);
        Assert.Equal(2, query.ViewerLevelId);
        Assert.Equal("testSite", query.Site);
        Assert.Equal(456, query.CurrencyId);
        Assert.Equal("en", query.Language);
        Assert.Equal(limitDate, query.LimitedDateToShowInfo);
        Assert.Equal(123, query.CustId);
        Assert.Equal("TestUser", query.CustName);
        Assert.Equal(2, query.CustLevelId);
        Assert.Equal(jackpotIds, query.EnableJackpotProductIds);
    }

    [Fact]
    public void EnableJackpotProductIds_WhenPropertyIsNull_ShouldReturnEmptyList()
    {
        // Arrange
        var userSession = GlobalMockUserSession.CreateMockUserSessionDefault();
        userSession.Setup(u => u.GetProperty<IList<int>>("GameEnable_Jackpot", null)).Returns((IList<int>)null);

        // Act
        var query = new BaseQuery(userSession.Object);

        // Assert
        Assert.NotNull(query.EnableJackpotProductIds);
        Assert.Empty(query.EnableJackpotProductIds);
    }

    [Theory]
    [InlineData(456, 3, true)]  // Same level
    [InlineData(456, 2, true)]  // Viewer is upline of customer
    [InlineData(456, 4, false)] // Customer is upline of viewer
    [InlineData(0, 3, false)] // Invalid CustId
    public void IsValid_ShouldValidateCorrectly(int custId, byte custLevelId, bool expected)
    {
        // Arrange
        var userSession = GlobalMockUserSession.CreateMockUserSession(
            123, "TestMaster", 3, "TestSite", 20, "en", DateTime.MinValue);
        var query = new BaseQuery(userSession.Object)
        {
            CustId = custId,
            CustLevelId = custLevelId
        };

        // Act
        var result = query.IsValid();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void CustLevel_ShouldReturnEnumerationFromCustLevelId()
    {
        // Arrange
        var userSession = GlobalMockUserSession.CreateMockUserSessionDefault();
        var query = new BaseQuery(userSession.Object)
        {
            CustLevelId = 2
        };

        // Act
        var result = query.CustLevel;

        // Assert
        Assert.Equal(2, result.Value);
    }
}