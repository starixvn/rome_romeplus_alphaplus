using AlphaPlus.Reports.Features.BetCreditTracking.MoveStatementHistory;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Site.Features.Shared;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using ONEbook.SpreadSheets;
using static ONEbook.SpreadSheets.Column;

namespace AlphaPlus.Reports.Site.Features.BetCreditTracking;

public interface IMoveStatementHistoryWorkbook
{
    ExcelFileViewModel BuildExcelFile(MoveStatementHistoryExcelQuery message, IEnumerable<MoveStatementHistoryByMember> reportData);
}

public class MoveStatementHistoryWorkbook(IWebHostEnvironment webHostEnvironment)
    : ReportNpoiWorkbook<MoveStatementHistoryByRefNo>(webHostEnvironment), IMoveStatementHistoryWorkbook
{
    private IEnumerable<MoveStatementHistoryByMember> reportData;

    private Mapper<MoveStatementHistoryByRefNo> memberMapper
        = new Mapper<MoveStatementHistoryByRefNo>()
        .Map(A, moveStatementHistory => moveStatementHistory.Username)
        .Map(D, moveStatementHistory => moveStatementHistory.MemberWinLoss)
        .Map(E, moveStatementHistory => moveStatementHistory.MemberComm)
        .Map(F, moveStatementHistory => moveStatementHistory.MemberTotal);

    private Mapper<MoveStatementHistoryByRefNo> refNoMapper
        = new Mapper<MoveStatementHistoryByRefNo>()
        .Map(A, moveStatementHistory => moveStatementHistory.TransId.ToString())
        .Map(B, moveStatementHistory => moveStatementHistory.OldEventDate.ToShortDateWithSlash())
        .Map(C, moveStatementHistory => moveStatementHistory.NewEventDate.ToShortDateWithSlash())
        .Map(D, moveStatementHistory => moveStatementHistory.MemberWinLoss)
        .Map(E, moveStatementHistory => moveStatementHistory.MemberComm)
        .Map(F, moveStatementHistory => moveStatementHistory.MemberTotal);

    public ExcelFileViewModel BuildExcelFile(MoveStatementHistoryExcelQuery message, IEnumerable<MoveStatementHistoryByMember> reportData)
    {
        var fromDate = message.FromDate.ToShortDateWithSlash();
        var toDate = message.ToDate.ToShortDateWithSlash();
        this.reportData = reportData;

        var fileContent = UseTemplate(new TemplateInfo(
                GetExcelTemplatePath("MoveStatementHistory"),
                noInfomationText: Resources.NoInformationIsAvailable,
                bodyStartIndex: 3))
            .Title($"{Resources.MoveStatementHistory} {fromDate} --> {fromDate}")
            .Header(Resources.ResourceManager.GetString)
            .Sheet(Resources.DefaultSheetName)
            .ExportToBytes();

        return new ExcelFileViewModel($"{Resources.MoveStatementHistory}-{message.CustName}-{fromDate}-{toDate}", fileContent);
    }

    protected override Sheet LoadTemplateSheet()
    {
        var sheet = GetSheetAt(TemplateInfo.SheetIndex);
        var currentRowIndex = TemplateInfo.BodyStartIndex;

        var memberRowTemplate = sheet.GetRow(currentRowIndex++);
        sheet.OddRowTemplate = sheet.GetRow(currentRowIndex++);
        sheet.EvenRowTemplate = sheet.GetRow(currentRowIndex++);
        sheet.FooterRowsTemplate = [memberRowTemplate];

        return sheet;
    }

    protected override void FillBody(Sheet sheet)
    {
        if (sheet == null)
        {
            throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
        }

        RemoveTemplateRows(sheet);

        if (reportData.Any())
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
        var currentRowIndex = TemplateInfo.BodyStartIndex;

        foreach (var member in reportData)
        {
            FillMemberTitle(sheet, currentRowIndex++, member);

            foreach (var row in member.RefNoRow)
            {
                FillRefNo(sheet, currentRowIndex++, row);
            }
        }
    }

    private void FillMemberTitle(Sheet sheet, int rowIndex, MoveStatementHistoryByMember member)
    {
        var row = sheet.GetRow(rowIndex);
        row.CopyCellsFrom(sheet.FooterRowsTemplate.First());
        FillCells(row, member.MemberRow, memberMapper.Mappings);
    }

    private void FillRefNo(Sheet sheet, int rowIndex, MoveStatementHistoryByRefNo refno)
    {
        var row = sheet.GetRow(rowIndex);
        row.CopyCellsFrom(refno.IsMultipleMoved ? sheet.EvenRowTemplate : sheet.OddRowTemplate);
        FillCells(row, refno, refNoMapper.Mappings);
    }
}