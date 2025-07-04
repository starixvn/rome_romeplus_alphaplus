using AlphaPlus.CashService.API.Features.MemberSites.Models;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.MemberSites.Models;

public class UpdateMemberSiteModelTest
{
    [Fact]
    public void Constructor_WhenCreated_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var model = new UpdateMemberSiteModel();

        // Assert
        Assert.Equal(0, model.SubAccId);
        Assert.Null(model.IP);
        Assert.Null(model.Language);
        Assert.Equal(0, model.UserId);
        Assert.Equal(0, model.SiteDB);
    }

    [Fact]
    public void Properties_WhenSet_ShouldStoreValuesCorrectly()
    {
        // Arrange
        var model = new UpdateMemberSiteModel();
        var subAccId = 1;
        var ip = "127.0.0.1";
        var language = "en";
        var userId = 2;
        var siteDB = 3;

        // Act
        model.SubAccId = subAccId;
        model.IP = ip;
        model.Language = language;
        model.UserId = userId;
        model.SiteDB = siteDB;

        // Assert
        Assert.Equal(subAccId, model.SubAccId);
        Assert.Equal(ip, model.IP);
        Assert.Equal(language, model.Language);
        Assert.Equal(userId, model.UserId);
        Assert.Equal(siteDB, model.SiteDB);
    }

    [Fact]
    public void Properties_WhenSet_ShouldInheritFromBaseClassCorrectly()
    {
        // Arrange
        var model = new UpdateMemberSiteModel();
        var custId = 1;
        var countryCode = "US";
        var countryCodes = new[] { "US", "UK" };
        var domains = new[] { new DomainModel { Url = "https://example.com" } };
        var generalSettings = new[] { new MemberSitePermission { Id = "test", Name = "Test", Description = "Test Desc", Value = true } };
        var mobile = "1234567890";
        var contacts = "contact info";
        var announcement = "announcement text";

        // Act
        model.CustId = custId;
        model.CountryCode = countryCode;
        model.CountryCodes = countryCodes;
        model.Domains = domains;
        model.GeneralSettings = generalSettings;
        model.Mobile = mobile;
        model.Contacts = contacts;
        model.Announcement = announcement;

        // Assert
        Assert.Equal(custId, model.CustId);
        Assert.Equal(countryCode, model.CountryCode);
        Assert.Equal(countryCodes, model.CountryCodes);
        Assert.Equal(domains, model.Domains);
        Assert.Equal(generalSettings, model.GeneralSettings);
        Assert.Equal(mobile, model.Mobile);
        Assert.Equal(contacts, model.Contacts);
        Assert.Equal(announcement, model.Announcement);
    }
} 