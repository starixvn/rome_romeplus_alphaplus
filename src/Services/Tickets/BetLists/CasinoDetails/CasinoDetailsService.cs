using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.BetLists.Shared.Services;
using AlphaPlus.IPInfo.Services;
using AlphaPlus.Utilities.IP;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.CasinoDetails;

public interface ICasinoDetailsService
{
    Task<PagedReportData<TicketDto>> GetBetList(CasinoDetailsParam param);
    Task<PagedReportData<TicketDto>> GetLiveCasinoBetList(CasinoDetailsParam param);
    Task<PagedReportData<TicketDto>> GetRNGBetList(CasinoDetailsParam param);
}

public class CasinoDetailsService(
    IReportDateService reportDateService,
    IDynamicRepository repository,
    IIPInfoService ipInfoService,
    IPService ipService) : BetListServiceBase(repository, ipInfoService, ipService), ICasinoDetailsService
{
    private readonly IReportDateService reportDateService = reportDateService;

    public async Task<PagedReportData<TicketDto>> GetBetList(CasinoDetailsParam param)
    {
        var reportDate = await reportDateService.GetBalanceUpDayAndMoveBetsDay();
        var criteria = new GetCasinoDetailsCriteria(
            param.RefNo,
            param.CustId,
            param.TransType,
            param.ProductId,
            param.PageIndex,
            param.PageSize,
            reportDate.MoveBetsDay,
            reportDate.BalanceUpDay);
        var tickets = await repository.FetchAsync<TicketDto>(criteria);
        var shouldHideIP = await ipService.CheckHideIP(param.SuperId, param.System);
        AssignIP(tickets, shouldHideIP);
        await AssignCountry(tickets, param.DisplayVNIP, shouldHideIP);

        return new PagedReportData<TicketDto>(tickets, criteria.PageIndex, criteria.PageSize, criteria.TotalRows);
    }
    public async Task<PagedReportData<TicketDto>> GetLiveCasinoBetList(CasinoDetailsParam param)
    {
        var reportDate = await reportDateService.GetBalanceUpDayAndMoveBetsDay();
        var criteria = new GetLiveCasinoDetailsCriteria(
            param.RefNo,
            param.CustId,
            param.TransType,
            param.ProductId,
            param.PageIndex,
            param.PageSize,
            reportDate.MoveBetsDay,
            reportDate.BalanceUpDay);
        var tickets = await repository.FetchAsync<TicketDto>(criteria);
        var shouldHideIP = await ipService.CheckHideIP(param.SuperId, param.System);
        AssignIP(tickets, shouldHideIP);
        await AssignCountry(tickets, param.DisplayVNIP, shouldHideIP);

        return new PagedReportData<TicketDto>(tickets, criteria.PageIndex, criteria.PageSize, criteria.TotalRows);
    }
    public async Task<PagedReportData<TicketDto>> GetRNGBetList(CasinoDetailsParam param)
    {
        var reportDate = await reportDateService.GetBalanceUpDayAndMoveBetsDay();
        var criteria = new GetRNGCasinoDetailsCriteria(
            param.RefNo,
            param.CustId,
            param.TransType,
            param.ProductId,
            param.PageIndex,
            param.PageSize,
            reportDate.MoveBetsDay,
            reportDate.BalanceUpDay);
        var tickets = await repository.FetchAsync<TicketDto>(criteria);
        await AssignCountry(tickets, param.DisplayVNIP);
        await HideIP(tickets, param.SuperId, param.System);

        return new PagedReportData<TicketDto>(tickets, criteria.PageIndex, criteria.PageSize, criteria.TotalRows);
    }
}