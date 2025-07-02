using ONEbook.SpreadSheets.BetListNPOI;

namespace AlphaPlus.CashService.Site.Features.Shared;

public class ReportNpoiWorkbook<T>(IWebHostEnvironment webHostEnvironment) : NpoiWorkbook<T> where T : class
{
    private const string ExcelTemplateDir = "App_Data/ExcelTemplates/{0}.{1}";

    protected string GetExcelTemplatePath(string templateName, string fileExtension = "xls")
    {
        if (string.IsNullOrWhiteSpace(templateName))
        {
            throw new ArgumentNullException(nameof(templateName), "Template Name cannot be null or empty");
        }

        var relativePath = string.Format(ExcelTemplateDir, templateName, fileExtension);
        var absolutePath = Path.Combine(webHostEnvironment.ContentRootPath, relativePath);

        if (File.Exists(absolutePath))
        {
            return absolutePath;
        }

        throw new FileNotFoundException($"Template {absolutePath} does not exist");
    }
}
