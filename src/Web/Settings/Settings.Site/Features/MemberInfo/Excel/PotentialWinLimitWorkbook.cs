using AlphaPlus.Settings.MemberInfo.Domain.Models.WinLossLimit;
using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.WinLossLimit;
using ONEbook.SpreadSheets;
using ONEbook.SpreadSheets.NPOI;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Excel;

public interface IPotentialWinLimitWorkbook
{
    Workbook<PotentialWinLimitDetail> UseTemplate(TemplateInfo templateInfo);
    IPotentialWinLimitWorkbook SetData(SportsbookPotentialWinLimitViewModel data);
    IPotentialWinLimitWorkbook SetRowMappers(Mapper<PotentialWinLimitDetail> rowMapper);
}

public class PotentialWinLimitWorkbook : 
    NpoiWorkbook<PotentialWinLimitDetail>,
    IPotentialWinLimitWorkbook
{
    private SportsbookPotentialWinLimitViewModel Data { get; set; }
    private Mapper<PotentialWinLimitDetail> RowMapper { get; set; }
    private Row DataRowTemplate { get; set; }

    public IPotentialWinLimitWorkbook SetData(SportsbookPotentialWinLimitViewModel data)
    {
        Data = data;
        return this;
    }

    public IPotentialWinLimitWorkbook SetRowMappers(Mapper<PotentialWinLimitDetail> rowMapper)
    {
        RowMapper = rowMapper;
        return this;
    }

    protected override Sheet LoadTemplateSheet()
    {
        var sheet = GetSheetAt(TemplateInfo.SheetIndex);
        var currentRowIndex = TemplateInfo.BodyStartIndex;
        DataRowTemplate = sheet.GetRow(currentRowIndex);
        return sheet;
    }

    protected override void FillNoInfoRow(Sheet sheet)
    {
        if (sheet == null)
        {
            throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
        }

        var rowIndex = TemplateInfo.BodyStartIndex;
        var colIndex = Column.GetColIndex(TemplateInfo.ColumnStartName);

        sheet.SetCellRangeValue(
            rowIndex,
            colIndex,
            rowIndex,
            DataRowTemplate.LastCellIndex - 1,
            TemplateInfo.NoInfomationText);

        sheet.GetCell(rowIndex, colIndex).SetCenterHorizontalAlignment();
    }

    protected override void FillBody(Sheet sheet)
    {
        if (sheet == null)
        {
            throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
        }

        RemoveTemplateRows(sheet);

        if (Data.PotentialWinLimitPage?.PotentialWinLimitDetail != null && Data.PotentialWinLimitPage.PotentialWinLimitDetail.Any())
        {
            FillBodyRows(sheet);
        }
        else
        {
            FillNoInfoRow(sheet);
        }
    }

    protected override void FillBodyRows(Sheet sheet)
    {
        if (sheet == null)
        {
            throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
        }

        var currentRowIndex = TemplateInfo.BodyStartIndex;
        currentRowIndex++;

        foreach (var item in Data.PotentialWinLimitPage.PotentialWinLimitDetail)
        {
            var row = sheet.GetRow(currentRowIndex++);
            row.CopyCellsFrom(DataRowTemplate);
            FillCells(row, item, RowMapper.Mappings);
        }
    }

    protected override void RemoveTemplateRows(Sheet sheet)
    {
        // No need to remove template rows as we're using the same sheet
    }
} 