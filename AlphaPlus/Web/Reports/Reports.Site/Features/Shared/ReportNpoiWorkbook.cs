using ONEbook.SpreadSheets.BetListNPOI;

namespace AlphaPlus.Reports.Site.Features.Shared;

public class ReportNpoiWorkbook<T>(IWebHostEnvironment webHostEnvironment) : NpoiWorkbook<T> where T : class
{
    private const string ExcelTemplateDir = "App_Data/ExcelTemplates/{0}/{1}.{2}";

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
}
