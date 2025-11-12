namespace AlphaPlus.Auth.Site.UnitTests;

public class ModelTests
{
    [Fact]
    public void SiteDB_Properties_ShouldBeSetCorrectly()
    {
        // Arrange & Act
        var siteDB = new SiteDB
        {
            SiteDBGroupId = 1,
            CustDBGroupId = 2
        };

        // Assert
        siteDB.SiteDBGroupId.Should().Be(1);
        siteDB.CustDBGroupId.Should().Be(2);
    }

    [Fact]
    public void SignInParam_Properties_ShouldBeSetCorrectly()
    {
        // Arrange
        var requestInfo = new RequestInfo.Models.RequestInfo
        {
            LoginIp = "192.168.1.1",
            BrowserName = "Test Browser"
        };

        var loginTime = DateTime.Now;

        // Act
        var signInParam = new SignInParam
        {
            CustId = 123,
            Password = "testpassword",
            SubAccId = 456,
            Nickname = "testuser",
            IsNickname = true,
            LoginTime = loginTime,
            Language = "E",
            RequestInfo = requestInfo,
            DarkModeName = "dark",
            AssetAppPath = "/assets",
            DCSStaticAppPath = "/static"
        };

        // Assert
        signInParam.CustId.Should().Be(123);
        signInParam.Password.Should().Be("testpassword");
        signInParam.SubAccId.Should().Be(456);
        signInParam.Nickname.Should().Be("testuser");
        signInParam.IsNickname.Should().BeTrue();
        signInParam.LoginTime.Should().Be(loginTime);
        signInParam.Language.Should().Be("E");
        signInParam.RequestInfo.Should().Be(requestInfo);
        signInParam.DarkModeName.Should().Be("dark");
        signInParam.AssetAppPath.Should().Be("/assets");
        signInParam.DCSStaticAppPath.Should().Be("/static");
    }

    [Fact]
    public void SSOParam_Properties_ShouldBeSetCorrectly()
    {
        // Arrange
        var loginTime = DateTimeOffset.Now.ToUnixTimeSeconds();

        // Act
        var ssoParam = new SSOParam
        {
            CustId = 123,
            SubAccId = 456,
            LoginTime = loginTime,
            EncryptPassword = "encrypted_password",
            Nickname = "testuser",
            IsNickname = true,
            BrowserSize = "1920x1080",
            EncryptCode = "encrypted_code",
            DarkModeName = "dark",
            AssetAppPath = "/assets",
            DCSStaticAppPath = "/static"
        };

        // Assert
        ssoParam.CustId.Should().Be(123);
        ssoParam.SubAccId.Should().Be(456);
        ssoParam.LoginTime.Should().Be(loginTime);
        ssoParam.EncryptPassword.Should().Be("encrypted_password");
        ssoParam.Nickname.Should().Be("testuser");
        ssoParam.IsNickname.Should().BeTrue();
        ssoParam.BrowserSize.Should().Be("1920x1080");
        ssoParam.EncryptCode.Should().Be("encrypted_code");
        ssoParam.DarkModeName.Should().Be("dark");
        ssoParam.AssetAppPath.Should().Be("/assets");
        ssoParam.DCSStaticAppPath.Should().Be("/static");
    }

    [Fact]
    public void InternalLoginParam_Properties_ShouldBeSetCorrectly()
    {
        // Arrange
        var requestInfo = new AlphaPlus.RequestInfo.Models.RequestInfo
        {
            LoginIp = "192.168.1.1",
            BrowserName = "Test Browser"
        };

        var loginTime = DateTime.Now;

        // Act
        var internalLoginParam = new InternalLoginParam
        {
            AdminId = 123,
            Username = "admin",
            LoginTime = loginTime,
            Language = "E",
            RequestInfo = requestInfo
        };

        // Assert
        internalLoginParam.AdminId.Should().Be(123);
        internalLoginParam.Username.Should().Be("admin");
        internalLoginParam.LoginTime.Should().Be(loginTime);
        internalLoginParam.Language.Should().Be("E");
        internalLoginParam.RequestInfo.Should().Be(requestInfo);
    }

    [Fact]
    public void InternalLoginParam_DefaultLanguage_ShouldBeE()
    {
        // Arrange & Act
        var internalLoginParam = new InternalLoginParam();

        // Assert
        internalLoginParam.Language.Should().Be("E");
    }

    [Fact]
    public void IndirectLoginParam_Properties_ShouldBeSetCorrectly()
    {
        // Arrange
        var requestInfo = new AlphaPlus.RequestInfo.Models.RequestInfo
        {
            LoginIp = "192.168.1.1",
            BrowserName = "Test Browser"
        };

        var loginTime = DateTime.Now;

        // Act
        var indirectLoginParam = new IndirectLoginParam
        {
            CustId = 123,
            UplineUsername = "upline",
            UplineCustId = 456,
            UplineSubAccId = 789,
            SubAccId = 101,
            Username = "testuser",
            LoginTime = loginTime,
            Language = "E",
            RequestInfo = requestInfo,
            DarkModeName = "dark",
            AssetAppPath = "/assets"
        };

        // Assert
        indirectLoginParam.CustId.Should().Be(123);
        indirectLoginParam.UplineUsername.Should().Be("upline");
        indirectLoginParam.UplineCustId.Should().Be(456);
        indirectLoginParam.UplineSubAccId.Should().Be(789);
        indirectLoginParam.SubAccId.Should().Be(101);
        indirectLoginParam.Username.Should().Be("testuser");
        indirectLoginParam.LoginTime.Should().Be(loginTime);
        indirectLoginParam.Language.Should().Be("E");
        indirectLoginParam.RequestInfo.Should().Be(requestInfo);
        indirectLoginParam.DarkModeName.Should().Be("dark");
        indirectLoginParam.AssetAppPath.Should().Be("/assets");
    }

    [Fact]
    public void ReCheckInResult_Properties_ShouldBeSetCorrectly()
    {
        // Arrange & Act
        var reCheckInResult = new ReCheckInResult
        {
            IsKickedOut = true,
            KickOutReason = 1
        };

        // Assert
        reCheckInResult.IsKickedOut.Should().BeTrue();
        reCheckInResult.KickOutReason.Should().Be(1);
    }

    [Fact]
    public void ReCheckInResult_DefaultKickOutReason_ShouldBeZero()
    {
        // Arrange & Act
        var reCheckInResult = new ReCheckInResult();

        // Assert
        reCheckInResult.KickOutReason.Should().Be(0);
    }

    [Fact]
    public void ReCheckInResult_DefaultIsKickedOut_ShouldBeFalse()
    {
        // Arrange & Act
        var reCheckInResult = new ReCheckInResult();

        // Assert
        reCheckInResult.IsKickedOut.Should().BeFalse();
    }
}