using AlphaPlus.Reports.Features.CashBillingService.Models;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.WinLossDetail.Requests;

public class CashFeeGetDataQuery : WinLossDetailQuery, IRequest<CashFeeModel>
{
}
