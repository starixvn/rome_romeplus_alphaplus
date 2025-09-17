using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.BetCreditTracking.MoveStatementHistory;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.BetCreditTracking;

public class MoveStatementHistoryViewModel(MoveStatementHistoryQuery message, MoveStatementHistoryModel model = null)
{
    public UserLevel CustLevel { get; } = message.ViewerLevel;

    public bool IsFilterVisible { get; } = message.IsFilterVisible;

    public bool IsMainReport { get; } = message.IsMainReport;

    public string Member { get; } = message.Member;

    public string ErrorMessage { get; } = model?.ErrorMessage;

    public bool CanViewBetList { get; } = !message.IsProtectedMode || !message.ViewerLevel.IsAgentLevel();

    public MonthPickerViewModel MonthPicker { get; }
        = new MonthPickerViewModel(message.MinDate, message.FromDate, message.ToDate, true);

    public string ViewDataInstructionMessage
        => string.Format(Resources.CanOnlyViewDataAfterTimeHistoryReport, message.MinDate.ToShortDateWithSlash());

    public IEnumerable<MoveStatementHistoryByMember> ReportData { get; } = model?.ReportData;

    public MoveStatementHistorySummary Summary { get; } = model?.Summary;

    public PagerViewModel Pager { get; }
        = new PagerViewModel(message.PageIndex, message.PageSize, model?.TotalRows ?? 0);

    public bool IsShowDuplicatedTicketMessage
        => ReportData != null && ReportData.Any(member => member.RefNoRow.Any(row => row.IsMultipleMoved));

    public IDictionary<string, string> GetExcelParams() => new Dictionary<string, string>
    {
        [nameof(Member)] = Member,
        [nameof(IsMainReport)] = IsMainReport.ToString(),
        [nameof(MonthPicker.FromDate)] = message.FromDate.ToShortDateWithSlash(),
        [nameof(MonthPicker.ToDate)] = MonthPicker.ToDate.ToShortDateWithSlash()
    };

    public string GetBetListUrl(int custId, string custName, long transId)
    {
        var queryString = QueryString.Create(
        [
            KeyValuePair.Create(nameof(custId), custId.ToString()),
            KeyValuePair.Create(nameof(custName), custName),
            KeyValuePair.Create(nameof(transId), transId.ToString())
        ]);

        return $"{BetListPath}{queryString}";
    }

    public string GetUserGuideLink()
    {
        if (AppSettings.SupportedNewHandbook)
        {
            var handbookPath = "HandbookDefault/GetBySlug?slug=bet-credit-tracking";
            return MicroSites.FeatureUrl($"~/{handbookPath}");
        }

        return AppSettings.IsSiteLabel789y
            ? MicroSites.FeatureUrl("~/StaticMessage/Guide?id=167296")
            : MicroSites.FeatureUrl("~/Index.aspx?linktomessagesite=167296");
    }

    private static string BetListPath = MicroSites.TicketUrl("~/MoveStatementHistory/Index");

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
