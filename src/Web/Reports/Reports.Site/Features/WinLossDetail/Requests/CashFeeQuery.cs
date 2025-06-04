using AlphaPlus.Reports.Site.Features.WinLossDetail.ViewModels;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.WinLossDetail.Requests;

public class CashFeeQuery : WinLossDetailQuery, IRequest<CashFeeViewModel>
{
}
