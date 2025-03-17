using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.BetCreditTracking.MoveStatementHistory;
using Fanex.Data.Repository;
using NSubstitute;

namespace AlphaPlus.Reports.UnitTests.Features.BetCreditTracking.MoveStatementHistory;

[Trait("Category", "MoveStatementHistory")]
public class MoveStatementHistoryServiceTests
{
    private readonly IDynamicRepository repository;
    private readonly IPartialUMService partialUMService;
    private readonly MoveStatementHistoryService service;

    public MoveStatementHistoryServiceTests()
    {
        repository = Substitute.For<IDynamicRepository>();
        partialUMService = Substitute.For<IPartialUMService>();

        service = new MoveStatementHistoryService(
            repository,
            partialUMService);
    }

    [Fact]
    public async Task GetMoveStatementHistory_ShouldReturnCorrectData()
    {
        // Arrange
        var param = new MoveStatementHistoryParam
        (
            Username: "testuser",
            FromDate: new DateTime(2025, 1, 1),
            ToDate: new DateTime(2025, 1, 31),
            ViewerId: 1,
            PageIndex: 0,
            PageSize: 10,
            IsVariance: true,
            SiteName: "testsite"
        );
        var criteria = new MoveStatementHistoryCriteria(
            param.Username,
            param.FromDate,
            param.ToDate,
            param.ViewerId,
            param.PageIndex,
            param.PageSize,
            param.IsVariance);
        var moveStatementHistories = new List<MoveStatementHistoryByRefNo>
        {
            new MoveStatementHistoryByRefNo
            {
                CustId = 1,
                Username = "testuser",
                MemberWinLoss = 100.50m,
                MemberComm = 10.75m,
                MemberTotal = 111.25m,
                IsDirectMember = true
            }
        };
        var totalMoveStatementHistories = new List<MoveStatementHistoryByRefNo>
        {
            new MoveStatementHistoryByRefNo
            {
                CustId = 1,
                Username = "testuser",
                MemberWinLoss = 100.50m,
                MemberComm = 10.75m,
                MemberTotal = 111.25m,
                IsDirectMember = true
            }
        };
        var summaryMoveStatementHistories = new List<MoveStatementHistorySummary>
        {
            new MoveStatementHistorySummary
            {
                MemberWinLoss = 100.50m,
                MemberComm = 10.75m,
                MemberTotal = 111.25m
            }
        };

        partialUMService.When(x => x.ThrowIfUM(criteria.GetSettingKey(), param.SiteName)).DoNotCallBase();
        repository.FetchMultipleAsync<MoveStatementHistoryByRefNo, MoveStatementHistoryByRefNo, MoveStatementHistorySummary>(criteria)
            .Returns((moveStatementHistories, totalMoveStatementHistories, summaryMoveStatementHistories));

        // Act
        var result = await service.GetMoveStatementHistory(param);

        // Assert
        Assert.Equal(criteria.ErrorCode, result.ErrorCode);
        Assert.Equal(criteria.ErrorMess, result.ErrorMessage);
        Assert.Single(result.ReportData);
        Assert.Equal(moveStatementHistories.First().Username, result.ReportData.First().MemberRow.Username);
        Assert.Equal(summaryMoveStatementHistories.First().MemberTotal, result.Summary.MemberTotal);
        Assert.Equal(criteria.TotalRows, result.TotalRows);
    }
}
