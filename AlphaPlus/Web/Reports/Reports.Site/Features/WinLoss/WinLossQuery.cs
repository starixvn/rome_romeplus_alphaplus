using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.WinLoss;

public class WinLossQuery : ProductDetailDateRangeQuery, IRequest<WinLossViewModel>
{
}