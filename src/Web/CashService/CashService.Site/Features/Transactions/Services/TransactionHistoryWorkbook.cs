namespace AlphaPlus.CashService.Site.Features.Transactions.Services;

using AlphaPlus.Shared.AppResources.Report;
using Core.Models;
using Models;
using ONEbook.SpreadSheets;

public interface ITransactionHistoryWorkbook
{
    ExcelFileViewModel BuildExcelFile(ExportExcelParams param, IEnumerable<Transaction> reportData);
}

public class TransactionHistoryWorkbook(IWebHostEnvironment webHostEnvironment) : ReportNpoiWorkbook<Transaction>(webHostEnvironment),
    ITransactionHistoryWorkbook
{
    public ExcelFileViewModel BuildExcelFile(ExportExcelParams param, IEnumerable<Transaction> reportData)
    {
        var fileContent = UseTemplate(
                new TemplateInfo(
                    GetExcelTemplatePath("TransactionHistory"),
                    noInfomationText: "No Data"))
            .Title($"Transaction History {param.FromDate} --> {param.ToDate}")
            .Header(Resources.ResourceManager.GetString)
            .Body(reportData, null)
            .Sheet("Transaction_History")
            .ExportToBytes();

        return new ExcelFileViewModel($"Transaction History-{param.FromDate}-{param.ToDate}", fileContent);
    }

    protected override void FillBodyRows(Sheet sheet)
    {
        var currentRowIndex = TemplateInfo.BodyStartIndex;

        foreach (var transaction in SpreadSheet.BodyRows)
        {
            var row = sheet.GetRow(currentRowIndex++);
            row.CopyCellsFrom(sheet.OddRowTemplate);
        }
    }
}