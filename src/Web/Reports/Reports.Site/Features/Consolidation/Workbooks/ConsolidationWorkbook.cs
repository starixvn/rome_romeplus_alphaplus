using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.Consolidation;
using AlphaPlus.Reports.Site.Features.Shared;
using BetListNPOI.SS.Util;
using ONEbook.SpreadSheets;
using ONEbook.SpreadSheets.BetListNPOI;
using static ONEbook.SpreadSheets.Column;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.Consolidation;

public interface IConsolidationWorkbook
{
    ExcelFileViewModel BuildExcelFile(
        ConsolidationExcelQuery message,
        IEnumerable<ConsolidationByGroup> reportData,
        string productNames,
        IEnumerable<ConsolidationCurrency> currencies);
}

public class ConsolidationWorkbook(IWebHostEnvironment webHostEnvironment) :
    ReportNpoiWorkbook<ConsolidationByGroup>(webHostEnvironment),
    IConsolidationWorkbook
{
    private bool isReportByDate;


    public ExcelFileViewModel BuildExcelFile(
        ConsolidationExcelQuery message,
        IEnumerable<ConsolidationByGroup> reportData,
        string productNames,
        IEnumerable<ConsolidationCurrency> currencies)
    {
        var fromDate = message.FromDate.ToShortDateWithSlash();
        var toDate = message.ToDate.ToShortDateWithSlash();
        var byDate = isReportByDate = message.ReportType == ConsolidationReportType.ByDate;
        var templateName = byDate ? "ConsolidationByDate" : "ConsolidationByProduct";
        var reportTypeName = byDate ? Resources.ConsolidationReportByDate : Resources.ConsolidationReportByProduct;
        var currencyName = message.UserSelectedCurrencyId == 0
            ? Resources.AllCurrencies
            : currencies.FirstOrDefault(c => c.Currency == message.UserSelectedCurrencyId)?.CurrencyName ?? string.Empty;

        var fileContent = UseTemplate(
            new TemplateInfo(
                GetExcelTemplatePath(templateName),
                noInfomationText: Resources.NoInformationIsAvailable,
                freezeRowIndex: 4,
                freezeColumnName: C))
            .Title($"{reportTypeName} {fromDate} --> {toDate} {Resources.of2} {productNames} - {currencyName}")
            .Header(Resources.ResourceManager.GetString)
            .Body(reportData, null)
            .Sheet(Resources.DefaultSheetName)
            .ExportToBytes();

        return new ExcelFileViewModel($"{Resources.Consolidation}-{fromDate}-{toDate}", fileContent);
    }

    protected override void FillBodyRows(Sheet sheet)
    {
        var currentRowIndex = TemplateInfo.BodyStartIndex;

        foreach (var group in SpreadSheet.BodyRows)
        {
            foreach (var item in group.WinLoss)
            {
                if (item.BetCount == 0 && !isReportByDate)
                {
                    continue;
                }
                var row = sheet.GetRow(currentRowIndex++);
                row.CopyCellsFrom(sheet.OddRowTemplate);

                row.SetCellValue(0, group.BreakDown);
                SetWinLossByUsernameValue(item, row, isReportByDate);
            }
        }

        MergeCells(sheet, (group) => group.WinLoss.Count(x => x.BetCount > 0));
    }

    private void SetWinLossByUsernameValue(ConsolidationEntity data, Row row, bool isReportByDate)
    {
        var custLevelNameIndex = 1;
        row.SetCellValue(custLevelNameIndex++, data.CustLevelName);
        row.SetCellValue(custLevelNameIndex++, data.Username);
        row.SetCellValue(custLevelNameIndex++, data.CurrencyName);
        row.SetCellValue(custLevelNameIndex++, data.Turnover);
        row.SetCellValue(custLevelNameIndex++, data.GrossComm);
        row.SetCellValue(custLevelNameIndex++, data.MemberWinLoss);
        row.SetCellValue(custLevelNameIndex++, data.MemberComm);
        row.SetCellValue(custLevelNameIndex++, data.MemberTotal);
        row.SetCellValue(custLevelNameIndex++, data.OwnerTurnover);
        row.SetCellValue(custLevelNameIndex++, data.OwnerWinLoss);
        row.SetCellValue(custLevelNameIndex++, data.OwnerComm);
        row.SetCellValue(custLevelNameIndex++, data.OwnerTotal);
        row.SetCellValue(custLevelNameIndex++, data.Company);

        if (isReportByDate)
        {
            row.SetCellValue(custLevelNameIndex++, data.BetCount);
            row.SetCellValue(custLevelNameIndex++, data.RewardAmount);
            row.SetCellValue(custLevelNameIndex, data.RewardCurrency);
        }
        else
        {
            row.SetCellValue(custLevelNameIndex, data.BetCount);
        }
    }

    private void MergeCells(Sheet sheet, Func<ConsolidationByGroup, int> getGroupLength)
    {
        var currentRowIndex = TemplateInfo.BodyStartIndex;

        foreach (var group in SpreadSheet.BodyRows)
        {
            var row = sheet.GetRow(currentRowIndex);
            var itemCount = getGroupLength(group);
            var endRowIndex = currentRowIndex + itemCount - 1;

            if (currentRowIndex < endRowIndex)
            {
                var newRange = new CellRangeAddress(currentRowIndex, endRowIndex, row.FirstCellIndex, row.FirstCellIndex);
                ((NpoiSheet)sheet).Sheet.AddMergedRegion(newRange);
            }

            currentRowIndex += itemCount;
        }
    }
}