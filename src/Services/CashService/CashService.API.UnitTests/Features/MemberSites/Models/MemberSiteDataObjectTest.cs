using AlphaPlus.CashService.API.Features.MemberSites.Models;
using Xunit;

namespace CashService.API.UnitTests.Features.MemberSites.Models;

public class MemberSiteDataObjectTest
{
    [Fact]
    public void Constructor_WhenCreated_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var dataObject = new MemberSiteDataObject();

        // Assert
        Assert.Equal(0, dataObject.SettingInfo);
        Assert.False(dataObject.IsAllowRegister);
        Assert.False(dataObject.IsAllowLogin);
        Assert.Null(dataObject.Mobile);
        Assert.Null(dataObject.Contacts);
        Assert.Null(dataObject.Announcement);
    }

    [Theory]
    [InlineData(MemberSiteDataObject.GeneralSettingType.None, false, false)]
    [InlineData(MemberSiteDataObject.GeneralSettingType.AllowRegister, true, false)]
    [InlineData(MemberSiteDataObject.GeneralSettingType.AllowLogin, false, true)]
    [InlineData(MemberSiteDataObject.GeneralSettingType.AllowAll, true, true)]
    public void IsAllowRegisterAndLogin_ShouldReturnCorrectValues(
        MemberSiteDataObject.GeneralSettingType settingType,
        bool expectedAllowRegister,
        bool expectedAllowLogin)
    {
        // Arrange
        var dataObject = new MemberSiteDataObject
        {
            SettingInfo = (int)settingType
        };

        // Act & Assert
        Assert.Equal(expectedAllowRegister, dataObject.IsAllowRegister);
        Assert.Equal(expectedAllowLogin, dataObject.IsAllowLogin);
    }

    [Fact]
    public void Properties_WhenSet_ShouldStoreValuesCorrectly()
    {
        // Arrange
        var dataObject = new MemberSiteDataObject();
        var mobile = "1234567890";
        var contacts = "contact info";
        var announcement = "announcement text";

        // Act
        dataObject.Mobile = mobile;
        dataObject.Contacts = contacts;
        dataObject.Announcement = announcement;

        // Assert
        Assert.Equal(mobile, dataObject.Mobile);
        Assert.Equal(contacts, dataObject.Contacts);
        Assert.Equal(announcement, dataObject.Announcement);
    }
} 