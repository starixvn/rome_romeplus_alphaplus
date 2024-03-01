namespace Alpha.MemberInfo.Site.Handlers
{
    using System;
    using System.IO;
    using AlphaPlus.Settings.MemberInfo.Domain.AppResources;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using ONEbook.SpreadSheets;

    public abstract class ExcelBaseHandler
    {
        private const int bodyStartIndex = 2;
        private const int bodyEndIndex = 3;
        private Func<string, string> mapPath;

        protected internal Func<string, string> MapPath
        {
            get { return mapPath; } // TODO
            set { mapPath = value; }
        }

        protected abstract string GetExcelTemplateDir();

        internal Func<string, bool> IsFileExist { get; set; } = File.Exists;

        protected internal virtual TemplateInfo GetTemplateInfo(UserLevel userLevel)
            => new TemplateInfo(
                   ExcelTemplatePath(userLevel),
                   noInfomationText: Resources.NoInformationIsAvailable,
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

            var absolutePath = MapPath(relativePath);

            if (IsFileExist(absolutePath))
            {
                return absolutePath;
            }

            throw new FileNotFoundException($"Template {absolutePath} does not exist");
        }
    }
}