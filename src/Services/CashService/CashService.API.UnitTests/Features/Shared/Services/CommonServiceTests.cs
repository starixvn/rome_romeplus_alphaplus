using AlphaPlus.CashService.API.Features.Shared.Criteria;
using AlphaPlus.CashService.API.Features.Shared.Models;
using AlphaPlus.CashService.API.Features.Shared.Requests;
using AlphaPlus.CashService.API.Features.Shared.Services;
using Fanex.Data.Repository;
using Fanex.Logging;
using Moq;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Shared.Services;

public class CommonServiceTests
{
    private readonly Mock<IDynamicRepository> _repositoryMock;
    private readonly Mock<ILogger> _loggerMock;
    private readonly CommonService _commonService;

    public CommonServiceTests()
    {
        _repositoryMock = new Mock<IDynamicRepository>();
        _loggerMock = new Mock<ILogger>();
        _commonService = new CommonService(_repositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetCountryCodes_WhenValidSiteNameProvided_ReturnsCountryCodes()
    {
        // Arrange
        var siteName = "TestSite";
        var expectedCountryCodes = new List<string> { "US", "UK", "CA" };
        _repositoryMock.Setup(x => x.FetchAsync<string>(It.Is<GetCountryCodesCriteria>(c => c.SiteName == siteName), string.Empty, CancellationToken.None))
            .ReturnsAsync(expectedCountryCodes);

        // Act
        var result = await _commonService.GetCountryCodes(siteName);

        // Assert
        Assert.Equal(expectedCountryCodes, result);
        _repositoryMock.Verify(x => x.FetchAsync<string>(It.Is<GetCountryCodesCriteria>(c => c.SiteName == siteName), string.Empty, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task RunDBCommand_WhenCommandHasError_ReturnsErrorResult()
    {
        // Arrange
        var request = new BaseActionRequest { ActorId = 1, UserId = 2, CustDBGroupId = 3 };
        var baseDBCommand = new BaseDBCommand(1, 0, "192.168.1.1", "en")
        {
            ErrCode = 1,
            ErrMsg = "Test Error"
        };

        // Act
        var result = await _commonService.RunDBCommand(request, baseDBCommand);

        // Assert
        Assert.Equal(1, result.Code);
        Assert.Equal("Test Error", result.Message);
        _repositoryMock.Verify(x => x.ExecuteAsync(It.IsAny<WriteLogDBCommandCriteria>(), string.Empty, CancellationToken.None), Times.Never);
    }

    [Fact]
    public async Task RunDBCommand_WhenCommandSuccessful_ReturnsSuccessResult()
    {
        // Arrange
        var request = new BaseActionRequest { ActorId = 1, UserId = 2, CustDBGroupId = 3 };
        var baseDBCommand = new BaseDBCommand(1, 0, "192.168.1.1", "en") { ErrCode = 0 };
        var writeLogModels = new List<WriteLogDBCommandModel>();
        _repositoryMock.Setup(x => x.FetchAsync<WriteLogDBCommandModel>(It.Is<BaseDBCommand>(c => c.CustId == 1 && c.SubAccId == 0), string.Empty, CancellationToken.None))
            .ReturnsAsync(writeLogModels);

        // Act
        var result = await _commonService.RunDBCommand(request, baseDBCommand);

        // Assert
        Assert.Equal(0, result.Code);
        Assert.Empty(result.Message);
        _repositoryMock.Verify(x => x.FetchAsync<WriteLogDBCommandModel>(It.Is<BaseDBCommand>(c => c.CustId == 1 && c.SubAccId == 0), string.Empty, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task WriteLogDBCommand_WhenFirstAttemptSucceeds_DoesNotRetry()
    {
        // Arrange
        var request = new BaseActionRequest { ActorId = 1, UserId = 2, CustDBGroupId = 3 };
        var writeLogModel = new WriteLogDBCommandModel
        {
            ActionId = 1,
            TrackingValue = "test",
            TrackingValue2 = "test2",
            TrackingValueMask = "mask",
            TrackingValueMask2 = "mask2"
        };
        _repositoryMock.Setup(x => x.ExecuteAsync(It.Is<WriteLogDBCommandCriteria>(c => c.CustDbGroupId == -1), string.Empty, CancellationToken.None))
            .Returns(Task.FromResult(0));

        // Act
        await _commonService.WriteLogDBCommand(request, writeLogModel);

        // Assert
        _repositoryMock.Verify(x => x.ExecuteAsync(It.Is<WriteLogDBCommandCriteria>(c => c.CustDbGroupId == -1), string.Empty, CancellationToken.None), Times.Once);
        _repositoryMock.Verify(x => x.ExecuteAsync(It.Is<WriteLogDBCommandCriteria>(c => c.CustDbGroupId != -1), string.Empty, CancellationToken.None), Times.Never);
    }

    [Fact]
    public async Task WriteLogDBCommand_WhenFirstAttemptFails_RetriesWithDefaultSiteDB()
    {
        // Arrange
        var request = new BaseActionRequest { ActorId = 1, UserId = 2, CustDBGroupId = 3 };
        var writeLogModel = new WriteLogDBCommandModel
        {
            ActionId = 1,
            TrackingValue = "test",
            TrackingValue2 = "test2",
            TrackingValueMask = "mask",
            TrackingValueMask2 = "mask2"
        };

        _repositoryMock.Setup(x => x.ExecuteAsync(It.Is<WriteLogDBCommandCriteria>(c => c.CustDbGroupId == -1), string.Empty, CancellationToken.None))
            .ThrowsAsync(new Exception("First attempt fails"));
        _repositoryMock.Setup(x => x.ExecuteAsync(It.Is<WriteLogDBCommandCriteria>(c => c.CustDbGroupId != -1), string.Empty, CancellationToken.None))
            .Returns(Task.FromResult(0));

        // Act
        await _commonService.WriteLogDBCommand(request, writeLogModel);

        // Assert
        _repositoryMock.Verify(x => x.ExecuteAsync(It.Is<WriteLogDBCommandCriteria>(c => c.CustDbGroupId == -1), string.Empty, CancellationToken.None), Times.Once);
        _repositoryMock.Verify(x => x.ExecuteAsync(It.Is<WriteLogDBCommandCriteria>(c => c.CustDbGroupId != -1), string.Empty, CancellationToken.None), Times.Once);
        _loggerMock.Verify(x => x.ErrorAsync(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
    }

    [Fact]
    public async Task GetSetupCheckList_WhenValidCustIdProvided_ReturnsSetupCheckListModel()
    {
        // Arrange
        const int custId = 12345;
        var expectedSetupCheckList = new SetupCheckListModel(true, false, true);
        _repositoryMock.Setup(x => x.GetAsync<SetupCheckListModel>(It.Is<GetSetupCheckListCriteria>(c => c.CustId == custId), string.Empty, CancellationToken.None))
            .ReturnsAsync(expectedSetupCheckList);

        // Act
        var result = await _commonService.GetSetupCheckList(custId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedSetupCheckList.HasDepositMethods, result.HasDepositMethods);
        Assert.Equal(expectedSetupCheckList.HasWithdrawalMethods, result.HasWithdrawalMethods);
        Assert.Equal(expectedSetupCheckList.HasLineID, result.HasLineID);
        Assert.Equal(expectedSetupCheckList.IsCompleteAll, result.IsCompleteAll);
        _repositoryMock.Verify(x => x.GetAsync<SetupCheckListModel>(It.Is<GetSetupCheckListCriteria>(c => c.CustId == custId), string.Empty, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetSetupCheckList_WhenCompleteSetup_ReturnsCompleteSetupCheckList()
    {
        // Arrange
        const int custId = 12345;
        var expectedSetupCheckList = new SetupCheckListModel(true, true, true);
        _repositoryMock.Setup(x => x.GetAsync<SetupCheckListModel>(It.Is<GetSetupCheckListCriteria>(c => c.CustId == custId), string.Empty, CancellationToken.None))
            .ReturnsAsync(expectedSetupCheckList);

        // Act
        var result = await _commonService.GetSetupCheckList(custId);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.HasDepositMethods);
        Assert.True(result.HasWithdrawalMethods);
        Assert.True(result.HasLineID);
        Assert.True(result.IsCompleteAll);
        _repositoryMock.Verify(x => x.GetAsync<SetupCheckListModel>(It.Is<GetSetupCheckListCriteria>(c => c.CustId == custId), string.Empty, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetSetupCheckList_WhenIncompleteSetup_ReturnsIncompleteSetupCheckList()
    {
        // Arrange
        const int custId = 12345;
        var expectedSetupCheckList = new SetupCheckListModel(false, false, false);
        _repositoryMock.Setup(x => x.GetAsync<SetupCheckListModel>(It.Is<GetSetupCheckListCriteria>(c => c.CustId == custId), string.Empty, CancellationToken.None))
            .ReturnsAsync(expectedSetupCheckList);

        // Act
        var result = await _commonService.GetSetupCheckList(custId);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.HasDepositMethods);
        Assert.False(result.HasWithdrawalMethods);
        Assert.False(result.HasLineID);
        Assert.False(result.IsCompleteAll);
        _repositoryMock.Verify(x => x.GetAsync<SetupCheckListModel>(It.Is<GetSetupCheckListCriteria>(c => c.CustId == custId), string.Empty, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetSetupCheckList_WhenPartialSetup_ReturnsPartialSetupCheckList()
    {
        // Arrange
        const int custId = 12345;
        var expectedSetupCheckList = new SetupCheckListModel(true, true, false);
        _repositoryMock.Setup(x => x.GetAsync<SetupCheckListModel>(It.Is<GetSetupCheckListCriteria>(c => c.CustId == custId), string.Empty, CancellationToken.None))
            .ReturnsAsync(expectedSetupCheckList);

        // Act
        var result = await _commonService.GetSetupCheckList(custId);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.HasDepositMethods);
        Assert.True(result.HasWithdrawalMethods);
        Assert.False(result.HasLineID);
        Assert.False(result.IsCompleteAll);
        _repositoryMock.Verify(x => x.GetAsync<SetupCheckListModel>(It.Is<GetSetupCheckListCriteria>(c => c.CustId == custId), string.Empty, CancellationToken.None), Times.Once);
    }
}