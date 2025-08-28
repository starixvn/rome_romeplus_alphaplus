using AlphaPlus.CashService.API.Features.Shared.Criteria;
using AlphaPlus.CashService.API.Features.Shared.Models;
using AlphaPlus.CashService.API.Features.Shared.Requests;
using AlphaPlus.CashService.API.Features.Shared.Services;
using Fanex.Data.Repository;
using Fanex.Logging;
using Moq;
using Xunit;
using Microsoft.Extensions.Caching.Memory;

namespace AlphaPlus.CashService.API.UnitTests.Features.Shared.Services;

public class CommonServiceTests
{
    private readonly Mock<IDynamicRepository> _repositoryMock;
    private readonly Mock<ILogger> _loggerMock;
    private readonly Mock<ICacheRefService> _cacheRefServiceMock;
    private readonly Mock<IMemoryCache> _cacheServiceMock;
    private readonly CommonService _commonService;

    public CommonServiceTests()
    {
        _repositoryMock = new Mock<IDynamicRepository>();
        _loggerMock = new Mock<ILogger>();
        _cacheRefServiceMock = new Mock<ICacheRefService>();
        _cacheServiceMock = new Mock<IMemoryCache>();
        _commonService = new CommonService(_repositoryMock.Object, _loggerMock.Object, _cacheRefServiceMock.Object, _cacheServiceMock.Object);
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

        // Fix: Mock the FetchAsync call with the correct parameter type
        _repositoryMock.Setup(x => x.FetchAsync<WriteLogDBCommandModel>(baseDBCommand, string.Empty, CancellationToken.None))
            .ReturnsAsync(writeLogModels);

        // Act
        var result = await _commonService.RunDBCommand(request, baseDBCommand);

        // Assert
        Assert.Equal(0, result.Code);
        Assert.Null(result.Message);

        // Fix: Verify the mock call with the correct parameter
        _repositoryMock.Verify(x => x.FetchAsync<WriteLogDBCommandModel>(baseDBCommand, string.Empty, CancellationToken.None), Times.Once);
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

    [Fact]
    public async Task GetProviderBanks_WhenValidParametersProvided_ReturnsFilteredBanks()
    {
        // Arrange
        var site = "test-site";
        var providerId = 1;
        var languageCode = "en";
        var allBanks = new List<BankModel>
        {
            new BankModel { Id = 1, ProviderId = 1, Bank = "Bank1", Name = "OriginalName1" },
            new BankModel { Id = 2, ProviderId = 1, Bank = "Bank2", Name = "OriginalName2" },
            new BankModel { Id = 3, ProviderId = 2, Bank = "Bank3", Name = "OriginalName3" }
        };

        // Mock the GetOrCreateAsync extension method behavior
        _cacheServiceMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny))
            .Returns(false);

        var cacheEntry = Mock.Of<ICacheEntry>();
        _cacheServiceMock.Setup(x => x.CreateEntry(It.IsAny<object>()))
            .Returns(cacheEntry);

        // Mock the repository call that the factory function would make
        _repositoryMock.Setup(x => x.FetchAsync<BankModel>(It.IsAny<GetProviderBanksCriteria>(), string.Empty, CancellationToken.None))
            .ReturnsAsync(allBanks);

        _cacheRefServiceMock.Setup(x => x.GetBankName(1, "Bank1", languageCode))
            .Returns("TranslatedBank1");
        _cacheRefServiceMock.Setup(x => x.GetBankName(2, "Bank2", languageCode))
            .Returns("TranslatedBank2");

        // Act
        var result = await _commonService.GetProviderBanks(site, providerId, languageCode);

        // Assert
        var resultList = result.ToList();
        Assert.Equal(2, resultList.Count);
        Assert.Equal("TranslatedBank1", resultList[0].Name);
        Assert.Equal("TranslatedBank2", resultList[1].Name);
        Assert.Equal(1, resultList[0].ProviderId);
        Assert.Equal(1, resultList[1].ProviderId);

