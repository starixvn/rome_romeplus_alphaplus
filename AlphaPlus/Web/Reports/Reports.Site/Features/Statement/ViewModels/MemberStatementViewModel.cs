using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Statement;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

namespace AlphaPlus.Reports.Site.Features.Statement;

public class MemberStatementViewModel(MemberStatementQuery message, IEnumerable<StatementModel> statements)
    : SMAStatementViewModel(message, statements, $"{Resources.DailyStatement} - {message.CustName}")
{
    public string ProductIds { get; } = message.ProductIds;

    public string BetListLink { get; } = "/tickets/DailyStatement/Index";

    public string BuildBetListLink()
    {
        var today = DateTime.Today;
        var queryString = QueryString.Create(new[] {
            KeyValuePair.Create(nameof(message.CustId), message.CustId.ToString()),
            KeyValuePair.Create(nameof(message.CustName), message.CustName),
            KeyValuePair.Create("FromDate", today.Yesterday().ToShortDateWithSlash()),
            KeyValuePair.Create("ToDate", today.ToShortDateWithSlash()),
            KeyValuePair.Create("GetAll", "true"),
            KeyValuePair.Create(nameof(ProductIds), ProductIds),
            KeyValuePair.Create("UserSelectedDateRangeOption", DateRangePickerViewModel.SpecificDateOptionValue.ToString())
        });

        return $"/tickets/MemberBetList/Index{queryString}";
    }

    public string BuildBetListRunningLink()
    {
        var today = DateTime.Today;
        var queryString = QueryString.Create(new[] {
            KeyValuePair.Create(nameof(message.CustId), message.CustId.ToString()),
            KeyValuePair.Create(nameof(message.CustName), message.CustName),
            KeyValuePair.Create("FromDate", today.OneWeekAgo().ToShortDateWithSlash()),
            KeyValuePair.Create("ToDate", today.ToShortDateWithSlash()),
            KeyValuePair.Create("GetAll", "true"),
            KeyValuePair.Create(nameof(ProductIds), ProductIds)
        });

        return $"/tickets/RunningBetList/Index{queryString}";
    }

    public bool IsNegative(decimal? cellValue) => cellValue.HasValue && cellValue.Value < 0;
}
