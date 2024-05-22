using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.BetCreditTracking.BetCreditHistory;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Site.Features.Shared;
using ONEbook.SpreadSheets;
using static ONEbook.SpreadSheets.Column;

namespace AlphaPlus.Reports.Site.Features.BetCreditTracking;

public interface IBetCreditHistoryWorkbook
{
    ExcelFileViewModel BuildExcelFile(BetCreditHistoryExcelQuery message, IEnumerable<BetCreditHistoryEntity> reportData);
}

public class BetCreditHistoryWorkbook(IWebHostEnvironment webHostEnvironment)
    : ReportNpoiWorkbook<BetCreditHistoryEntity>(webHostEnvironment), IBetCreditHistoryWorkbook
{
    private Row highlightRowTemplate;
    private Row redHighlightRowTemplate;

    public ExcelFileViewModel BuildExcelFile(BetCreditHistoryExcelQuery message, IEnumerable<BetCreditHistoryEntity> reportData)
    {
        var fromDate = message.FromDate.ToShortDateWithSlash();
        var toDate = message.ToDate.ToShortDateWithSlash();

        var fileContent = UseTemplate(
            new TemplateInfo(
                GetExcelTemplatePath("BetCreditHistory"),
                noInfomationText: Resources.NoInformationIsAvailable,
                bodyStartIndex: 3))
            .Title($"{Resources.BetCreditHistory} {fromDate} --> {toDate} - {Resources.Member}: {message.Member}")
            .Header(Resources.ResourceManager.GetString)
            .Body(reportData, new Mapper<BetCreditHistoryEntity>()
                .Map(A, betCreditHistory => betCreditHistory.ActionDateString)
                .Map(B, betCreditHistory => betCreditHistory.WinLostDateString)
                .Map(C, betCreditHistory => betCreditHistory.RefNoString)
                .Map(D, betCreditHistory => GetNullableDecimal(betCreditHistory.ActualStake))
                .Map(E, betCreditHistory => GetNullableDecimal(betCreditHistory.WinLoss))
                .Map(F, betCreditHistory => GetNullableDecimal(betCreditHistory.PlayerComm))
                .Map(G, betCreditHistory => GetNullableDecimal(betCreditHistory.BalanceChanged))
                .Map(H, betCreditHistory => GetNullableDecimal(betCreditHistory.NewBalance))
                .Map(I, betCreditHistory => betCreditHistory.GivenCredit)
                .Map(J, betCreditHistory => GetNullableDecimal(betCreditHistory.BetCredit))
                .Map(K, betCreditHistory => betCreditHistory.Action))
            .Sheet(Resources.DefaultSheetName)
            .ExportToBytes();

        return new ExcelFileViewModel($"{Resources.BetCreditHistory}-{fromDate}-{toDate}", fileContent);
    }

    private static object GetNullableDecimal(decimal? value)
        => value.HasValue ? value.Value : string.Empty;

    protected override Sheet LoadTemplateSheet()
    {
        var sheet = GetSheetAt(TemplateInfo.SheetIndex);
        var currentRowIndex = TemplateInfo.BodyStartIndex;

        highlightRowTemplate = sheet.GetRow(currentRowIndex++);
        sheet.OddRowTemplate = sheet.GetRow(currentRowIndex++);
        redHighlightRowTemplate = sheet.GetRow(currentRowIndex++);

        return sheet;
    }

    protected override void RemoveTemplateRows(Sheet sheet)
    {
        sheet.RemoveRow(highlightRowTemplate);
        sheet.RemoveRow(sheet.OddRowTemplate);
        sheet.RemoveRow(redHighlightRowTemplate);
    }

    protected override void FillBodyRows(Sheet sheet)
    {
        var currentRowIndex = TemplateInfo.BodyStartIndex;

        foreach (var data in SpreadSheet.BodyRows)
        {
            if (data.BetCredit == null)
            {
                FillHighlightRow(sheet, currentRowIndex++, data);
            }
            else
            {
                if (data.BetCredit < 0M)
                {
                    FillRedHighlightRow(sheet, currentRowIndex++, data);
                    continue;
                }

                FillRow(sheet, currentRowIndex++, data);
            }
        }
    }

    private void FillHighlightRow(Sheet sheet, int rowIndex, BetCreditHistoryEntity data)
    {
        var row = sheet.GetRow(rowIndex);
        row.CopyCellsFrom(highlightRowTemplate);
        FillCells(row, data, SpreadSheet.RowMapper.Mappings);
    }

    private void FillRedHighlightRow(Sheet sheet, int rowIndex, BetCreditHistoryEntity data)
    {
        var row = sheet.GetRow(rowIndex);
        row.CopyCellsFrom(redHighlightRowTemplate);
        FillCells(row, data, SpreadSheet.RowMapper.Mappings);
    }

    private void FillRow(Sheet sheet, int rowIndex, BetCreditHistoryEntity data)
    {
        var row = sheet.GetRow(rowIndex);
        row.CopyCellsFrom(sheet.OddRowTemplate);
        FillCells(row, data, SpreadSheet.RowMapper.Mappings);
    }
}
