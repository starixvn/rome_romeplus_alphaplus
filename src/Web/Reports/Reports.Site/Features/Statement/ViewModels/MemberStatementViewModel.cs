using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Utils;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Statement;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.Statement;

public class MemberStatementViewModel(MemberStatementQuery message, IEnumerable<StatementModel> statements)
    : SMAStatementViewModel(message, statements, $"{Resources.DailyStatement} - {message.CustName}")
{
    private static string ticketUrl = MicroSites.TicketUrl();

    public string ProductIds { get; } = message.ProductIds;

    public bool IsDirectMember { get; } = message.IsDirectMember;

    public string BetListLink => $"{ticketUrl}DailyStatement/Index";

    public string BuildBetListLink()
    {
        var queryString = QueryString.Create(new[] {
            KeyValuePair.Create(nameof(message.CustId), message.CustId.ToString()),
            KeyValuePair.Create(nameof(message.CustName), message.CustName),
            KeyValuePair.Create("FromDate", SystemTime.Yesterday.ToShortDateWithSlash()),
            KeyValuePair.Create("ToDate", SystemTime.Today.ToShortDateWithSlash()),
            KeyValuePair.Create("GetAll", "true"),
            KeyValuePair.Create(nameof(ProductIds), ProductIds),
            KeyValuePair.Create("UserSelectedDateRangeOption", DateRangePickerViewModel.SpecificDateOptionValue.ToString())
        });

        return $"{ticketUrl}MemberBetList/Index{queryString}";
    }

    public string BuildBetListRunningLink()
    {
        var queryString = QueryString.Create(new[] {
            KeyValuePair.Create(nameof(message.CustId), message.CustId.ToString()),
            KeyValuePair.Create(nameof(message.CustName), message.CustName),
            KeyValuePair.Create("FromDate", SystemTime.Today.AddDays(-7).ToShortDateWithSlash()),
            KeyValuePair.Create("ToDate", SystemTime.Today.ToShortDateWithSlash()),
            KeyValuePair.Create("GetAll", "true"),
            KeyValuePair.Create(nameof(ProductIds), ProductIds)
        });

        return $"{ticketUrl}RunningBetList/Index{queryString}";
    }

    public bool IsNegative(decimal? cellValue) 
    {
        if (!cellValue.HasValue) 
        {
            return false;
        }
        var rounded = Math.Round(cellValue.Value, 2, MidpointRounding.AwayFromZero);
        return rounded < 0;
    }
}
