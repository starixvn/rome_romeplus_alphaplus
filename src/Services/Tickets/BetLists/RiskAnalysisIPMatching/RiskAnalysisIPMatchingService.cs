using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.BetLists.Shared.Services;
using AlphaPlus.IPInfo.Services;
using AlphaPlus.PartialUM.Services;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Entities;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.RiskAnalysisIPMatching;

public interface IRiskAnalysisIPMatchingService
{
    Task<BetListData> GetBetList(RiskAnalysisIPMatchingParam param);
}

public class RiskAnalysisIPMatchingService(
    IReportDateService reportDateService,
    IPartialUMService partialUMService,
    IDynamicRepository repository,
    IIPInfoService ipInfoService,
    IPService ipService) :
        BetListServiceBase(repository, ipInfoService, ipService),
        IRiskAnalysisIPMatchingService
{
    private readonly IReportDateService reportDateService = reportDateService;
    private readonly IPartialUMService partialUMService = partialUMService;

    public async Task<BetListData> GetBetList(RiskAnalysisIPMatchingParam param)
    {
        var moveBetsDay = await reportDateService.GetMoveBetsDay();
        var criteria = new GetRiskAnalysisIPMatchingCriteria(
            param.CustId,
            param.IP,
            param.Date,
            moveBetsDay);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        var (tickets, ticketData) = await repository.FetchMultipleAsync<TicketDto, TicketData>(criteria);
        var shouldHideIP = await ipService.CheckHideIP(param.SuperId, param.System);
        AssignIP(tickets, shouldHideIP);
        await AssignCountry(tickets, param.DisplayVNIP, shouldHideIP);

        return new BetListData(tickets.OrderByDescending(ticket => ticket.TransDate).ToList(), ticketData);
    }
}