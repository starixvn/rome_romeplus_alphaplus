using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.BetCreditTracking.BetCreditHistory;
using AlphaPlus.Reports.Features.Shared.Constants;
using AlphaPlus.Shared.AppResources.Report;
using Fanex.Data.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace AlphaPlus.Reports.UnitTests.Features.BetCreditTracking.BetCreditHistory;

[Trait("Category", "BetCreditHistory")]
public class BetCreditHistoryServiceTests
{
    private readonly Mock<IDynamicRepository> repositoryMock = new();
    private readonly Mock<IPartialUMService> partialUMServiceMock = new();
    private readonly Mock<IMemoryCache> memoryCacheMock = new();
    private readonly BetCreditHistoryService service;

    private readonly BetCreditHistoryParam testParam = new(
        Member: "TestMember",
        RefNo: 123,
        FromDate: new DateTime(2023, 1, 1),
        ToDate: new DateTime(2023, 1, 31),
        ViewerId: 456,
        Language: "en-US",
        PageIndex: 1,
        PageSize: 20,
        SiteName: "TestSite"
    );

    public BetCreditHistoryServiceTests()
    {
        service = new BetCreditHistoryService(
            repositoryMock.Object,
            partialUMServiceMock.Object,
            memoryCacheMock.Object
        );
    }

    [Fact]
    public async Task GetBetCreditHistory_Should_Call_PartialUMService_With_CorrectSettingKey()
    {
        // Arrange
        SetupMocks();

        // Act
        await service.GetBetCreditHistory(testParam);

        // Assert
        partialUMServiceMock.Verify(
            s => s.ThrowIfUM(It.IsAny<string>(), testParam.SiteName),
            Times.Once);
    }

    [Fact]
    public async Task GetBetCreditHistory_Should_Call_Repository_With_CorrectCriteria()
    {
        // Arrange
        SetupMocks();

        // Act
        await service.GetBetCreditHistory(testParam);

        // Assert
        repositoryMock.Verify(
            r => r.FetchAsync<BetCreditHistoryEntity>(
                It.Is<BetCreditHistoryCriteria>(c =>
                    c.Username == testParam.Member &&
                    c.FromDate == testParam.FromDate &&
                    c.ToDate == testParam.ToDate &&
                    c.RefNo == testParam.RefNo &&
                    c.LoggedId == testParam.ViewerId &&
                    c.PageIndex == testParam.PageIndex &&
                    c.PageSize == testParam.PageSize), string.Empty),
            Times.Once);
    }

