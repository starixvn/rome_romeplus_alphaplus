using UserModel = AlphaPlus.Settings.Site.Features.MemberInfo.Models.User;

namespace AlphaPlus.Settings.Site.UnitTests.Models;

public class UserTests
{
    private readonly Mock<IUserSession> _mockUserSession;

    public UserTests()
    {
        _mockUserSession = new Mock<IUserSession>();
        _mockUserSession.SetupGet(x => x.UserId).Returns(123);
        _mockUserSession.SetupGet(x => x.CustId).Returns(456);
        _mockUserSession.SetupGet(x => x.UserName).Returns("testuser");
        _mockUserSession.SetupGet(x => x.Site).Returns("TestSite");
        _mockUserSession.SetupGet(x => x.Language).Returns("en-US");
        _mockUserSession.SetupGet(x => x.LangKey).Returns("EN");
        _mockUserSession.SetupGet(x => x.RoleId).Returns((byte)2);
        _mockUserSession.SetupGet(x => x.SubAccId).Returns(789);
        _mockUserSession.SetupGet(x => x.SubUserName).Returns("subuser");
        _mockUserSession.SetupGet(x => x.UserLoginIp).Returns("192.168.1.1");
        _mockUserSession.SetupGet(x => x.SubPageAccess).Returns(new Mock<PageAccess>().Object);
        _mockUserSession.SetupGet(x => x.IsAllowedTotalBalanceTransfer).Returns(true);
        _mockUserSession.SetupGet(x => x.IsInternal).Returns(false);
        _mockUserSession.SetupGet(x => x.SRecommendId).Returns(101);
        _mockUserSession.SetupGet(x => x.IsIndirectLoggedIn).Returns(false);
        _mockUserSession.SetupGet(x => x.IsDoubleCommLosing).Returns(false);
        _mockUserSession.SetupGet(x => x.IsOfferProtectedMode).Returns(false);
        _mockUserSession.SetupGet(x => x.DarkModeName).Returns("light");
        _mockUserSession.SetupGet(x => x.AssetAppPath).Returns("/assets");
        _mockUserSession.SetupGet(x => x.AGIP).Returns("10.0.0.1");
        _mockUserSession.SetupGet(x => x.Currency).Returns("USD");
        _mockUserSession.SetupGet(x => x.IsSubAcc).Returns(false);
        _mockUserSession.SetupGet(x => x.CurrencyId).Returns(1);
        _mockUserSession.SetupGet(x => x.SiteDBGroupId).Returns(1);
        _mockUserSession.SetupGet(x => x.CustDBGroupId).Returns(1);
    }

    [Fact]
    public void User_Should_Initialize_With_Default_Constructor()
    {
        // Act
        var user = new UserModel();

        // Assert
        Assert.NotNull(user);
    }

    [Fact]
    public void User_Should_Initialize_With_UserSession()
    {
        // Act
        var user = new UserModel(_mockUserSession.Object);

        // Assert
        Assert.NotNull(user);
        Assert.Equal(123, user.LoggedInUserId);
        Assert.Equal(456, user.Id);
        Assert.Equal("testuser", user.Name);
    }

    [Fact]
    public void User_Should_Return_Correct_User_Properties()
    {
        // Arrange
        var user = new UserModel(_mockUserSession.Object);

        // Act & Assert
        Assert.Equal(123, user.LoggedInUserId);
        Assert.Equal(456, user.Id);
        Assert.Equal("testuser", user.Name);
        Assert.Equal("TestSite", user.Site);
        Assert.Equal("en-US", user.Language);
        Assert.Equal("EN", user.LanguageKey);
        Assert.Equal((byte)2, user.LevelId);
        Assert.Equal(789, user.SubAccId);
        Assert.Equal("subuser", user.SubAccName);
        Assert.Equal("192.168.1.1", user.IP);
        Assert.Equal("USD", user.Currency);
        Assert.Equal(1, user.CurrencyId);
        Assert.Equal("10.0.0.1", user.AGIP);
        Assert.Equal(1, user.SiteDBGroupId);
        Assert.Equal(1, user.CustDBGroupId);
    }

    [Fact]
    public void User_Should_Return_Correct_Boolean_Properties()
    {
        // Arrange
        var user = new UserModel(_mockUserSession.Object);

        // Act & Assert
        Assert.True(user.IsAllowedTotalBalanceTransfer);
        Assert.True(user.AllowedTotalBalanceTransfer);
        Assert.False(user.IsInternal);
        Assert.False(user.IsSubAcc);
        Assert.False(user.IsDoubleCommLosing);
        Assert.False(user.IsOfferProtectedMode);
    }

    [Fact]
    public void User_Should_Have_Correct_Constants()
    {
        // Assert
        Assert.Equal("Jackpot", GetPrivateConstant(typeof(UserModel), "Jackpot"));
        Assert.Equal("_GameEnable_", GetPrivateConstant(typeof(UserModel), "GameEnableSessionKey"));
        Assert.Equal("CustomerDateLimit", GetPrivateConstant(typeof(UserModel), "CustomerDatelimit"));
    }

    private static string GetPrivateConstant(Type type, string fieldName)
    {
        var field = type.GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        return (string)field?.GetValue(null);
    }
}
