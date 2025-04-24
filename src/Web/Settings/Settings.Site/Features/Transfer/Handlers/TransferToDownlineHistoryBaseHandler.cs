using AlphaPlus.Core.Extensions;
using AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferToDownlineHistory;
using AlphaPlus.Settings.Transfer.API.Models.TransferToDownlineHistory;
using AlphaPlus.Settings.Transfer.API.Params.TransferToDownlineHistory;
using AlphaPlus.Settings.Transfer.API.Services;

namespace AlphaPlus.Settings.Site.Features.Transfer.Handlers;

public class TransferToDownlineHistoryBaseHandler(TransferToDownlineHistoryService transferToDownlineHistoryService) : ExcelBaseHandler
{
    private readonly TransferToDownlineHistoryService transferToDownlineHistoryService = transferToDownlineHistoryService;

    protected internal Task<(IEnumerable<TransferToDownlineHistoryModel>, int)> GetTransferToDownlineHistory(TransferToDownlineHistoryPagingQuery message)
    {
        var param = new TransferToDownlineHistoryParam(
            message.CustId,
            message.UserName,
            message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
            message.ToDate,
            message.LevelType,
            message.PageSize,
            message.PageIndex);

        return transferToDownlineHistoryService.GetTransferToDownlineHistory(param);
    }
}