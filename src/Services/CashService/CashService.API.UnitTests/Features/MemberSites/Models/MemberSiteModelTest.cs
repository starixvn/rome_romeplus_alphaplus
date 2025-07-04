using AlphaPlus.CashService.API.Features.MemberSites.Models;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.MemberSites.Models;

public class MemberSiteModelTest
{
    [Fact]
    public void Constructor_WhenCreated_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var model = new MemberSiteModel();

        // Assert
        Assert.Equal(0, model.CustId);
        Assert.Null(model.CountryCode);
        Assert.Null(model.CountryCodes);
        Assert.Null(model.Domains);
        Assert.Null(model.GeneralSettings);
        Assert.Null(model.Mobile);
        Assert.Null(model.Contacts);
        Assert.Null(model.Announcement);
    }

    [Fact]
    public void Properties_WhenSet_ShouldStoreValuesCorrectly()
    {
        // Arrange
        var model = new MemberSiteModel();
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