    [Fact]
    public async Task GetBetCreditHistory_Should_Return_Data_With_Converted_ActionNames()
    {
        // Arrange
        var testResults = new List<BetCreditHistoryEntity>
        {
            new() { Action = "TransLog settled, running to settled", ActionId = 30, IsUserLogActionId = false },
            new() { Action = "Bet updated, running to void", ActionId = 50, IsUserLogActionId = false },
            new() { Action = "CustomLogAction", ActionId = 999, IsUserLogActionId = true }
        };

        SetupMocks(testResults);
        SetupLogActionCache(new LogAction
        {
            Id = 999,
            EnglishName = "CustomActionName",
            ChineseTraditionalName = "繁體中文名稱",
            ChineseSimplifiedName = "简体中文名称",
            JapaneseName = "日本語名称",
            ThaiName = "ชื่อไทย",
            KoreanName = "한국어 이름",
            VietnameseName = "Tên Tiếng Việt"
        });

        // Act
        var result = await service.GetBetCreditHistory(testParam);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.ReportData.Count());
        Assert.Equal(BetCreditHistoryResources.BetSettled + ", " + BetCreditHistoryResources.running + " " + BetCreditHistoryResources.to + " " + BetCreditHistoryResources.BetSettled, result.ReportData.ElementAt(0).Action);
        Assert.Equal(BetCreditHistoryResources.BetUpdated + ", " + BetCreditHistoryResources.running + " " + BetCreditHistoryResources.to + " " + BetCreditHistoryResources._void, result.ReportData.ElementAt(1).Action);
        Assert.Equal("CustomActionName", result.ReportData.ElementAt(2).Action);
    }

    [Theory]
    [InlineData(LanguageCode.ChineseTraditional, "繁體中文名稱")]
    [InlineData(LanguageCode.ChineseSimplified, "简体中文名称")]
    [InlineData(LanguageCode.Japanese, "日本語名称")]
    [InlineData(LanguageCode.Thai, "ชื่อไทย")]
    [InlineData(LanguageCode.Korean, "한국어 이름")]
    [InlineData(LanguageCode.Vietnamese, "Tên Tiếng Việt")]
    [InlineData("other", "CustomActionName")]
    public async Task GetBetCreditHistory_Should_Return_LogAction_With_Correct_Language(string language, string expectedName)
    {
        // Arrange
        var testResults = new List<BetCreditHistoryEntity>
        {
            new() { Action = "CustomLogAction", ActionId = 999, IsUserLogActionId = true }
        };

        var param = testParam with { Language = language };

        SetupMocks(testResults);
        SetupLogActionCache(new LogAction
        {
            Id = 999,
            EnglishName = "CustomActionName",
            ChineseTraditionalName = "繁體中文名稱",
            ChineseSimplifiedName = "简体中文名称",
            JapaneseName = "日本語名称",
            ThaiName = "ชื่อไทย",
            KoreanName = "한국어 이름",
            VietnameseName = "Tên Tiếng Việt"
        });

        // Act
        var result = await service.GetBetCreditHistory(param);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedName, result.ReportData.ElementAt(0).Action);
    }

    [Fact]
    public async Task GetBetCreditHistory_Should_Return_Original_Action_When_LogAction_Not_Found()
    {
        // Arrange
        var testResults = new List<BetCreditHistoryEntity>
        {
            new() { Action = "CustomLogAction", ActionId = 999, IsUserLogActionId = true }
        };

        SetupMocks(testResults);
        SetupEmptyLogActionCache();

        // Act
        var result = await service.GetBetCreditHistory(testParam);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("CustomLogAction", result.ReportData.ElementAt(0).Action);
    }

    [Theory]
    [InlineData(-1, "Opening credit")]
    [InlineData(29, "Cash out, running to settled")]
    [InlineData(34, "Cancel cash out, running to settled")]
    [InlineData(110, "Transfer submitted REF123")]
    public async Task GetBetCreditHistory_Should_Convert_Special_Actions(int actionId, string expectedResult)
    {
        // Arrange
        string action = actionId == 110
            ? "Transfer inserted REF123"
            : actionId == -1
                ? "any"
                : "running to settled";

        var testResults = new List<BetCreditHistoryEntity>
        {
            new() { Action = action, ActionId = actionId, IsUserLogActionId = false }
        };

        SetupMocks(testResults);

        // Act
        var result = await service.GetBetCreditHistory(testParam);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResult, result.ReportData.ElementAt(0).Action);
    }

    private void SetupMocks(List<BetCreditHistoryEntity> results = null)
    {
        results ??= new List<BetCreditHistoryEntity>();

        partialUMServiceMock
            .Setup(s => s.ThrowIfUM(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        repositoryMock
            .Setup(r => r.FetchAsync<BetCreditHistoryEntity>(It.IsAny<BetCreditHistoryCriteria>(), string.Empty))
            .ReturnsAsync(results);

        repositoryMock
            .Setup(r => r.FetchAsync<LogAction>(It.IsAny<GetLogActionsCriteria>(), string.Empty))
            .ReturnsAsync(new List<LogAction>());
    }

    private void SetupLogActionCache(LogAction logAction)
    {
        var logActions = new Dictionary<int, LogAction> { { logAction.Id, logAction } };
        var cacheEntry = new Mock<ICacheEntry>();

        memoryCacheMock
            .Setup(m => m.CreateEntry(It.IsAny<object>()))
            .Returns(cacheEntry.Object);

        memoryCacheMock
            .Setup(m => m.TryGetValue(It.IsAny<object>(), out It.Ref<Dictionary<int, LogAction>>.IsAny))
            .Callback(new TryGetValueCallback((object key, out Dictionary<int, LogAction> value) =>
            {
                value = logActions;
            }))
            .Returns(true);
    }

    private void SetupEmptyLogActionCache()
    {
        var cacheEntry = new Mock<ICacheEntry>();

        memoryCacheMock
            .Setup(m => m.CreateEntry(It.IsAny<object>()))
            .Returns(cacheEntry.Object);

        memoryCacheMock
            .Setup(m => m.TryGetValue(It.IsAny<object>(), out It.Ref<Dictionary<int, LogAction>>.IsAny))
            .Callback(new TryGetValueCallback((object key, out Dictionary<int, LogAction> value) =>
            {
                value = new Dictionary<int, LogAction>();
            }))
            .Returns(true);
    }

    private delegate void TryGetValueCallback(object key, out Dictionary<int, LogAction> value);
}