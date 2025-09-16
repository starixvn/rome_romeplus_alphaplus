using AlphaPlus.Core.Extensions;
using AlphaPlus.Reports.Features.Statement;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.Statement;

public class SMAStatementViewModel(SMAStatementQuery message, IEnumerable<StatementModel> statements, string title = null)
{
    public int CustId { get; } = message.CustId;

    public string CustName { get; } = message.CustName;

    public byte CustLevelId { get; } = message.CustLevelId;

    public PageHeaderViewModel PageHeader { get; }
        = new PageHeaderViewModel(Title: title ?? Resources.Statement, IsFilterVisible: message.IsFilterVisible);

    public DateRangePickerViewModel DateRangePicker { get; }
        = new DateRangePickerViewModel(
            message.MinDate,
            DateTime.MaxValue,
            message.FromDate,
            message.ToDate,
            message.UserSelectedDateRangeOption);

    public string ViewDataInstructionMessage { get; }
        = string.Format(Resources.CanOnlyViewDataAfterTimeHistoryReport, message.MinDate.ToShortDateWithSlash());

    public IEnumerable<StatementModel> ReportData { get; } = statements;

    public PagerViewModel Pager { get; }
        = new PagerViewModel(message.PageIndex, message.PageSize, statements.Count());

    public IDictionary<string, string> GetExcelParams()
        => new Dictionary<string, string>
        {
            [nameof(CustId)] = CustId.ToString(),
            [nameof(CustName)] = CustName,
            [nameof(CustLevelId)] = CustLevelId.ToString(),
            [nameof(DateRangePicker.FromDate)] = DateRangePicker.FromDate,
            [nameof(DateRangePicker.ToDate)] = DateRangePicker.ToDate
        };
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
