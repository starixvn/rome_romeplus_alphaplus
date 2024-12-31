using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.Shared.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.WhiteLabel;

public interface IWhiteLabelService
{
    Task<IEnumerable<IdNameModel>> GetWhiteLabels(int custId, string siteName);

    Task<ReportData<SettlementModel>> GetSettlement(WhiteLabelParam param);

    Task<ReportData<DailyWinLossModel>> GetDailyWinLoss(WhiteLabelParam param);
}

public class WhiteLabelService(IDynamicRepository repository, IPartialUMService partialUMService) : IWhiteLabelService
{
    private readonly IDynamicRepository repository = repository;
    private readonly IPartialUMService partialUMService = partialUMService;

    public Task<IEnumerable<IdNameModel>> GetWhiteLabels(int custId, string siteName)
    {
        var criteria = new GetWhiteLabelsCriteria(custId);
        partialUMService.ThrowIfUM(criteria.GetSettingKey(), siteName);

        return repository.FetchAsync<IdNameModel>(criteria);
    }

    public async Task<ReportData<SettlementModel>> GetSettlement(WhiteLabelParam param)
    {
        var criteria = new GetSettlementCriteria(
            param.ViewerId,
            param.FromDate,
            param.ToDate,
            param.WhiteLabels,
            param.Username);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        var settlement = await repository.FetchAsync<SettlementModel>(criteria);

        return new ReportData<SettlementModel>(settlement, AccumulateSettlementTotal(settlement));
    }

    private static SettlementModel AccumulateSettlementTotal(IEnumerable<SettlementModel> settlement)
    {
        var total = new SettlementModel();
        foreach (var item in settlement)
        {
            total += item;
        }

        return total;
    }

    public async Task<ReportData<DailyWinLossModel>> GetDailyWinLoss(WhiteLabelParam param)
    {
        var criteria = new GetDailyWinLossCriteria(param.ViewerId, param.FromDate, param.ToDate, param.WhiteLabels, param.Username);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        var winloss = await repository.FetchAsync<DailyWinLossModel>(criteria);

        return new ReportData<DailyWinLossModel>(winloss, AccumulateDailyWinLossTotal(winloss));
    }

    private static DailyWinLossModel AccumulateDailyWinLossTotal(IEnumerable<DailyWinLossModel> winloss)
    {
        var total = new DailyWinLossModel();
        foreach (var item in winloss)
        {
            total += item;
        }

        return total;
    }
}