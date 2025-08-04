namespace AlphaPlus.CashService.Site.Features.Transactions.Services;

using System;
using AlphaPlus.CashService.Site.Features.Shared;
using AlphaPlus.Shared.AppResources.CashServices;
using AlphaPlus.Shared.AppResources.Common;
using AlphaPlus.Shared.AppResources.Report;
using Core.Extensions;
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
    private int number = 1;

    private Mapper<Transaction> RowMapper()
        => new Mapper<Transaction>()
        .Map(A, _ => number++)
        .Map(B, item => item.Username + WriteNickname(item.Nickname))
        .Map(C, item => $"{item.FirstName} {item.LastName}")
        .Map(D, item => item.Amount)
        .Map(E, item => GetAccountNumberPart(item.PaymentFrom))
        .Map(F, item => GetAccountNumberPart(item.PaymentTo))
        .Map(G, item => item.Status)
        .Map(H, item => item.TransactionType)
        .Map(I, item => item.TransId)
        .Map(J, item => item.CreatedTime.ToLongDateTimeWithSlash());

    public ExcelFileViewModel BuildExcelFile(ExportExcelParams param, IEnumerable<Transaction> reportData)
    {
        var fileContent = UseTemplate(
                new TemplateInfo(
                    GetExcelTemplatePath("TransactionHistory"),
                    noInfomationText: Resources.NoInformationIsAvailable,
                    bodyStartIndex: 3,
                    bodyEndIndex: 4))
            .Title(GetTitle(param))
            .Header(Resources.ResourceManager.GetString)
            .Body(reportData, RowMapper())
            .Sheet(Resources.DefaultSheetName)
            .ExportToBytes();

        return new ExcelFileViewModel($"{CashServicesResx.TransactionHistory}-{param.Username}-{param.FromDate.Value.ToShortDateWithSlash()}-{param.ToDate.Value.ToShortDateWithSlash()}", fileContent);
    }

    private static string GetTitle(ExportExcelParams param)
        => $"{CashServicesResx.TransactionHistory} {param.FromDate.Value.ToShortDateWithSlash()} --> {param.ToDate.Value.ToShortDateWithSlash()}";

    private static string GetAccountNumberPart(string bankAccount)
    {
        if (string.IsNullOrWhiteSpace(bankAccount))
        {
            return string.Empty;
        }

        var parts = bankAccount.Split('-', StringSplitOptions.RemoveEmptyEntries);

        return parts switch
        {
            [var accountNumber] => accountNumber,

            [_, var accountNumber] => accountNumber,

            _ => string.Empty
        };
    }

    protected override void FillHeader(Sheet sheet)
    {
        var row = sheet.GetRow(TemplateInfo.HeaderStartIndex);
        int colIndex = row.FirstCellIndex;

        row.SetCellValue(colIndex++, AlphaPlus.Shared.AppResources.Betlist.Services.Resources.number);
        row.SetCellValue(colIndex++, $"{Resources.Username}\n{CommonResx.Nickname}");
        row.SetCellValue(colIndex++, CashServicesResx.FirstAndLastName);
        row.SetCellValue(colIndex++, Resources.Amount);
        row.SetCellValue(colIndex++, Resources.From);
        row.SetCellValue(colIndex++, Resources.To);
        row.SetCellValue(colIndex++, Resources.Status);
        row.SetCellValue(colIndex++, CashServicesResx.TransactionType);
        row.SetCellValue(colIndex++, Resources.refno);
        row.SetCellValue(colIndex, Resources.CreatedDate);
    }

    private static string WriteNickname(string nickName)
    => string.IsNullOrWhiteSpace(nickName) ? string.Empty : " (" + nickName + ")";
}