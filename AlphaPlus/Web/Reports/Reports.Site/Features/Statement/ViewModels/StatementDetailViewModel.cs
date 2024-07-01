using AlphaPlus.Core.Extensions;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.Statement;

namespace AlphaPlus.Reports.Site.Features.Statement;

public class StatementDetailViewModel(StatementDetailsQuery message, ReportData<StatementDetailModel> reportData)
{
    public string CustName { get; } = message.CustName;

    public DateTime TransferDate { get; } = message.TransferDate;

    public ReportData<StatementDetailModel> ReportData { get; } = reportData;

    public IDictionary<string, string> GetExcelParams()
        => new Dictionary<string, string>
        {
            [nameof(message.CustId)] = message.CustId.ToString(),
            [nameof(CustName)] = CustName,
            [nameof(TransferDate)] = TransferDate.ToShortDateWithSlash()
        };
}