using Xunit;
using FluentAssertions;
using Moq;
using AlphaPlus.Core.Models;
using AlphaPlus.RequestInfo.Models;

namespace AlphaPlus.Auth.Site.UnitTests;

public class AuthSiteTests
{
    [Fact]
    public void SignInResult_ShouldHaveCorrectProperties()
    {
        // Arrange
        var signInResult = new AlphaPlus.Auth.Site.Features.Account.SignInResult
        {
            CustId = 123,
            UserName = "TestUser",
            SubAccId = 1,
            RoleId = 1,
            Site = "TestSite",
            Currency = "USD",
            IsFirstLogin = false
        };

        // Act & Assert
        signInResult.CustId.Should().Be(123);
        signInResult.UserName.Should().Be("TestUser");
        signInResult.SubAccId.Should().Be(1);
        signInResult.RoleId.Should().Be(1);
        signInResult.Site.Should().Be("TestSite");
        signInResult.Currency.Should().Be("USD");
        signInResult.IsFirstLogin.Should().BeFalse();
    }

    [Fact]
    public void ReCheckInResult_ShouldHaveCorrectProperties()
    {
        // Arrange
        var reCheckInResult = new AlphaPlus.Auth.Site.Features.Account.ReCheckInResult
        {
            IsKickedOut = false,
            KickOutReason = 0
        };

        // Act & Assert
        reCheckInResult.IsKickedOut.Should().BeFalse();
        reCheckInResult.KickOutReason.Should().Be(0);
    }

    [Fact]
    public void SSOParam_ShouldHaveCorrectProperties()
    {
        // Arrange
        var ssoParam = new AlphaPlus.Auth.Site.Features.Account.SSOParam
        {
            CustId = 123,
            EncryptPassword = "encrypted-password",
            EncryptCode = "encrypt-code",
            SubAccId = 1,
            Nickname = "TestUser",
            IsNickname = false,
            LoginTime = DateTime.Now.Ticks
        };

        // Act & Assert
        ssoParam.CustId.Should().Be(123);
        ssoParam.EncryptPassword.Should().Be("encrypted-password");
        ssoParam.EncryptCode.Should().Be("encrypt-code");
        ssoParam.SubAccId.Should().Be(1);
        ssoParam.Nickname.Should().Be("TestUser");
        ssoParam.IsNickname.Should().BeFalse();
    }

    [Fact]
    public void InternalLoginParam_ShouldHaveCorrectProperties()
    {
        // Arrange
        var internalLoginParam = new AlphaPlus.Auth.Site.Features.Account.InternalLoginParam
        {
            AdminId = 1,
            Language = "en-US",
            Username = "admin",
            LoginTime = DateTime.Now
        };

        // Act & Assert
        internalLoginParam.AdminId.Should().Be(1);
        internalLoginParam.Language.Should().Be("en-US");
        internalLoginParam.Username.Should().Be("admin");
    }

    [Fact]
    public void IndirectLoginParam_ShouldHaveCorrectProperties()
    {
        // Arrange
        var indirectLoginParam = new AlphaPlus.Auth.Site.Features.Account.IndirectLoginParam
        {
            CustId = 123,
            SubAccId = 1,
            Username = "TestUser",
            LoginTime = DateTime.Now,
            Language = "en-US"
        };

        // Act & Assert
        indirectLoginParam.CustId.Should().Be(123);
        indirectLoginParam.SubAccId.Should().Be(1);
        indirectLoginParam.Username.Should().Be("TestUser");
        indirectLoginParam.Language.Should().Be("en-US");
    }

    [Fact]
    public void RequestInfo_ShouldHaveCorrectProperties()
    {
        // Arrange
        var requestInfo = new AlphaPlus.RequestInfo.Models.RequestInfo
        {
            LoginIp = "127.0.0.1",
            CountryCode = "US",
            LoginDomain = "test.com",
            BrowserName = "Chrome",
            Platform = "Windows"
        };

        // Act & Assert
        requestInfo.LoginIp.Should().Be("127.0.0.1");
        requestInfo.CountryCode.Should().Be("US");
        requestInfo.LoginDomain.Should().Be("test.com");
        requestInfo.BrowserName.Should().Be("Chrome");
        requestInfo.Platform.Should().Be("Windows");
    }

    [Theory]
    [InlineData(1, "Member")]
    [InlineData(2, "Agent")]
    [InlineData(3, "Master")]
    [InlineData(4, "Super")]
    public void RoleId_ShouldHaveDifferentValues(int roleId, string expectedRole)
    {
        // Arrange
        var signInResult = new AlphaPlus.Auth.Site.Features.Account.SignInResult
        {
            RoleId = (byte)roleId,
            UserName = expectedRole
        };

        // Act & Assert
        signInResult.RoleId.Should().Be((byte)roleId);
        signInResult.UserName.Should().Be(expectedRole);
    }

    [Fact]
    public void SignInParam_ShouldHaveCorrectProperties()
    {
        // Arrange
        var signInParam = new AlphaPlus.Auth.Site.Features.Account.SignInParam
        {
            CustId = 123,
            Password = "password123",
            SubAccId = 1,
            Nickname = "TestUser",
            IsNickname = false,
            LoginTime = DateTime.Now,
            Language = "en-US"
        };

        // Act & Assert
        signInParam.CustId.Should().Be(123);
        signInParam.Password.Should().Be("password123");
        signInParam.SubAccId.Should().Be(1);
        signInParam.Nickname.Should().Be("TestUser");
        signInParam.IsNickname.Should().BeFalse();
        signInParam.Language.Should().Be("en-US");
    }

    [Fact]
    public void SiteDB_ShouldHaveCorrectProperties()
    {
        // Arrange
        var siteDB = new AlphaPlus.Auth.Site.Features.Account.SiteDB
        {
            AGIP = "agip-value",
            SiteDBGroupId = 1,
            CustDBGroupId = 2
        };

        // Act & Assert
        siteDB.AGIP.Should().Be("agip-value");
        siteDB.SiteDBGroupId.Should().Be(1);
        siteDB.CustDBGroupId.Should().Be(2);
    }
} 