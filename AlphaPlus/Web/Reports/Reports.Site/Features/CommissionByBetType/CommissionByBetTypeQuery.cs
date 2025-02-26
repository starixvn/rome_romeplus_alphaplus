using AlphaPlus.Reports.Features.CommissionByBetType;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.CommissionByBetType;

public class CommissionByBetTypeQuery : BalanceUpDateRangeQuery, IRequest<CommissionByBetTypeViewModel>
{
    public string SortPropertyName { get; set; } = nameof(CommissionByBetTypeModel.CustName);

    public byte DescendingSort { get; set; }
}