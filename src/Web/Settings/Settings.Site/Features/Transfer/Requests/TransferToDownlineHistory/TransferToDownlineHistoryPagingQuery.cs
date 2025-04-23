using MediatR;
using AlphaPlus.Settings.Transfer.API.Constants;
using AlphaPlus.Settings.Site.Features.Transfer.ViewModels.TransferToDownlineHistory;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferToDownlineHistory;

public class TransferToDownlineHistoryPagingQuery : TransferToDownlineHistoryQuery, IRequest<TransferToDownlineHistoryPagingViewModel>
{
    public int PageIndex { get; set; } = PagerConstant.DefaultPageIndex;

    public int PageSize { get; set; } = PagerConstant.DefaultPageSize;
}