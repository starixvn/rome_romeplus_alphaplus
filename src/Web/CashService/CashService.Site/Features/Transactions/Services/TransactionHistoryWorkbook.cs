namespace AlphaPlus.CashService.Site.Features.Transactions.Services;

using AlphaPlus.Shared.AppResources.Report;
using Core.Models;
using Models;
using ONEbook.SpreadSheets;
using static ONEbook.SpreadSheets.Column;

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
                    noInfomationText: Resources.NoInformationIsAvailable,
                    bodyStartIndex: 3,
                    bodyEndIndex: 4))
            .Title($"Transaction History {param.FromDate} --> {param.ToDate}")
            .Header(Resources.ResourceManager.GetString)
            .Body(reportData, new Mapper<Transaction>()
                .Map(B, item => item.Username + WriteNickname(item.Nickname))
                .Map(C, item => item.FirstName + item.LastName)
                .Map(D, item => item.Amount)
                .Map(E, item => item.PaymentFrom)
                .Map(F, item => item.PaymentTo)
                .Map(G, item => item.StatusId)
                .Map(H, item => item.TransactionType)
                .Map(I, item => item.TransId)
                .Map(J, item => item.CreatedTime)
            )
            .Sheet("Transaction_History")
            .ExportToBytes();

        return new ExcelFileViewModel($"Transaction History-{param.FromDate}-{param.ToDate}", fileContent);
    }

    private static string WriteNickname(string nickName)
    => string.IsNullOrWhiteSpace(nickName) ? string.Empty : " (" + nickName + ")";

    protected override void FillBodyRows(Sheet sheet)
    {
        var currentRowIndex = TemplateInfo.BodyStartIndex;

        foreach (var data in SpreadSheet.BodyRows)
        {
            var row = sheet.GetRow(currentRowIndex);
            row.CopyCellsFrom(sheet.OddRowTemplate);
            FillCells(row, data, SpreadSheet.RowMapper.Mappings);
        }
    }
}