using AlphaPlus.Reports.Features.Shared;

namespace AlphaPlus.Reports.Site.Features.Shared.Requests;

public class BalanceUpDateRangeQuery : DefaultDateRangeQuery
{
    public BalanceUpDateRangeQuery() : base()
        => MinDate = ReportDateService.Instance.GetBalanceUpDay().GetAwaiter().GetResult();
}