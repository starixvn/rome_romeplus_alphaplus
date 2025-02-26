using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.Statement;

public interface IStatementService
{
    Task<IEnumerable<StatementModel>> GetStatements(GetStatementParam param);

    Task<ReportData<StatementDetailModel>> StatementDetails(int custId, DateTime transferDate);
}

public class StatementService(
    IReportDateService reportDateService,
    IDynamicRepository repository,
    IPartialUMService partialUMService) : IStatementService
{
    private readonly IReportDateService reportDateService = reportDateService;
    private readonly IDynamicRepository repository = repository;
    private readonly IPartialUMService partialUMService = partialUMService;

    public async Task<IEnumerable<StatementModel>> GetStatements(GetStatementParam param)
    {
        var timeRangesTask = reportDateService.GetTimeRangesToGetReportDataBetweenTables(param.FromDate, param.ToDate);
        var balanceUpDayTask = reportDateService.GetBalanceUpDay();
        await Task.WhenAll(timeRangesTask, balanceUpDayTask);

        var timeRanges = await timeRangesTask;
        var balanceUpDay = await balanceUpDayTask;
        var statements = new List<StatementModel>();

        foreach (var timeRange in timeRanges)
        {
            var storeMappingSuffix = await reportDateService.GetStoreMappingSuffix(timeRange.FromDate);
            var criteria = string.Equals(storeMappingSuffix, ReportDateService.BetTrans14SPSuffix, StringComparison.OrdinalIgnoreCase)
                ? new GetStatementBetTrans14Criteria(param.CustId, timeRange.FromDate, timeRange.ToDate, balanceUpDay)
                : new GetStatementCriteria(param.CustId, timeRange.FromDate, timeRange.ToDate, storeMappingSuffix);
            await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

            var rawdata = await repository.FetchAsync<StatementModel>(criteria);
            statements.AddRange(rawdata);
        }

        return statements;
    }

    public async Task<ReportData<StatementDetailModel>> StatementDetails(int custId, DateTime transferDate)
    {
        var balanceUpDay = await reportDateService.GetBalanceUpDay();

        var statements = await repository.FetchAsync<StatementDetailModel>(new TransferDetailsCriteria(
            custId,
            transferDate,
            transferDate >= balanceUpDay ? string.Empty : ReportDateService.WarehouseSPSuffix));
        var totalAmount = statements.Sum(s => s.Amount ?? 0);

        return new ReportData<StatementDetailModel>(statements, new StatementDetailModel { Amount = totalAmount });
    }
}