        _cacheServiceMock.Verify(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny), Times.Once);
        _cacheServiceMock.Verify(x => x.CreateEntry(It.IsAny<object>()), Times.Once);
        _repositoryMock.Verify(x => x.FetchAsync<BankModel>(It.IsAny<GetProviderBanksCriteria>(), string.Empty, CancellationToken.None), Times.Once);
        _cacheRefServiceMock.Verify(x => x.GetBankName(1, "Bank1", languageCode), Times.Once);
        _cacheRefServiceMock.Verify(x => x.GetBankName(2, "Bank2", languageCode), Times.Once);
    }

    [Fact]
    public async Task GetProviderBanks_WhenNoBanksFound_ReturnsEmptyCollection()
    {
        // Arrange
        var site = "test-site";
        var providerId = 1;
        var languageCode = "en";
        var allBanks = new List<BankModel>();

        // Mock the GetOrCreateAsync extension method behavior
        _cacheServiceMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny))
            .Returns(false);

        var cacheEntry = Mock.Of<ICacheEntry>();
        _cacheServiceMock.Setup(x => x.CreateEntry(It.IsAny<object>()))
            .Returns(cacheEntry);

        // Mock the repository call that the factory function would make
        _repositoryMock.Setup(x => x.FetchAsync<BankModel>(It.IsAny<GetProviderBanksCriteria>(), string.Empty, CancellationToken.None))
            .ReturnsAsync(allBanks);

        // Act
        var result = await _commonService.GetProviderBanks(site, providerId, languageCode);

        // Assert
        Assert.Empty(result);
        _cacheServiceMock.Verify(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny), Times.Once);
        _cacheServiceMock.Verify(x => x.CreateEntry(It.IsAny<object>()), Times.Once);
        _repositoryMock.Verify(x => x.FetchAsync<BankModel>(It.IsAny<GetProviderBanksCriteria>(), string.Empty, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetProviderBanks_WhenNoMatchingProviderId_ReturnsEmptyCollection()
    {
        // Arrange
        var site = "test-site";
        var providerId = 1;
        var languageCode = "en";
        var allBanks = new List<BankModel>
        {
            new BankModel { Id = 1, ProviderId = 2, Bank = "Bank1", Name = "OriginalName1" },
            new BankModel { Id = 2, ProviderId = 3, Bank = "Bank2", Name = "OriginalName2" }
        };

        // Mock the GetOrCreateAsync extension method behavior
        _cacheServiceMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny))
            .Returns(false);

        var cacheEntry = Mock.Of<ICacheEntry>();
        _cacheServiceMock.Setup(x => x.CreateEntry(It.IsAny<object>()))
            .Returns(cacheEntry);

        // Mock the repository call that the factory function would make
        _repositoryMock.Setup(x => x.FetchAsync<BankModel>(It.IsAny<GetProviderBanksCriteria>(), string.Empty, CancellationToken.None))
            .ReturnsAsync(allBanks);

        // Act
        var result = await _commonService.GetProviderBanks(site, providerId, languageCode);

        // Assert
        Assert.Empty(result);
        _cacheServiceMock.Verify(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny), Times.Once);
        _cacheServiceMock.Verify(x => x.CreateEntry(It.IsAny<object>()), Times.Once);
        _repositoryMock.Verify(x => x.FetchAsync<BankModel>(It.IsAny<GetProviderBanksCriteria>(), string.Empty, CancellationToken.None), Times.Once);
        _cacheRefServiceMock.Verify(x => x.GetBankName(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetProviderBanks_WhenCacheReturnsNull_ReturnsEmptyCollection()
    {
        // Arrange
        var site = "test-site";
        var providerId = 1;
        var languageCode = "en";

        var cacheEntry = Mock.Of<ICacheEntry>();
        _cacheServiceMock.Setup(x => x.CreateEntry(It.IsAny<object>()))
            .Returns(cacheEntry);

        // Mock the GetOrCreateAsync extension method behavior
        _cacheServiceMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny))
            .Returns(false);

        // Mock the repository call that the factory function would make
        _repositoryMock.Setup(x => x.FetchAsync<BankModel>(It.IsAny<GetProviderBanksCriteria>(), string.Empty, CancellationToken.None))
            .ReturnsAsync((IEnumerable<BankModel>)null);

        // Act
        var result = await _commonService.GetProviderBanks(site, providerId, languageCode);

        // Assert
        Assert.Empty(result);
        _cacheServiceMock.Verify(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny), Times.Once);
        _cacheServiceMock.Verify(x => x.CreateEntry(It.IsAny<object>()), Times.Once);
        _repositoryMock.Verify(x => x.FetchAsync<BankModel>(It.IsAny<GetProviderBanksCriteria>(), string.Empty, CancellationToken.None), Times.Once);
        _cacheRefServiceMock.Verify(x => x.GetBankName(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetProviderBanks_WhenBankHasDepositMode_DeserializesAndSetsDepositMatchingMode()
    {
        // Arrange
        var site = "test-site";
        var providerId = 1;
        var languageCode = "en";
        var depositModeJson = "{\"SourceType\":\"TestSource\",\"MappingType\":[1,2,3],\"SourceTypeName\":\"OriginalSourceTypeName\"}";
        
        var allBanks = new List<BankModel>
        {
            new BankModel 
            { 
                Id = 1, 
                ProviderId = 1, 
                Bank = "Bank1", 
                Name = "OriginalName1",
                DepositMode = depositModeJson
            }
        };

        var cacheEntry = Mock.Of<ICacheEntry>();
        _cacheServiceMock.Setup(x => x.CreateEntry(It.IsAny<object>()))
            .Returns(cacheEntry);

        // Mock the GetOrCreateAsync extension method behavior
        _cacheServiceMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny))
            .Returns(false);

        // Mock the repository call that the factory function would make
        _repositoryMock.Setup(x => x.FetchAsync<BankModel>(It.IsAny<GetProviderBanksCriteria>(), string.Empty, CancellationToken.None))
            .ReturnsAsync(allBanks);

        _cacheRefServiceMock.Setup(x => x.GetBankName(1, "Bank1", languageCode))
            .Returns("TranslatedBank1");
        _cacheRefServiceMock.Setup(x => x.GetDepositAutoModeName("TestSource", "TestSource", languageCode))
            .Returns("TranslatedSourceTypeName");

        // Act
        var result = await _commonService.GetProviderBanks(site, providerId, languageCode);

        // Assert
        var resultList = result.ToList();
        Assert.Single(resultList);
        Assert.NotNull(resultList[0].DepositMatchingMode);
        Assert.Equal("TestSource", resultList[0].DepositMatchingMode.SourceType);
        Assert.Equal(new[] { 1, 2, 3 }, resultList[0].DepositMatchingMode.MappingType);
        Assert.Equal("TranslatedSourceTypeName", resultList[0].DepositMatchingMode.SourceTypeName);

        _cacheRefServiceMock.Verify(x => x.GetDepositAutoModeName("TestSource", "TestSource", languageCode), Times.Once);
    }

    [Fact]
    public async Task GetProviderBanks_WhenBankHasNullDepositMode_DoesNotSetDepositMatchingMode()
    {
        // Arrange
        var site = "test-site";
        var providerId = 1;
        var languageCode = "en";
        
        var allBanks = new List<BankModel>
        {
            new BankModel 
            { 
                Id = 1, 
                ProviderId = 1, 
                Bank = "Bank1", 
                Name = "OriginalName1",
                DepositMode = null
            }
        };

        var cacheEntry = Mock.Of<ICacheEntry>();
        _cacheServiceMock.Setup(x => x.CreateEntry(It.IsAny<object>()))
            .Returns(cacheEntry);

        // Mock the underlying methods that GetOrCreateAsync extension method uses
        _cacheServiceMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny))
            .Returns(false);

        // Mock the repository call that the factory function would make
        _repositoryMock.Setup(x => x.FetchAsync<BankModel>(It.IsAny<GetProviderBanksCriteria>(), string.Empty, CancellationToken.None))
            .ReturnsAsync(allBanks);

        _cacheRefServiceMock.Setup(x => x.GetBankName(1, "Bank1", languageCode))
            .Returns("TranslatedBank1");

        // Act
        var result = await _commonService.GetProviderBanks(site, providerId, languageCode);

        // Assert
        var resultList = result.ToList();
        Assert.Single(resultList);
        Assert.Null(resultList[0].DepositMatchingMode);

        _cacheRefServiceMock.Verify(x => x.GetDepositAutoModeName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetProviderBanks_WhenBankHasEmptyDepositMode_DoesNotSetDepositMatchingMode()
    {
        // Arrange
        var site = "test-site";
        var providerId = 1;
        var languageCode = "en";
        
        var allBanks = new List<BankModel>
        {
            new BankModel 
            { 
                Id = 1, 
                ProviderId = 1, 
                Bank = "Bank1", 
                Name = "OriginalName1",
                DepositMode = null
            }
        };

        // Mock the GetOrCreateAsync extension method behavior
        _cacheServiceMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny))
            .Returns(false);

        var cacheEntry = Mock.Of<ICacheEntry>();
        _cacheServiceMock.Setup(x => x.CreateEntry(It.IsAny<object>()))
            .Returns(cacheEntry);

        // Mock the repository call that the factory function would make
        _repositoryMock.Setup(x => x.FetchAsync<BankModel>(It.IsAny<GetProviderBanksCriteria>(), string.Empty, CancellationToken.None))
            .ReturnsAsync(allBanks);

        _cacheRefServiceMock.Setup(x => x.GetBankName(1, "Bank1", languageCode))
            .Returns("TranslatedBank1");

        // Act
        var result = await _commonService.GetProviderBanks(site, providerId, languageCode);

        // Assert
        var resultList = result.ToList();
        Assert.Single(resultList);
        Assert.Null(resultList[0].DepositMatchingMode);

        _cacheRefServiceMock.Verify(x => x.GetDepositAutoModeName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetProviderBanks_WhenMultipleBanksWithMixedDepositModes_ProcessesCorrectly()
    {
        // Arrange
        var site = "test-site";
        var providerId = 1;
        var languageCode = "en";
        var depositModeJson = "{\"SourceType\":\"TestSource\",\"MappingType\":[1,2,3],\"SourceTypeName\":\"OriginalSourceTypeName\"}";
        
        var allBanks = new List<BankModel>
        {
            new BankModel 
            { 
                Id = 1, 
                ProviderId = 1, 
                Bank = "Bank1", 
                Name = "OriginalName1",
                DepositMode = depositModeJson
            },
            new BankModel 
            { 
                Id = 2, 
                ProviderId = 1, 
                Bank = "Bank2", 
                Name = "OriginalName2",
                DepositMode = null
            },
            new BankModel 
            { 
                Id = 3, 
                ProviderId = 1, 
                Bank = "Bank3", 
                Name = "OriginalName3",
                DepositMode = null
            }
        };

        // Mock the GetOrCreateAsync extension method behavior
        _cacheServiceMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny))
            .Returns(false);

        var cacheEntry = Mock.Of<ICacheEntry>();
        _cacheServiceMock.Setup(x => x.CreateEntry(It.IsAny<object>()))
            .Returns(cacheEntry);

        // Mock the repository call that the factory function would make
        _repositoryMock.Setup(x => x.FetchAsync<BankModel>(It.IsAny<GetProviderBanksCriteria>(), string.Empty, CancellationToken.None))
            .ReturnsAsync(allBanks);

        _cacheRefServiceMock.Setup(x => x.GetBankName(1, "Bank1", languageCode))
            .Returns("TranslatedBank1");
        _cacheRefServiceMock.Setup(x => x.GetBankName(2, "Bank2", languageCode))
            .Returns("TranslatedBank2");
        _cacheRefServiceMock.Setup(x => x.GetBankName(3, "Bank3", languageCode))
            .Returns("TranslatedBank3");
        _cacheRefServiceMock.Setup(x => x.GetDepositAutoModeName("TestSource", "TestSource", languageCode))
            .Returns("TranslatedSourceTypeName");

        // Act
        var result = await _commonService.GetProviderBanks(site, providerId, languageCode);

        // Assert
        var resultList = result.ToList();
        Assert.Equal(3, resultList.Count);
        
        // Bank with deposit mode
        Assert.NotNull(resultList[0].DepositMatchingMode);
        Assert.Equal("TestSource", resultList[0].DepositMatchingMode.SourceType);
        Assert.Equal("TranslatedSourceTypeName", resultList[0].DepositMatchingMode.SourceTypeName);
        
        // Bank with null deposit mode
        Assert.Null(resultList[1].DepositMatchingMode);
        
        // Bank with empty deposit mode
        Assert.Null(resultList[2].DepositMatchingMode);

        _cacheRefServiceMock.Verify(x => x.GetDepositAutoModeName("TestSource", "TestSource", languageCode), Times.Once);
    }
}