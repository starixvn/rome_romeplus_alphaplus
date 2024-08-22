namespace AlphaPlus.Settings.Site.Features.Transfer.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using AlphaPlus.Settings.Transfer.API.Models.TransferToDownlineHistory;
    using AlphaPlus.Settings.Transfer.API.Params.TransferToDownlineHistory;
    using AlphaPlus.Settings.Transfer.API.Services;

    public class TransferToDownlineHistoryBaseHandler
    {
        private readonly TransferToDownlineHistoryService transferToDownlineHistoryService;

        private const string ExcelTemplateDir = "App_Data/ExcelTemplates/TransferToDownlineHistory/{0}.{1}";

        protected internal Func<string, bool> IsFileExist { get; set; } = File.Exists;

        public TransferToDownlineHistoryBaseHandler(TransferToDownlineHistoryService transferToDownlineHistoryService)
        {
            this.transferToDownlineHistoryService = transferToDownlineHistoryService;
        }

        protected internal Tuple<IEnumerable<TransferToDownlineHistoryModel>, int> GetTransferToDownlineHistory(TransferToDownlineHistoryPagingParam param)
            => transferToDownlineHistoryService.GetTransferToDownlineHistory(param);

        protected internal virtual string GetExcelTemplatePath(string templateName, string fileExtension = "xls")
        {
            if (string.IsNullOrWhiteSpace(templateName))
            {
                throw new ArgumentNullException(nameof(templateName), "Template Name cannot be null or empty!");
            }

            var relativePath = string.Format($"{AppDomain.CurrentDomain.BaseDirectory}/{ExcelTemplateDir}", templateName, fileExtension);

            var absolutePath = relativePath;

            if (IsFileExist(absolutePath))
            {
                return absolutePath;
            }

            throw new FileNotFoundException($"Template {absolutePath} does not exist!");
        }
    }
}