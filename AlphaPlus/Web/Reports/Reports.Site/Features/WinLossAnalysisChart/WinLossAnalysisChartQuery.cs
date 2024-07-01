using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Utils;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.WinLossAnalysisChart;

public class WinLossAnalysisChartQuery : DefaultDateRangeQuery, IRequest<WinLossAnalysisChartViewModel>
{
    private const int DefaultLoadingMonths = 2;

    public WinLossAnalysisChartQuery() : base()
    {
        var today = SystemTime.Today;
        if (today.IsEndOfMonth())
        {
            FromDate = today.AddMonths(-2).FirstDayOfMonth();
            ToDate = today;
        }
        else
        {
            FromDate = today.AddMonths(-3).FirstDayOfMonth();
            ToDate = today.AddMonths(-1).LastDayOfMonth();
        }

        MinDate = today.AddMonths(-18).FirstDayOfMonth();
        CurrencyName = Viewer.Currency;
        DarkModeName = Viewer.DarkModeName;
    }

    public string UserSelectedProductIds { get; set; } = "*";

    public string CurrencyName { get; }

    public string DarkModeName { get; }

    public override bool IsValid()
        => ToDate.GetMonthDiff(FromDate) >= DefaultLoadingMonths && base.IsValid();
}