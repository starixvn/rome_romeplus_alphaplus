using AlphaPlus.Core.Models;
using FluentAssertions;
using Xunit;

namespace AlphaPlus.Auth.Site.UnitTests;

public class BasicServiceTests
{
    [Fact]
    public void PageAccess_ShouldBeCreated()
    {
        // Arrange & Act
        var pageAccess = new PageAccess();

        // Assert
        pageAccess.Should().NotBeNull();
    }

    [Fact]
    public void SiteDB_ShouldBeCreated()
    {
        // Arrange & Act
        var siteDB = new AlphaPlus.Auth.Site.Features.Account.SiteDB();

        // Assert
        siteDB.Should().NotBeNull();
    }

    [Fact]
    public void SignInResult_ShouldBeCreated()
    {
        // Arrange & Act
        var signInResult = new AlphaPlus.Auth.Site.Features.Account.SignInResult();

        // Assert
        signInResult.Should().NotBeNull();
    }

    [Fact]
    public void ReCheckInResult_ShouldBeCreated()
    {
        // Arrange & Act
        var reCheckInResult = new AlphaPlus.Auth.Site.Features.Account.ReCheckInResult();

        // Assert
        reCheckInResult.Should().NotBeNull();
    }

    [Fact]
    public void SignInParam_ShouldBeCreated()
    {
        // Arrange & Act
        var signInParam = new AlphaPlus.Auth.Site.Features.Account.SignInParam();

        // Assert
        signInParam.Should().NotBeNull();
    }

    [Fact]
    public void SSOParam_ShouldBeCreated()
    {
        // Arrange & Act
        var ssoParam = new AlphaPlus.Auth.Site.Features.Account.SSOParam();

        // Assert
        ssoParam.Should().NotBeNull();
    }

    [Fact]
    public void InternalLoginParam_ShouldBeCreated()
    {
        // Arrange & Act
        var internalLoginParam = new AlphaPlus.Auth.Site.Features.Account.InternalLoginParam();

        // Assert
        internalLoginParam.Should().NotBeNull();
    }

    [Fact]
    public void IndirectLoginParam_ShouldBeCreated()
    {
        // Arrange & Act
        var indirectLoginParam = new AlphaPlus.Auth.Site.Features.Account.IndirectLoginParam();

        // Assert
        indirectLoginParam.Should().NotBeNull();
    }

    [Fact]
    public void RequestInfo_ShouldBeCreated()
    {
        // Arrange & Act
        var requestInfo = new AlphaPlus.RequestInfo.Models.RequestInfo();

        // Assert
        requestInfo.Should().NotBeNull();
    }

    [Fact]
    public void Enumeration_ShouldWorkWithUserLevel()
    {
        // Arrange & Act
        var userLevel = Enumeration.FromValue<UserLevel>((byte)1);

        // Assert
        userLevel.Should().NotBeNull();
        userLevel.DisplayName.Should().Be("Member");
    }

    [Fact]
    public void Enumeration_ShouldWorkWithDifferentValues()
    {
        // Arrange & Act
        var member = Enumeration.FromValue<UserLevel>((byte)1);
        var agent = Enumeration.FromValue<UserLevel>((byte)2);
        var master = Enumeration.FromValue<UserLevel>((byte)3);
        var super = Enumeration.FromValue<UserLevel>((byte)4);

        // Assert
        member.DisplayName.Should().Be("Member");
        agent.DisplayName.Should().Be("Agent");
        master.DisplayName.Should().Be("Master");
        super.DisplayName.Should().Be("Super");
    }
} 