using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.BetLists.Shared.Services;
using AlphaPlus.IPInfo.Services;
using AlphaPlus.PartialUM.Services;
using AlphaPlus.Utilities.IP;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.CancelledBets;

public interface ICancelledBetService
{
    Task<PagedReportData<BetListData>> GetCancelledBet(CancelledBetParam param);
}

public class CancelledBetService(
    IPartialUMService partialUMService,
    IDynamicRepository repository,
    IIPInfoService ipInfoService,
    IPService ipService) : BetListServiceBase(repository, ipInfoService, ipService), ICancelledBetService
{
    private readonly IPartialUMService partialUMService = partialUMService;

    public async Task<PagedReportData<BetListData>> GetCancelledBet(CancelledBetParam param)
    {
        var criteria = new GetCancelledBetCriteria(
            param.CustId,
            param.FromDate,
            param.ToDate,
            param.MoveBetsDay,
            param.RoleId,
            param.ProductIds,
            param.StatusIds,
            param.PageIndex,
            param.PageSize);
        var betlistData = await GetBetList(criteria, param.DisplayVNIP);

        return new PagedReportData<BetListData>([betlistData], criteria.PageIndex, criteria.PageSize, criteria.TotalRows);
    }
}