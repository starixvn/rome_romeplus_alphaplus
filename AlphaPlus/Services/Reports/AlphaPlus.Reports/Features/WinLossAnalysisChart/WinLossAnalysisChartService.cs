using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.Shared;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.WinLossAnalysisChart;

public interface IWinLossAnalysisChartService
{
    Task<IEnumerable<WinLossAnalysisChartModel>> GetWinLossAnalysisChart(WinLossAnalysisChartParam param);
}

public class WinLossAnalysisChartService(
    IReportDateService reportDateService,
    IDynamicRepository repository,
    IPartialUMService partialUMService) : IWinLossAnalysisChartService
{
    public async Task<IEnumerable<WinLossAnalysisChartModel>> GetWinLossAnalysisChart(WinLossAnalysisChartParam param)
    {
        var reportDate = await reportDateService.GetBalanceUpDayAndMoveBetsDay();
        var criteria = new WinLossAnalysisChartCriteria(
            param.CustId,
            param.FromDate,
            param.ToDate,
            reportDate.BalanceUpDay,
            reportDate.MoveBetsDay,
            param.ProductIds,
            param.CustLevelId);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        var rawdata = await repository.FetchAsync<WinLossAnalysisChartModel>(criteria);

        return AddEmptyWinlossInLimitedDate(rawdata, param);
    }

    private static IEnumerable<WinLossAnalysisChartModel> AddEmptyWinlossInLimitedDate(
        IEnumerable<WinLossAnalysisChartModel> winloss,
        WinLossAnalysisChartParam param)
    {
        var winlossToList = winloss.ToList();
        for (var date = param.ToDate; date >= param.FromDate; date = date.AddMonths(-1))
        {
            var winlossDate = date.ToString("yyyy-MM");
            var isNotExisted = !winlossToList.Any(i => i.WinLossDate.Equals(winlossDate));

            if (isNotExisted)
            {
                winlossToList.Insert(0, new WinLossAnalysisChartModel
                {
                    WinLossDate = winlossDate,
                    Turnover = 0,
                    WinLoss = 0
                });
            }
        }

        return winlossToList;
    }
}