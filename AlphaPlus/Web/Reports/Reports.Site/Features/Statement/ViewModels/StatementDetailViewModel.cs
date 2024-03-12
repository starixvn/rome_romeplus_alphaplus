using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Statement;

namespace AlphaPlus.Reports.Site.Features.Statement;

public class StatementDetailViewModel(
    StatementDetailsQuery message,
    IEnumerable<StatementDetailModel> statements,
    StatementDetailModel total)
{
    public string CustName { get; } = message.CustName;

    public DateTime TransferDate { get; } = message.TransferDate;

    public IEnumerable<StatementDetailModel> ReportData { get; } = statements;

    public StatementDetailModel Total { get; } = total;

    public IDictionary<string, string> GetExcelParams()
        => new Dictionary<string, string>
        {
            [nameof(message.CustId)] = message.CustId.ToString(),
            [nameof(CustName)] = CustName,
            [nameof(TransferDate)] = TransferDate.ToShortDateWithSlash()
        };
}