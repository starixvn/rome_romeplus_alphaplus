using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Statement;
using AlphaPlus.Reports.Site.Features.Shared;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using BetListNPOI.HSSF.UserModel;
using BetListNPOI.SS.UserModel;
using ONEbook.SpreadSheets;
using ONEbook.SpreadSheets.BetListNPOI;

namespace AlphaPlus.Reports.Site.Features.Statement;

public interface ISMAStatementWorkbook
{
    ExcelFileViewModel BuildExcelFile(
        UserLevel custLevel,
        string custName,
        DateTime fromDate,
        DateTime toDate,
        IEnumerable<StatementModel> reportData);
}

public class SMAStatementWorkbook(IWebHostEnvironment webHostEnvironment) :
    ReportNpoiWorkbook<StatementModel>(webHostEnvironment),
    ISMAStatementWorkbook
{
    public ExcelFileViewModel BuildExcelFile(
        UserLevel custLevel,
        string custName,
        DateTime fromDate,
        DateTime toDate,
        IEnumerable<StatementModel> reportData)
    {
        var fromDateString = fromDate.ToShortDateWithSlash();
        var toDateString = toDate.ToShortDateWithSlash();

        var fileContent = UseTemplate(new TemplateInfo(
                GetExcelTemplatePath(custLevel.IsMemberLevel() ? "Member" : "SMA"),
                noInfomationText: Resources.NoInformationIsAvailable,
                bodyStartIndex: 3))
            .Title($"{Resources.Statement} ({custName}) {fromDateString} --> {toDateString}")
            .Header(key => key.Equals("Void/Reject", StringComparison.OrdinalIgnoreCase)
                ? $"{Resources.Void} / {Resources.Rejected}"
                : Resources.ResourceManager.GetString(key))
            .Body(reportData, null)
            .Sheet(Resources.DefaultSheetName)
            .ExportToBytes();

        return new ExcelFileViewModel($"{Resources.Statement}-{custName}-{fromDateString}-{toDateString}", fileContent);
    }

    protected override Sheet LoadTemplateSheet()
    {
        var sheet = GetSheetAt(TemplateInfo.SheetIndex);
        var currentRowIndex = TemplateInfo.BodyStartIndex;

        sheet.OddRowTemplate = sheet.GetRow(currentRowIndex++);
        sheet.EvenRowTemplate = sheet.GetRow(currentRowIndex++);
        sheet.FooterRowsTemplate = [];

        return sheet;
    }

    protected override void FillBodyRows(Sheet sheet)
    {
        var notedBalanceCellIndex = GetNotedBalanceCellIndex();
        var currentRowIndex = TemplateInfo.BodyStartIndex;
        var isEven = false;
        StatementModel previousStatement = null;

        var npoiSheet = ((NpoiSheet)sheet).Sheet;

        foreach (var data in SpreadSheet.BodyRows)
        {
            var templateRow = isEven ? sheet.EvenRowTemplate : sheet.OddRowTemplate;
            var row = sheet.GetRow(currentRowIndex);
            row.CopyCellsFrom(templateRow);

            var currentColIndex = row.FirstCellIndex;

            if (previousStatement != null
                && data.OpeningBalance != previousStatement.ClosingBalance)
            {
                var hssfRow = (HSSFRow)npoiSheet.GetRow(currentRowIndex);
                hssfRow.CopyCell(notedBalanceCellIndex, currentColIndex + 1);
            }

            row.SetCellValue(currentColIndex++, data.WinLossDate.ToShortDateWithSlash());
            row.SetCellValue(currentColIndex++, data.OpeningBalance);

            ExportWinLoss(row, data, currentColIndex);

            previousStatement = data;
            currentRowIndex++;
            isEven = !isEven;
        }

        RemoveColumnContent(Workbook.GetSheetAt(TemplateInfo.SheetIndex), notedBalanceCellIndex);
    }

    protected virtual int GetNotedBalanceCellIndex() => 7;

    protected virtual void ExportWinLoss(Row row, StatementModel data, int currentColIndex)
    {
        var colIndex = currentColIndex;
        row.SetCellValue(colIndex++, GetCellValue(data.Turnover));
        row.SetCellValue(colIndex++, GetCellValue(data.WinLoss));
        row.SetCellValue(colIndex++, GetCellValue(data.Commission));
        row.SetCellValue(colIndex++, GetCellValue(data.TransferAmount));
        row.SetCellValue(colIndex, GetCellValue(data.ClosingBalance));
    }

    protected static object GetCellValue(decimal? value)
        => (object)value ?? Resources.Symbol_NoDataNumber;

    private static void RemoveColumnContent(ISheet sheet, int colIndex)
    {
        var rowIter = sheet.GetEnumerator();

        while (rowIter.MoveNext())
        {
            var row = (HSSFRow)rowIter.Current;
            var cell = row.GetCell(colIndex);

            if (cell == null)
            {
                continue;
            }

            row.RemoveCell(cell);
        }
    }

    protected override void FillNoInfoRow(Sheet sheet)
    {
        var rowIndex = TemplateInfo.BodyStartIndex;
        var colIndex = Column.GetColIndex(TemplateInfo.ColumnStartName);

        sheet.SetCellRangeValue(
            rowIndex,
            colIndex,
            rowIndex,
            sheet.OddRowTemplate.LastCellIndex - 2,
            TemplateInfo.NoInfomationText);

        sheet.GetCell(rowIndex, colIndex).SetCenterHorizontalAlignment();
    }
}
