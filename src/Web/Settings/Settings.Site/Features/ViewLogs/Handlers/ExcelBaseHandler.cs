using AlphaPlus.Core.Models;
using AlphaPlus.Shared.AppResources.Settings.ViewLog;
using ONEbook.SpreadSheets;

namespace AlphaPlus.Settings.Site.Features.ViewLogs.Handlers
{
    public abstract class ExcelBaseHandler
    {
        private const int bodyStartIndex = 2;
        private const int bodyEndIndex = 3;

        protected abstract string GetExcelTemplateDir();

        internal Func<string, bool> IsFileExist { get; set; } = File.Exists;

        protected internal virtual TemplateInfo GetTemplateInfo(UserLevel userLevel)
            => new TemplateInfo(
                   ExcelTemplatePath(userLevel),
                   noInfomationText: Resources.NoInfo,
                   titleStartIndex: -1,
                   headerStartIndex: 1,
                   bodyStartIndex: bodyStartIndex,
                   bodyEndIndex: bodyEndIndex);

        protected internal virtual string ExcelTemplatePath(UserLevel userLevel)
            => GetExcelTemplatePath(userLevel.DisplayName);

        protected internal virtual string GetExcelTemplatePath(string templateName, string fileExtension = "xls")
        {
            if (string.IsNullOrWhiteSpace(templateName))
            {
                throw new ArgumentNullException(nameof(templateName), "Template Name cannot be null or empty");
            }

            var relativePath = $"{GetExcelTemplateDir()}/{templateName}.{fileExtension}";

            if (IsFileExist(relativePath))
            {
                return relativePath;
            }

            throw new FileNotFoundException($"Template {relativePath} does not exist");
        }
    }
} 