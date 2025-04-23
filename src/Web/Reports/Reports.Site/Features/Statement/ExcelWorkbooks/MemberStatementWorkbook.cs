using AlphaPlus.Reports.Features.Statement;
using ONEbook.SpreadSheets;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.Statement;

public interface IMemberStatementWorkbook : ISMAStatementWorkbook
{
}

public class MemberStatementWorkbook(IWebHostEnvironment webHostEnvironment) :
    SMAStatementWorkbook(webHostEnvironment),
    IMemberStatementWorkbook
{
    protected override void FillHeader(Sheet sheet)
    {
        var row = sheet.GetRow(TemplateInfo.HeaderStartIndex);
        int colIndex = row.FirstCellIndex;

        row.SetCellValue(colIndex++, Resources.Date);
        row.SetCellValue(colIndex++, Resources.OpeningBalance);
        row.SetCellValue(colIndex++, Resources.Turnover);
        row.SetCellValue(colIndex++, Resources.Void + "/" + Resources.Rejected);
        row.SetCellValue(colIndex++, Resources.Outstanding);
        row.SetCellValue(colIndex++, Resources.WinLoss);
        row.SetCellValue(colIndex++, Resources.Commission);
        row.SetCellValue(colIndex++, Resources.GrandPrize);
        row.SetCellValue(colIndex++, Resources.TransferAmt);
        row.SetCellValue(colIndex, Resources.AccumulatedTotal);
    }

    protected override int GetNotedBalanceCellIndex() => 10;

    protected override void ExportWinLoss(Row row, StatementModel data, int currentColIndex)
    {
        var colIndex = currentColIndex;
        row.SetCellValue(colIndex++, GetCellValue(data.Turnover));
        row.SetCellValue(colIndex++, GetCellValue(data.VoidAmount));
        row.SetCellValue(colIndex++, GetCellValue(data.Outstanding));
        row.SetCellValue(colIndex++, GetCellValue(data.WinLoss));
        row.SetCellValue(colIndex++, GetCellValue(data.Commission));
        row.SetCellValue(colIndex++, GetCellValue(data.GrandPrize));
        row.SetCellValue(colIndex++, GetCellValue(data.TransferAmount));
        row.SetCellValue(colIndex, GetCellValue(data.ClosingBalance));
    }
}