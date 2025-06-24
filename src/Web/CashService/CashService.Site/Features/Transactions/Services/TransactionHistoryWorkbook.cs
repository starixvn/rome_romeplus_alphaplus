namespace AlphaPlus.CashService.Site.Features.Transactions.Services;

using System;
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
    private readonly Mapper<Transaction> rowMapper
        = new Mapper<Transaction>()
        .Map(A, item => item.Username + WriteNickname(item.Nickname))
        .Map(B, item => $"{item.FirstName} {item.LastName}")
        .Map(C, item => item.Amount)
        .Map(D, item => GetAccountNumberPart(item.PaymentFrom))
        .Map(E, item => GetAccountNumberPart(item.PaymentTo))
        .Map(F, item => MappingStatusId(item.StatusId))
        .Map(G, item => item.TransactionType)
        .Map(H, item => item.TransId)
        .Map(I, item => item.CreatedTime.ToLongDateTime24HWithSlash());

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
            .Body(reportData, rowMapper)
            .Sheet(Resources.DefaultSheetName)
            .ExportToBytes();

        return new ExcelFileViewModel($"TransactionHistory-{param.FromDate.Value.ToShortDateWithSlash()}-{param.ToDate.Value.ToShortDateWithSlash()}", fileContent);
    }

    private static string GetTitle(ExportExcelParams param)
        => $"Transaction History {param.FromDate.Value.ToShortDateWithSlash()} --> {param.ToDate.Value.ToShortDateWithSlash()}";

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

    private static string MappingStatusId(int statusId)
    => statusId switch
    {
        0 => "Waiting",
        1 => "Cancelled",
        2 => "Pending",
        3 => "Expired",
        4 => "Completed",
        5 => "Rejected",
        6 => "Provider Rejected",
        _ => ""
    };

    protected override void FillHeader(Sheet sheet)
    {
        var row = sheet.GetRow(TemplateInfo.HeaderStartIndex);
        int colIndex = row.FirstCellIndex;

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