using ONEbook.SpreadSheets;
using ONEbook.SpreadSheets.BetListNPOI;

namespace AlphaPlus.Reports.Site.Features.Shared;

public class ReportNpoiWorkbook<T>(IWebHostEnvironment webHostEnvironment) : NpoiWorkbook<T> where T : class
{
    private const int ReportSheetIndex = 1;
    private const string ExcelTemplateDir = "App_Data/ExcelTemplates/{0}/{1}.{2}";

    protected int CurrentRowIndex { get; set; }
    protected Row OddRowTemplate { get; set; }
    protected Row EvenRowTemplate { get; set; }
    protected Row TotalRowTemplate { get; set; }

    protected IEnumerable<T> ReportData { get; set; }
    protected T Total { get; set; }

    protected string GetExcelTemplatePath(string templateName, string fileExtension = "xls")
    {
        if (string.IsNullOrWhiteSpace(templateName))
        {
            throw new ArgumentNullException(nameof(templateName), "Template Name cannot be null or empty");
        }

        var relativePath = string.Format(ExcelTemplateDir, GetExcelTemplateFolder(), templateName, fileExtension);
        var absolutePath = Path.Combine(webHostEnvironment.ContentRootPath, relativePath);

        if (File.Exists(absolutePath))
        {
            return absolutePath;
        }

        throw new FileNotFoundException($"Template {absolutePath} does not exist");
    }

    protected string GetExcelTemplateFolder()
    {
        var reportNamespace = GetType().Namespace;
        var lastIndexOfDot = reportNamespace.LastIndexOf(".", StringComparison.Ordinal) + 1;

        if (lastIndexOfDot <= 0)
        {
            throw new ArgumentException("Namespace does not contain any dot character, wrong report convention.");
        }

        return reportNamespace.Substring(lastIndexOfDot);
    }

    protected override void RemoveTemplateRows(Sheet sheet)
        => Workbook.RemoveSheetAt(TemplateInfo.SheetIndex);

    protected override void FillSheetName(int index, string name)
        => Workbook.SetSheetName(ReportSheetIndex, name);

    protected override Sheet LoadTemplateSheet()
    {
        var sheet = GetSheetAt(TemplateInfo.SheetIndex);
        CurrentRowIndex = TemplateInfo.BodyStartIndex;

        OddRowTemplate = sheet.GetRow(CurrentRowIndex++);
        EvenRowTemplate = sheet.GetRow(CurrentRowIndex++);
        TotalRowTemplate = sheet.GetRow(CurrentRowIndex++);

        return GetSheetAt(ReportSheetIndex);
    }

    protected override void FillBody(Sheet sheet)
    {
        if (sheet == null)
        {
            throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
        }

        RemoveTemplateRows(sheet);

        if (ReportData.Any())
        {
            FillBodyRows(sheet);
        }
        else
        {
            FillNoInfoRow(sheet);
        }
    }
}