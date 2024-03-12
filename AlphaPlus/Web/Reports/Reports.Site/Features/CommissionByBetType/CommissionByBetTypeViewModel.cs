using AlphaPlus.Reports.Features.CommissionByBetType;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

namespace AlphaPlus.Reports.Site.Features.CommissionByBetType;

public class CommissionByBetTypeViewModel(
    CommissionByBetTypeQuery message,
    IEnumerable<CommissionByBetTypeModel> reportData,
    CommissionByBetTypeModel total,
    IEnumerable<Breadcrumb> breadcrumbs,
    string defaultBetListPath = null) : DrillDownableReportViewModel<CommissionByBetTypeModel>(message.CustLevel, breadcrumbs, defaultBetListPath)
{
    public const string SortByCustName = nameof(CommissionByBetTypeModel.CustName);
    public const string SortByComm1 = nameof(CommissionByBetTypeModel.Comm1);
    public const string SortByComm2 = nameof(CommissionByBetTypeModel.Comm2);
    public const string SortByComm3 = nameof(CommissionByBetTypeModel.Comm3);
    public const string SortByMyanmarOddsComm = nameof(CommissionByBetTypeModel.MyanmarOddsComm);
    public const string SortByCasinoComm = nameof(CommissionByBetTypeModel.CasinoComm);

    public int CustId { get; } = message.CustId;

    public string CustName { get; } = message.CustName;

    public bool IsShowMyanmarOdds { get; } = message.IsShowMyanmarOdds;

    public PageHeaderViewModel PageHeader { get; }
        = new PageHeaderViewModel(
            Resources.CommissionByBettype,
            IsFilterVisible: message.IsFilterVisible,
            BreadcrumbLevel: new BreadcrumbLevelViewModel(breadcrumbs));

    public DateRangePickerViewModel DateRangePicker { get; }
        = new DateRangePickerViewModel(message.MinDate, DateTime.MaxValue, message.FromDate, message.ToDate, message.UserSelectedDateRangeOption);

    public string ViewDataInstructionMessage
        => string.Format(Resources.CanOnlyViewDataAfterTimeHistoryReport, message.MinDate.ToShortDateWithSlash());

    public bool IsDescendingSort { get; } = message.IsDescendingSort;

    public string SortPropertyName { get; } = message.SortPropertyName;

    public IEnumerable<CommissionByBetTypeModel> ReportData { get; } = reportData;

    public CommissionByBetTypeModel Total { get; } = total;

    protected override IEnumerable<KeyValuePair<string, string>> GetBetListLinkQueryParams(CommissionByBetTypeModel model)
        => throw new NotImplementedException();

    protected override IEnumerable<KeyValuePair<string, string>> GetDownlineLinkQueryParams(CommissionByBetTypeModel model)
    {
        throw new NotImplementedException();
    }
}