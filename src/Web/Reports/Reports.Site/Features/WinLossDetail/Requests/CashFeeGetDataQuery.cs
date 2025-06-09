using AlphaPlus.Reports.Features.WinLossDetail.Models;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.WinLossDetail.Requests;

public class CashFeeGetDataQuery : DefaultDateRangeQuery, IRequest<CashFeeModel>
{
}
