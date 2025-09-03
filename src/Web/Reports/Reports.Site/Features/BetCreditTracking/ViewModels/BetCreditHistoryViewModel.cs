using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.BetCreditTracking.BetCreditHistory;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

namespace AlphaPlus.Reports.Site.Features.BetCreditTracking;

public class BetCreditHistoryViewModel(BetCreditHistoryQuery message, BetCreditHistoryModel model = null)
{
    public string Member { get; } = message.Member;

    public long? RefNo { get; } = message.RefNo;

    public bool IsFilterVisible { get; } = message.IsFilterVisible;

    public string ErrorMessage { get; } = model?.ErrorMessage;

    public UserLevel CustLevel { get; } = message.ViewerLevel;

    public DateRangePickerViewModel DateRangePicker { get; }
        = new DateRangePickerViewModel(
            message.MinDate,
            DateTime.MaxValue,
            message.FromDate,
            message.ToDate,
            message.UserSelectedDateRangeOption,
            optionValues: [
                DateRangePickerViewModel.TodayOptionValue,
                DateRangePickerViewModel.YesterdayOptionValue,
                DateRangePickerViewModel.ThisWeekOptionValue,
                DateRangePickerViewModel.LastWeekOptionValue,
                DateRangePickerViewModel.SpecificDateOptionValue
            ]);

    public IEnumerable<BetCreditHistoryEntity> ReportData { get; } = model?.ReportData;

    public PagerViewModel Pager { get; }
        = new PagerViewModel(message.PageIndex, message.PageSize, model?.TotalRows ?? 0);

    public IDictionary<string, string> GetExcelParams() => new Dictionary<string, string>
    {
        [nameof(Member)] = Member,
        [nameof(RefNo)] = RefNo?.ToString(),
        [nameof(DateRangePicker.FromDate)] = DateRangePicker.FromDate,
        [nameof(DateRangePicker.ToDate)] = DateRangePicker.ToDate
    };

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
    public bool IsNegative(decimal? cellValue) => cellValue.HasValue && cellValue.Value < 0;
}
