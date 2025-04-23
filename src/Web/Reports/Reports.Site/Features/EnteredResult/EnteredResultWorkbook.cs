using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared;
using ONEbook.SpreadSheets;
using static ONEbook.SpreadSheets.Column;
using AlphaPlus.Shared.AppResources.Report;
using AlphaPlus.Core.Utils;

namespace AlphaPlus.Reports.Site.Features.EnteredResult;

public interface IEnteredResultWorkbook
{
    ExcelFileViewModel BuildExcelFile(EnteredResultExcelQuery message, ReportData<EnteredResultSport> reportData);
}

public class EnteredResultWorkbook(IWebHostEnvironment webHostEnvironment) :
    ReportNpoiWorkbook<EnteredResultSport>(webHostEnvironment),
    IEnteredResultWorkbook
{
    private readonly Mapper<EnteredResultSport> rowMapper = new Mapper<EnteredResultSport>()
        .Map(A, enteredResult => string.IsNullOrWhiteSpace(enteredResult.SportName)
            ? Resources.Total
            : enteredResult.SportName)
        .Map(B, enteredResult => enteredResult.EnteredResultYesterday?.Entered ?? 0)
        .Map(C, enteredResult => enteredResult.EnteredResultYesterday?.NonEntered ?? 0)
        .Map(D, enteredResult => enteredResult.EnteredResultToday?.Entered ?? 0)
        .Map(E, enteredResult => enteredResult.EnteredResultToday?.NonEntered ?? 0);

    public ExcelFileViewModel BuildExcelFile(EnteredResultExcelQuery message, ReportData<EnteredResultSport> reportData)
    {
        DateTime today, yesterday;
        if (reportData.HasData)
        {
            today = reportData.Total.EnteredResultToday.EventDate;
            yesterday = reportData.Total.EnteredResultYesterday.EventDate;
        }
        else
        {
            today = SystemTime.Today;
            yesterday = SystemTime.Yesterday;
        }
        var resources = new Dictionary<string, string>
        {
            ["Today"] = today.ToShortDateWithSlash(),
            ["Yesterday"] = yesterday.ToShortDateWithSlash()
        };

        var fileContent = UseTemplate(
            new TemplateInfo(
                GetExcelTemplatePath("MatchResultEntered"),
                noInfomationText: Resources.NoInformationIsAvailable,
                titleStartIndex: 0,
                headerStartIndex: 1,
                bodyStartIndex: 3,
                bodyEndIndex: 4))
            .Header(key => resources.TryGetValue(key, out var value) ? value : Resources.ResourceManager.GetString(key))
            .Body(reportData.DataSource, rowMapper)
            .Footer(reportData.Total, rowMapper)
            .Sheet(Resources.DefaultSheetName)
            .ExportToBytes();

        return new ExcelFileViewModel($"{Resources.MatchResultEntered}-{resources["Yesterday"]}-{resources["Today"]}", fileContent);
    }

    protected override void FillBodyRows(Sheet sheet)
    {
        if (sheet == null)
        {
            throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
        }

        var currentRowIndex = TemplateInfo.BodyStartIndex;
        FillTotalRow(sheet, currentRowIndex++);

        var isEvenRow = false;
        foreach (var data in SpreadSheet.BodyRows)
        {
            FillRow(sheet, currentRowIndex++, isEvenRow, data);
            isEvenRow = !isEvenRow;
        }
    }

    private void FillTotalRow(Sheet sheet, int rowIndex)
    {
        var row = sheet.GetRow(rowIndex);
        row.CopyCellsFrom(sheet.FooterRowsTemplate.First());
        FillCells(row, SpreadSheet.FooterRows.First(), rowMapper.Mappings);
    }

    private void FillRow(Sheet sheet, int rowIndex, bool isEvenRow, EnteredResultSport data)
    {
        var row = sheet.GetRow(rowIndex);
        var templateRow = isEvenRow ? sheet.EvenRowTemplate : sheet.OddRowTemplate;

        row.CopyCellsFrom(templateRow);
        FillCells(row, data, rowMapper.Mappings);
    }
}