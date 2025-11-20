using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.BetLists.Shared.Services;
using AlphaPlus.IPInfo.Services;
using AlphaPlus.PartialUM.Services;
using AlphaPlus.Utilities.IP;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.CancelledBets;

public interface ICancelledBetService
{
    Task<PagedReportData<BetListData>> GetBetList(CancelledBetParam param);
}

public class CancelledBetService(
    IReportDateService reportDateService,
    IPartialUMService partialUMService,
    IDynamicRepository repository,
    IIPInfoService ipInfoService,
    IPService ipService) : BetListServiceBase(repository, ipInfoService, ipService), ICancelledBetService
{
    private readonly IReportDateService reportDateService = reportDateService;
    private readonly IPartialUMService partialUMService = partialUMService;

    public async Task<PagedReportData<BetListData>> GetBetList(CancelledBetParam param)
    {
        var moveBetsDay = await reportDateService.GetMoveBetsDay();
        var criteria = new GetCancelledBetCriteria(
            param.CustId,
            param.FromDate,
            param.ToDate,
            moveBetsDay,
            param.RoleId,
            param.ProductIds,
            param.StatusIds,
            param.PageIndex,
            param.PageSize);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        var betlistData = await GetBetList(criteria, param.DisplayVNIP, param.SuperId, param.System);

        return new PagedReportData<BetListData>([betlistData], param.PageIndex, param.PageSize, criteria.TotalRows);
    }
}