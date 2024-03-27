using AlphaPlus.Reports.Features.CommissionByBetType;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using BetListNPOI.HSSF.UserModel;
using BetListNPOI.SS.UserModel;
using ONEbook.SpreadSheets;
using static ONEbook.SpreadSheets.Column;

namespace AlphaPlus.Reports.Site.Features.CommissionByBetType;

public interface ICommissionByBetTypeWorkbook
{
    ExcelFileViewModel BuildExcelFile(CommissionByBetTypeExcelQuery message, ReportData<CommissionByBetTypeModel> reportData);
}

public class CommissionByBetTypeWorkbook(IWebHostEnvironment webHostEnvironment) :
    ReportNpoiWorkbook<CommissionByBetTypeModel>(webHostEnvironment),
    ICommissionByBetTypeWorkbook
{
    private bool isShowMyanmarOdds;

    public ExcelFileViewModel BuildExcelFile(CommissionByBetTypeExcelQuery message, ReportData<CommissionByBetTypeModel> reportData)
    {
        isShowMyanmarOdds = message.IsShowMyanmarOdds;
        var fromDate = message.FromDate.ToShortDateWithSlash();
        var toDate = message.ToDate.ToShortDateWithSlash();

        var fileContent = UseTemplate(
            new TemplateInfo(
                GetExcelTemplatePath(message.CustLevel.GetDirectDownlineLevel().DisplayName),
                noInfomationText: Resources.NoInformationIsAvailable,
                bodyStartIndex: 3,
                bodyEndIndex: 4))
            .Title($"{Resources.CommissionByBettype} {fromDate} --> {toDate}")
            .Header(Resources.ResourceManager.GetString)
            .Body(reportData.DataSource, GetRowMapper())
            .Footer(reportData.Total, GetRowMapper(true))
            .Sheet(Resources.DefaultSheetName)
            .ExportToBytes();

        return new ExcelFileViewModel($"{Resources.CommissionByBettype}-{message.CustName}-{fromDate}-{toDate}", fileContent);
    }

    private static Mapper<CommissionByBetTypeModel> GetRowMapper(bool isTotal = false)
        => new Mapper<CommissionByBetTypeModel>()
            .Map(A, commission => isTotal ? Resources.Total : commission.CustName)
            .Map(B, commission => commission.Comm1)
            .Map(C, commission => commission.Comm2)
            .Map(D, commission => commission.Comm3)
            .Map(E, commission => commission.MyanmarOddsComm)
            .Map(F, commission => commission.CasinoComm);

    protected override void FillBody(Sheet sheet)
    {
        base.FillBody(sheet);

        RemoveMyanmarOddsColumn();
    }

    protected override void FillNoInfoRow(Sheet sheet)
    {
        var rowIndex = TemplateInfo.BodyStartIndex;
        var colIndex = GetColIndex(TemplateInfo.ColumnStartName);

        var lastColumnIndex = sheet.OddRowTemplate.LastCellIndex - 1 - (!isShowMyanmarOdds ? 1 : 0);

        sheet.SetCellRangeValue(
            rowIndex,
            colIndex,
            rowIndex,
            lastColumnIndex,
            TemplateInfo.NoInfomationText);

        sheet.GetCell(rowIndex, colIndex).SetCenterHorizontalAlignment();
    }

    private void RemoveMyanmarOddsColumn()
    {
        var npoiSheet = Workbook.GetSheetAt(0);
        var headerAddress = npoiSheet.GetMergedRegion(0);

        npoiSheet.RemoveMergedRegion(0);

        if (!isShowMyanmarOdds)
        {
            RemoveColumnIndex(npoiSheet, 4);
            headerAddress.LastColumn--;
        }

        npoiSheet.AddMergedRegion(headerAddress);
    }

    private static void RemoveColumnIndex(ISheet sheet, int colIndex)
    {
        var rowIter = sheet.GetEnumerator();

        while (rowIter.MoveNext())
        {
            var row = (HSSFRow)rowIter.Current;
            var cell = row.GetCell(colIndex);

            if (cell != null)
            {
                row.RemoveCell(cell);

                for (int i = cell.ColumnIndex + 1, max = row.Cells.Count; i <= max; i++)
                {
                    row.CopyCell(i, i - 1);
                    row.RemoveCell(row.GetCell(i));
                }
            }
        }
    }
}