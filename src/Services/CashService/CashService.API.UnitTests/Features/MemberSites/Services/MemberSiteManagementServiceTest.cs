using AlphaPlus.CashService.API.Features.MemberSites.Criteria;
using AlphaPlus.CashService.API.Features.MemberSites.Models;
using AlphaPlus.CashService.API.Features.MemberSites.Services;
using AlphaPlus.CashService.API.Features.Shared.Models;
using AlphaPlus.CashService.API.Features.Shared.Requests;
using AlphaPlus.CashService.API.Features.Shared.Services;
using Fanex.Data.Repository;
using Moq;
using Xunit;

namespace CashService.API.UnitTests.Features.MemberSites.Services;

public class MemberSiteManagementServiceTest
{
    private readonly Mock<IDynamicRepository> _repositoryMock;
    private readonly Mock<ICommonService> _commonServiceMock;
    private readonly MemberSiteManagementService _service;

    public MemberSiteManagementServiceTest()
    {
        _repositoryMock = new Mock<IDynamicRepository>();
        _commonServiceMock = new Mock<ICommonService>();
        _service = new MemberSiteManagementService(_repositoryMock.Object, _commonServiceMock.Object);
    }

    [Fact]
    public async Task GetAsync_WhenValidParametersProvided_ShouldReturnMemberSiteModel()
    {
        // Arrange
        var custId = 1;
        var site = "test-site";
        var domains = new List<DomainModel> { new() { Url = "https://example.com" } };
        var settings = new List<MemberSiteDataObject> { new() { Mobile = "1234567890" } };
        var countryCodes = new List<string> { "US", "UK" };

        _repositoryMock.Setup(x =>
                x.FetchMultipleAsync<DomainModel, MemberSiteDataObject>(It.IsAny<ICriteria>(), It.IsAny<string>(),
                    It.IsAny<System.Threading.CancellationToken>()))
            .ReturnsAsync((domains as IEnumerable<DomainModel>, settings as IEnumerable<MemberSiteDataObject>));


        _repositoryMock.Setup(x => x.FetchAsync<string>(It.IsAny<ICriteria>(), It.IsAny<string>(),
                It.IsAny<System.Threading.CancellationToken>()))
            .Returns(Task.FromResult(countryCodes as IEnumerable<string>));

        // Act
        var result = await _service.GetAsync(custId, site);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(domains, result.Domains);
        Assert.Equal(countryCodes, result.CountryCodes);
        Assert.Equal(settings[0].Mobile, result.Mobile);
        Assert.Equal(settings[0].Contacts, result.Contacts);
        Assert.Equal(settings[0].Announcement, result.Announcement);
        Assert.NotNull(result.GeneralSettings);
        Assert.Equal(2, result.GeneralSettings.Count());
    }

    [Fact]
    public async Task GetAsync_WhenNoSettingsFound_ShouldUseDefaultValues()
    {
        // Arrange
        var custId = 1;
        var site = "test-site";
        var domains = new List<DomainModel>();
        var settings = new List<MemberSiteDataObject>();
        var countryCodes = new List<string>();

        _repositoryMock.Setup(x =>
                x.FetchMultipleAsync<DomainModel, MemberSiteDataObject>(It.IsAny<ICriteria>(), It.IsAny<string>(),
                    It.IsAny<System.Threading.CancellationToken>()))
            .ReturnsAsync((domains as IEnumerable<DomainModel>, settings as IEnumerable<MemberSiteDataObject>));


        _repositoryMock.Setup(x => x.FetchAsync<string>(It.IsAny<ICriteria>(), It.IsAny<string>(),
                It.IsAny<System.Threading.CancellationToken>()))
            .Returns(Task.FromResult(countryCodes as IEnumerable<string>));

        // Act
        var result = await _service.GetAsync(custId, site);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Domains);
        Assert.Empty(result.CountryCodes);
        Assert.Null(result.Mobile);
        Assert.Null(result.Contacts);
        Assert.Null(result.Announcement);
        Assert.NotNull(result.GeneralSettings);
        Assert.Equal(2, result.GeneralSettings.Count());
    }

    [Fact]
    public async Task UpdateAsync_WhenValidModelProvided_ShouldUpdateSuccessfully()
    {
        // Arrange
        var model = new UpdateMemberSiteModel
        {
            CustId = 1,
            SubAccId = 2,
            IP = "127.0.0.1",
            Language = "en",
            UserId = 3,
            SiteDB = 4,
            Mobile = "1234567890",
            Contacts = "contact info",
            Announcement = "announcement text",
            GeneralSettings = new List<MemberSitePermission>
            {
                new() { Id = "allowRegistration", Value = true },
                new() { Id = "allowDeposit", Value = true }
            }
        };

        _commonServiceMock.Setup(x => x.RunDBCommand(
                It.IsAny<BaseActionRequest>(),
                It.IsAny<UpdateMemberSiteInfoCommand>()))
            .ReturnsAsync(new ResultModel(0, "Success"));

        // Act
        var result = await _service.UpdateAsync(model);

        // Assert
        Assert.NotNull(result);
        _commonServiceMock.Verify(x => x.RunDBCommand(
            It.Is<BaseActionRequest>(r =>
                r.ActorId == model.UserId &&
                r.UserId == model.CustId &&
                r.IP == model.IP &&
                r.Language == model.Language &&
                r.CustDBGroupId == model.SiteDB),
            It.Is<UpdateMemberSiteInfoCommand>(c =>
                c.CustId == model.CustId &&
                c.SubAccId == model.SubAccId &&
                c.IP == model.IP &&
                c.Language == model.Language &&
                c.Mobile == model.Mobile &&
                c.Contacts == model.Contacts &&
                c.Announcement == model.Announcement &&
                c.SettingInfo == (int)MemberSiteDataObject.GeneralSettingType.AllowAll)));
    }

    [Fact]
    public async Task UpdateAsync_WhenModelIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        UpdateMemberSiteModel model = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _service.UpdateAsync(model));
    }

    [Theory]
    [InlineData(true, false, MemberSiteDataObject.GeneralSettingType.AllowRegister)]
    [InlineData(false, true, MemberSiteDataObject.GeneralSettingType.AllowLogin)]
    [InlineData(true, true, MemberSiteDataObject.GeneralSettingType.AllowAll)]
    [InlineData(false, false, MemberSiteDataObject.GeneralSettingType.None)]
    public async Task UpdateAsync_WithDifferentPermissionCombinations_ShouldSetCorrectSettingInfo(
        bool allowRegistration,
        bool allowDeposit,
        MemberSiteDataObject.GeneralSettingType expectedSettingType)
    {
        // Arrange
        var model = new UpdateMemberSiteModel
        {
            CustId = 1,
            SubAccId = 2,
            IP = "127.0.0.1",
            Language = "en",
            UserId = 3,
            SiteDB = 4,
            GeneralSettings = new List<MemberSitePermission>
            {
                new() { Id = "allowRegistration", Value = allowRegistration },
                new() { Id = "allowDeposit", Value = allowDeposit }
            }
        };

        _commonServiceMock.Setup(x => x.RunDBCommand(
                It.IsAny<BaseActionRequest>(),
                It.IsAny<UpdateMemberSiteInfoCommand>()))
            .ReturnsAsync(new ResultModel(0, "Success"));

        // Act
        await _service.UpdateAsync(model);

        // Assert
        _commonServiceMock.Verify(x => x.RunDBCommand(
            It.IsAny<BaseActionRequest>(),
            It.Is<UpdateMemberSiteInfoCommand>(c => c.SettingInfo == (int)expectedSettingType)));
    }
}