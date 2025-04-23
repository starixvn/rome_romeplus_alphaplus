using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.BetCreditTracking.BetCreditHistory;
using Fanex.Data.Repository;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;

namespace AlphaPlus.Reports.UnitTests.Features.BetCreditTracking.BetCreditHistory;

[Trait("Category", "BetCreditHistory")]
public class BetCreditHistoryServiceTests
{
    private readonly IDynamicRepository repository;
    private readonly IPartialUMService partialUMService;
    private readonly IMemoryCache cacheService;
    private readonly BetCreditHistoryService service;

    public BetCreditHistoryServiceTests()
    {
        repository = Substitute.For<IDynamicRepository>();
        partialUMService = Substitute.For<IPartialUMService>();
        cacheService = Substitute.For<IMemoryCache>();

        service = new BetCreditHistoryService(
            repository,
            partialUMService,
            cacheService);
    }

    [Fact]
    public async Task GetBetCreditHistory_ShouldReturnCorrectData()
    {
        // Arrange
        var param = new BetCreditHistoryParam
        (
            Member: "testuser",
            FromDate: new DateTime(2025, 1, 1),
            ToDate: new DateTime(2025, 1, 31),
            RefNo: 123456789,
            ViewerId: 1,
            PageIndex: 0,
            PageSize: 10,
            SiteName: "testsite",
            Language: "en"
        );
        var criteria = new BetCreditHistoryCriteria(
            param.Member,
            param.FromDate,
            param.ToDate,
            param.RefNo,
            param.ViewerId,
            param.PageIndex,
            param.PageSize);
        var rawdata = new List<BetCreditHistoryEntity>
        {
            new BetCreditHistoryEntity
            {
                ActionDate = new DateTime(2025, 3, 17),
                WinLostDate = new DateTime(2025, 3, 16),
                RefNo = 123456789,
                ActualStake = 100.50m,
                WinLoss = 50.25m,
                PlayerComm = 10.75m,
                BalanceChanged = 20.00m,
                NewBalance = 120.50m,
                GivenCredit = 30.00m,
                BetCredit = 15.00m,
                Action = "TestAction",
                ActionId = 1,
                IsUserLogActionId = true
            }
        };

        repository.FetchAsync<BetCreditHistoryEntity>(Arg.Any<BetCreditHistoryCriteria>()).Returns(rawdata);

        // Act
        var result = await service.GetBetCreditHistory(param);

        // Assert
        Assert.Equal(criteria.ErrorCode, result.ErrorCode);
        Assert.Equal(criteria.ErrorMess, result.ErrorMessage);
        Assert.Equal(rawdata, result.ReportData);
        Assert.Equal(criteria.TotalRows, result.TotalRows);
    }
}
