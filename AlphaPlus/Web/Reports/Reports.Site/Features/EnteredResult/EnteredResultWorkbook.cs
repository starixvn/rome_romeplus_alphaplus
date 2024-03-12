// using AlphaPlus.Reports.Features.EnteredResult;
// using AlphaPlus.Reports.Site.Features.Shared;
// using ONEbook.SpreadSheets;
// using static ONEbook.SpreadSheets.Column;

// namespace AlphaPlus.Reports.Site.Features.EnteredResult;

// public interface IEnteredResultWorkbook
// {
//     IEnteredResultWorkbook SetRowMapper();

//     IEnteredResultWorkbook SetData(IEnumerable<EnteredResultSport> reportData, EnteredResultSport total);

//     Workbook<EnteredResultSport> UseTemplate(TemplateInfo templateInfo);

//     TemplateInfo GetTemplateInfo();

//     string GetFileName(IDictionary<string, string> resources);
// }

// public class EnteredResultWorkbook(IWebHostEnvironment webHostEnvironment) :
//     ReportNpoiWorkbook<EnteredResultSport>(webHostEnvironment),
//     IEnteredResultWorkbook
// {
//     private Mapper<EnteredResultSport> rowMapper;

//     public IEnteredResultWorkbook SetRowMapper()
//     {
//         rowMapper = new Mapper<EnteredResultSport>()
//             .Map(A, enteredResult => string.IsNullOrWhiteSpace(enteredResult.SportName)
//                 ? Resources.Total
//                 : enteredResult.SportName)
//             .Map(B, enteredResult => enteredResult.EnteredResultYesterday?.Entered ?? 0)
//             .Map(C, enteredResult => enteredResult.EnteredResultYesterday?.NonEntered ?? 0)
//             .Map(D, enteredResult => enteredResult.EnteredResultToday?.Entered ?? 0)
//             .Map(E, enteredResult => enteredResult.EnteredResultToday?.NonEntered ?? 0);

//         return this;
//     }

//     public IEnteredResultWorkbook SetData(IEnumerable<EnteredResultSport> reportData, EnteredResultSport total)
//     {
//         ReportData = reportData;
//         Total = total;

//         return this;
//     }

//     public TemplateInfo GetTemplateInfo()
//         => new TemplateInfo(
//             templatePath: GetExcelTemplatePath("MatchResultEntered"),
//             noInfomationText: Resources.NoInformationIsAvailable,
//             titleStartIndex: 0,
//             headerStartIndex: 1,
//             bodyStartIndex: 3,
//             bodyEndIndex: 5);

//     protected override void FillBodyRows(Sheet sheet)
//     {
//         if (sheet == null)
//         {
//             throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
//         }

//         CurrentRowIndex = TemplateInfo.BodyStartIndex;
//         FillTotalRow(sheet, CurrentRowIndex++);

//         var isEvenRow = false;
//         foreach (var data in ReportData)
//         {
//             FillRow(sheet, CurrentRowIndex++, isEvenRow, data);
//             isEvenRow = !isEvenRow;
//         }
//     }

//     private void FillRow(Sheet sheet, int rowIndex, bool isEvenRow, EnteredResultSport data)
//     {
//         var row = sheet.GetRow(rowIndex);
//         var templateRow = isEvenRow ? EvenRowTemplate : OddRowTemplate;

//         row.CopyCellsFrom(templateRow);
//         FillCells(row, data, rowMapper.Mappings);
//     }

//     private void FillTotalRow(Sheet sheet, int rowIndex)
//     {
//         var row = sheet.GetRow(rowIndex);
//         row.CopyCellsFrom(TotalRowTemplate);
//         FillCells(row, Total, rowMapper.Mappings);
//     }

//     public string GetFileName(IDictionary<string, string> resources)
//         => string.Join("-", Resources.MatchResultEntered, resources["Yesterday"], resources["Today"]);
// }