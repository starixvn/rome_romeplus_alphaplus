using AlphaPlus.Reports.Features.CommissionByBetType;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.CommissionByBetType;

public class CommissionByBetTypeQuery : DefaultDateRangeQuery, IRequest<CommissionByBetTypeViewModel>
{
    public CommissionByBetTypeQuery() : base()
    {
        MinDate = ReportDateService.Instance.GetBalanceUpDay().GetAwaiter().GetResult();
        IsShowMyanmarOdds = Viewer.IsMyanmarOdds;
    }

    public string SortPropertyName { get; set; } = nameof(CommissionByBetTypeModel.CustName);

    public byte DescendingSort { get; set; }

    public bool IsShowMyanmarOdds { get; }
}