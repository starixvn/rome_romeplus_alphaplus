using AlphaPlus.Reports.Features.CashBillingService.Models;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.WinLoss;

public class CashFeeGetDataQuery : WinLossQuery, IRequest<CashFeeModel>
{
}
