using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.SuperWinLossDetailByPT;

public class SuperWinLossDetailByPTQuery : ProductDetailDateRangeQuery, IRequest<SuperWinLossDetailByPTViewModel>
{
    public SuperWinLossDetailByPTQuery()
    {
        SortPropertyName = "Turnover";
        DescendingSort = 1;
    }
}