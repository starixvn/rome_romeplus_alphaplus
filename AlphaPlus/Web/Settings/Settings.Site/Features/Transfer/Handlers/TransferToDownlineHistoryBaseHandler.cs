using AlphaPlus.Settings.Transfer.API.Models.TransferToDownlineHistory;
using AlphaPlus.Settings.Transfer.API.Params.TransferToDownlineHistory;
using AlphaPlus.Settings.Transfer.API.Services;

namespace AlphaPlus.Settings.Site.Features.Transfer.Handlers;

public class TransferToDownlineHistoryBaseHandler(TransferToDownlineHistoryService transferToDownlineHistoryService) : ExcelBaseHandler
{
    private readonly TransferToDownlineHistoryService transferToDownlineHistoryService = transferToDownlineHistoryService;

    protected internal Task<(IEnumerable<TransferToDownlineHistoryModel>, int)> GetTransferToDownlineHistory(TransferToDownlineHistoryPagingParam param)
        => transferToDownlineHistoryService.GetTransferToDownlineHistory(param);
}