using AlphaPlus.Reports.Features.CommissionByBetType;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.CommissionByBetType;

public class CommissionByBetTypeQuery : BalanceUpDateRangeQuery, IRequest<CommissionByBetTypeViewModel>
{
    public CommissionByBetTypeQuery() : base()
        => IsShowMyanmarOdds = Viewer.IsMyanmarOdds;


    public string SortPropertyName { get; set; } = nameof(CommissionByBetTypeModel.CustName);

    public byte DescendingSort { get; set; }

    public bool IsShowMyanmarOdds { get; }
}