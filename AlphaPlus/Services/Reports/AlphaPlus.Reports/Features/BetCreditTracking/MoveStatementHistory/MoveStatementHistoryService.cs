using AlphaPlus.PartialUM.Services;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.BetCreditTracking.MoveStatementHistory;

public interface IMoveStatementHistoryService
{
    Task<MoveStatementHistoryModel> GetMoveStatementHistory(MoveStatementHistoryParam param);
}

public class MoveStatementHistoryService(
    IDynamicRepository repository,
    IPartialUMService partialUMService) : IMoveStatementHistoryService
{
    private readonly IDynamicRepository repository = repository;
    private readonly IPartialUMService partialUMService = partialUMService;

    public async Task<MoveStatementHistoryModel> GetMoveStatementHistory(MoveStatementHistoryParam param)
    {
        var criteria = new MoveStatementHistoryCriteria(
            param.Username,
            param.FromDate,
            param.ToDate,
            param.ViewerId,
            param.PageIndex,
            param.PageSize,
            param.IsVariance);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        var (moveStatementHistories, totalMoveStatementHistories, summaryMoveStatementHistories)
            = await repository.FetchMultipleAsync<MoveStatementHistoryByRefNo, MoveStatementHistoryByRefNo, MoveStatementHistorySummary>(criteria);
        var reportData = Enumerable.Empty<MoveStatementHistoryByMember>();
        var summaryData = new MoveStatementHistorySummary();

        if (criteria.TotalRows > 0)
        {
            var memberRowHash = totalMoveStatementHistories.ToDictionary(kvp => kvp.CustId, kvp => kvp);

            reportData = moveStatementHistories.GroupBy(
                r => new { r.CustId, r.Username },
                (key, g) => new MoveStatementHistoryByMember(
                    key.Username,
                    memberRowHash.TryGetValue(key.CustId, out var memberRow) ? memberRow : new MoveStatementHistoryByRefNo(),
                    g));
            summaryData = summaryMoveStatementHistories.FirstOrDefault();
        }

        return new MoveStatementHistoryModel(criteria.ErrorCode, criteria.ErrorMess, reportData, summaryData, criteria.TotalRows);
    }
}