using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.Statement;
using AlphaPlus.Reports.Site.Features.Shared;
using ONEbook.SpreadSheets;
using static ONEbook.SpreadSheets.Column;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.Statement;

public interface IStatementDetailWorkbook
{
    ExcelFileViewModel BuildExcelFile(StatementDetailExcelQuery message, ReportData<StatementDetailModel> reportData);
}

public class StatementDetailWorkbook(IWebHostEnvironment webHostEnvironment) :
    ReportNpoiWorkbook<StatementDetailModel>(webHostEnvironment),
    IStatementDetailWorkbook
{
    private Mapper<StatementDetailModel> BodyRowMapper
        => new Mapper<StatementDetailModel>()
            .Map(A, statement => statement.TransDate.ToString("dddd\r\nMM/dd/yyyy\r\nhh:mm:ss tt"))
            .Map(B, statement => $"{statement.TransType}\r\n{statement.TransDesc}")
            .Map(C, statement => (object)statement.Amount ?? Resources.Symbol_NoDataNumber);

    private Mapper<StatementDetailModel> FooterRowMapper
        => new Mapper<StatementDetailModel>()
            .Map(A, statement => Resources.Total)
            .Map(C, statement => (object)statement.Amount ?? Resources.Symbol_NoDataNumber);

    public ExcelFileViewModel BuildExcelFile(StatementDetailExcelQuery message, ReportData<StatementDetailModel> reportData)
    {
        var transferDate = message.TransferDate.ToShortDateWithSlash();

        var fileContent = UseTemplate(
            new TemplateInfo(
                GetExcelTemplatePath("StatementDetails"),
                noInfomationText: Resources.NoInformationIsAvailable,
                bodyStartIndex: 3,
                bodyEndIndex: 4,
                footerEndIndex: 5))
            .Title($"{Resources.TransferDetails} ({message.CustName}): {transferDate})")
            .Header(Resources.ResourceManager.GetString)
            .Body(reportData.DataSource, BodyRowMapper)
            .Footer(reportData.Total, FooterRowMapper)
            .Sheet(Resources.DefaultSheetName)
            .ExportToBytes();

        return new ExcelFileViewModel($"{Resources.TransferDetails}-{message.CustName}-{transferDate})", fileContent);
    }
}
