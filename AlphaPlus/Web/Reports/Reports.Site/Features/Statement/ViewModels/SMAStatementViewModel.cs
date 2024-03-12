using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Statement;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

namespace AlphaPlus.Reports.Site.Features.Statement;

public class SMAStatementViewModel
{
    public SMAStatementViewModel(SMAStatementQuery message, IEnumerable<StatementModel> statements, string title = null)
    {
        CustId = message.CustId;
        CustName = message.CustName;
        CustLevelId = message.CustLevelId;
        PageHeader = new PageHeaderViewModel(Title: title ?? Resources.Statement, IsFilterVisible: message.IsFilterVisible);
        DateRangePicker = new DateRangePickerViewModel(message.MinDate, DateTime.MaxValue, message.FromDate, message.ToDate, message.UserSelectedDateRangeOption);
        ViewDataInstructionMessage = string.Format(Resources.CanOnlyViewDataAfterTimeHistoryReport, message.MinDate.ToShortDateWithSlash());
        ReportData = statements;
        Pager = new PagerViewModel(message.PageIndex, message.PageSize, statements.Count());
    }

    public int CustId { get; }

    public string CustName { get; }

    public byte CustLevelId { get; }

    public PageHeaderViewModel PageHeader { get; }

    public DateRangePickerViewModel DateRangePicker { get; }

    public string ViewDataInstructionMessage { get; }

    public IEnumerable<StatementModel> ReportData { get; }

    public PagerViewModel Pager { get; }

    public IDictionary<string, string> GetExcelParams()
        => new Dictionary<string, string>
        {
            [nameof(CustId)] = CustId.ToString(),
            [nameof(CustName)] = CustName,
            [nameof(CustLevelId)] = CustLevelId.ToString(),
            [nameof(DateRangePicker.FromDate)] = DateRangePicker.FromDate,
            [nameof(DateRangePicker.ToDate)] = DateRangePicker.ToDate
        };
}
