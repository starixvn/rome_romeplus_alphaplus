using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.BetLists.Shared.Services;
using AlphaPlus.IPInfo.Services;
using AlphaPlus.PartialUM.Services;
using AlphaPlus.Shared.AppResources.Betlist.Services;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Entities;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.WinLoss;

public interface IWinLossService
{
    Task<BetListData> GetBetList(WinLossParam param);

    Task<IEnumerable<WinLossWeekModel>> GetWeekData(WinLossWeekParam param);
}

public class WinLossService(
    IReportDateService reportDateService,
    IPartialUMService partialUMService,
    IDynamicRepository repository,
    IIPInfoService ipInfoService,
    IPService ipService) : BetListServiceBase(repository, ipInfoService, ipService), IWinLossService
{
    private readonly IReportDateService reportDateService = reportDateService;
    private readonly IPartialUMService partialUMService = partialUMService;

    public async Task<BetListData> GetBetList(WinLossParam param)
    {
        var balanceUpDate = await reportDateService.GetBalanceUpDay();

        if (param.FromDate < balanceUpDate)
        {
            var previousBalanceUpDate = balanceUpDate.AddDays(-1);

            if (param.ToDate <= previousBalanceUpDate)
            {
                return await GetBetListBeforeBalanceUpDay(param, param.FromDate, param.ToDate);
            }
            else
            {
                var betListData = await GetBetList(param, fromDate: balanceUpDate);
                var bettrans14Tickets = betListData.OutputParams.ErrCode != 0
                    ? betListData.OutputParams.MaxTotalRow
                    : betListData.Total;
                if (bettrans14Tickets <= param.MaxTotalRowConfig)
                {
                    param.MaxTotalRow = param.MaxTotalRow - bettrans14Tickets + 1;
                }

                var oldBetListData = await GetBetListBeforeBalanceUpDay(param, param.FromDate, toDate: previousBalanceUpDate);
                var totalTickets = (oldBetListData.OutputParams.ErrCode != 0
                    ? oldBetListData.OutputParams.MaxTotalRow
                    : oldBetListData.Total) + bettrans14Tickets;

                if (totalTickets > param.MaxTotalRowConfig)
                {
                    if (!param.IsExcel)
                    {
                        betListData = oldBetListData = new BetListData(
                            [],
                            [],
                            new BetListOutputParams(100,
                                string.Format(Resources.ExceededMaxTotalRowWithExcelFunc, totalTickets, param.MaxTotalRowConfig),
                                param.MaxTotalRowConfig));
                    }
                    else
                    {
                        var remainingTickets = param.MaxTotalRowConfig - bettrans14Tickets;
                        var ticketDto = oldBetListData.Tickets.Take(remainingTickets);
                        var ticketData = oldBetListData.TicketData.Take(remainingTickets);
                        oldBetListData = new BetListData(
                            ticketDto,
                            ticketData,
                            new BetListOutputParams(100,
                                string.Format(Resources.ExceededMaxTotalRowExportExcelFunc, param.MaxTotalRowConfig),
                                param.MaxTotalRowConfig));
                    }
                }

                return new BetListData(
                    betListData.Tickets.Concat(oldBetListData.Tickets).ToList(),
                    betListData.TicketData.Concat(oldBetListData.TicketData).ToList(),
                    oldBetListData.OutputParams);
            }
        }
        else
        {
            return await GetBetList(param, param.FromDate);
        }
    }

    private async Task<BetListData> GetBetList(WinLossParam param, DateTime fromDate)
    {
        var criteria = new GetBetListWinLossByDateCriteria(
            param.CustId,
            param.ProductIds,
            param.ProductDetailsXML,
            fromDate,
            param.ToDate,
            await reportDateService.GetMoveBetsDay(),
            maxTotalRow: param.MaxTotalRow,
            isExportExcel: param.IsExcel);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        var (tickets, ticketData) = await repository.FetchMultipleAsync<TicketDto, TicketData>(criteria, param.IP);
        await AssignCountry(tickets, param.DisplayVNIP);

        var errFormat = param.IsExcel
            ? string.Format(Resources.ExceededMaxTotalRowExportExcelFunc, param.MaxTotalRowConfig)
            : string.Format(Resources.ExceededMaxTotalRowWithExcelFunc, criteria.MaxTotalRow, param.MaxTotalRowConfig);
        var outputParams = new BetListOutputParams(criteria.ErrCode, criteria.ErrCode != 0 ? errFormat : criteria.ErrMsg, criteria.MaxTotalRow);

        return new BetListData(tickets, ticketData, outputParams);
    }

    private async Task<BetListData> GetBetListBeforeBalanceUpDay(WinLossParam param, DateTime fromDate, DateTime toDate)
    {
        var criteria = new GetBetListWinLossByDateOldCriteria(
            param.CustId,
            param.ProductIds,
            param.ProductDetailsXML,
            fromDate,
            toDate,
            maxTotalRow: param.MaxTotalRow,
            isExportExcel: param.IsExcel);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        var betListData = await GetBetList(criteria, param.DisplayVNIP);
        var errFormat = param.IsExcel
            ? string.Format(Resources.ExceededMaxTotalRowExportExcelFunc, param.MaxTotalRowConfig)
            : string.Format(Resources.ExceededMaxTotalRowWithExcelFunc, criteria.MaxTotalRow, param.MaxTotalRowConfig);
        betListData.OutputParams = new BetListOutputParams(criteria.ErrCode, criteria.ErrCode != 0 ? errFormat : criteria.ErrMsg, criteria.MaxTotalRow);

        return betListData;
    }

    public async Task<IEnumerable<WinLossWeekModel>> GetWeekData(WinLossWeekParam param)
    {
        var reportDate = await reportDateService.GetBalanceUpDayAndMoveBetsDay();
        var criteria = new GetWinLossWeekDataCriteria(
            param.ViewerId,
            param.FromDate,
            param.ToDate,
            reportDate.BalanceUpDay,
            reportDate.MoveBetsDay,
            param.ProductIds,
            param.ProductDetailsXML,
            param.Username);

        return await repository.FetchAsync<WinLossWeekModel>(criteria, param.IP);
    }
}