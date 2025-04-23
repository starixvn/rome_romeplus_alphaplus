namespace AlphaPlus.Settings.Site.Features.Transfer.Handlers;

public abstract class ExcelBaseHandler
{
    internal Func<string, bool> IsFileExist { get; set; } = File.Exists;

    protected internal virtual string GetExcelTemplatePath(string folderAndFileName, string fileExtension = "xls")
    {
        if (string.IsNullOrWhiteSpace(folderAndFileName))
        {
            throw new ArgumentNullException(nameof(folderAndFileName), "Template Name cannot be null or empty");
        }

        var absolutePath = $"{AppDomain.CurrentDomain.BaseDirectory}/App_Data/ExcelTemplates/{folderAndFileName}.{fileExtension}";

        if (IsFileExist(absolutePath))
        {
            return absolutePath;
        }

        throw new FileNotFoundException($"Template {absolutePath} does not exist");
    }
}