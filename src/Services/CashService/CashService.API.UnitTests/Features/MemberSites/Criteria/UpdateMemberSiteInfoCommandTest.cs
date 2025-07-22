using AlphaPlus.CashService.API.Features.MemberSites.Criteria;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.MemberSites.Criteria;

//
public class UpdateMemberSiteInfoCommandTests
{
    [Fact]
    public void Constructor_WhenValidRequestProvided_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var custId = 1;
        var subAccId = 2;
        var ip = "127.0.0.1";
        var language = "en";

        // Act
        var command = new UpdateMemberSiteInfoCommand(custId, subAccId, ip, language);

        // Assert
        Assert.Equal(custId, command.CustId);
        Assert.Equal(subAccId, command.SubAccId);
        Assert.Equal(ip, command.IP);
        Assert.Equal(language, command.Language);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var command = new UpdateMemberSiteInfoCommand(1, 2, "127.0.0.1", "en");

        // Act
        var settingKey = command.GetSettingKey();

        // Assert
        Assert.Equal("Age_CashService_GeneralSetting_Upd", settingKey);
    }

    [Fact]
    public void Properties_WhenSet_ShouldStoreValuesCorrectly()
    {
        // Arrange
        var command = new UpdateMemberSiteInfoCommand(1, 2, "127.0.0.1", "en");
        var settingInfo = 123;
        var mobile = "1234567890";
        var contacts = "contact info";
        var announcement = "announcement text";

        // Act
        command.SettingInfo = settingInfo;
        command.Mobile = mobile;
        command.Contacts = contacts;
        command.Announcement = announcement;

        // Assert
        Assert.Equal(settingInfo, command.SettingInfo);
        Assert.Equal(mobile, command.Mobile);
        Assert.Equal(contacts, command.Contacts);
        Assert.Equal(announcement, command.Announcement);
    }
}

