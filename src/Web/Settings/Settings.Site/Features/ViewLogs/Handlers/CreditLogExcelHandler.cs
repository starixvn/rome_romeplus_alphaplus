using System.Text;
using AlphaPlus.Settings.Site.Features.ViewLogs.Requests.Credit;
using AlphaPlus.Settings.ViewLogs.API.Services;
using ViewLogResources = AlphaPlus.Shared.AppResources.Settings.ViewLog.Resources;
using MemberInfoResources = AlphaPlus.Shared.AppResources.Settings.MemberInfo.Resources;
using AlphaPlus.Settings.ViewLogs.API.Models;
using AlphaPlus.Settings.ViewLogs.API.Helpers;
using MediatR;
using ONEbook.SpreadSheets;
using static ONEbook.SpreadSheets.Column;
using AlphaPlus.Core.Models;
using AlphaPlus.Settings.MemberInfo.Domain.Extensions;
using AlphaPlus.Settings.Site.Features.ViewLogs.ViewModels;
using AlphaPlus.Settings.ViewLogs.API.Extensions;

namespace AlphaPlus.Settings.Site.Features.ViewLogs.Handlers
{
    public class CreditLogExcelHandler : BaseHandler, IRequestHandler<CreditLogExcelQuery, ExcelFileViewModel>
    {
        private int currentRowIndex = 1;
        private readonly CreditLogWorkbook workbook;

        public CreditLogExcelHandler(
            [FromKeyedServices("creditLogSvc")] LogService logService,
            LogFormatter logFormatter,
            IDateTime dateTimeWrapper,
            CreditLogWorkbook workbook,
            IUserSession userSession) :
            base(logService, logFormatter, dateTimeWrapper, userSession)
        {
            this.workbook = workbook;
        }

        public async Task<ExcelFileViewModel> Handle(CreditLogExcelQuery message, CancellationToken cancellation)
        {
            var logsParam = CreateLogParam(message);
            
            // For Excel export, we want all data, not just a page
            logsParam.PageSize = int.MaxValue; // Get all records
            logsParam.PageIndex = 0; // Start from the first page
            
            var pagingLogs = await LogService.GetPagingLogs(logsParam);
            
            // Log the data count for debugging
            var logCount = pagingLogs.Logs.Count();
            
            if (logCount == 0)
            {
                // If no data found, create a message to inform the user
                var emptyLogs = new List<Log>();
                var emptyFileContent = workbook
                    .SetData(emptyLogs)
                    .SetRowMappers(GetBodyRowMapper())
                    .UseTemplate(GetTemplateInfo())
                    .Sheet("Sheet1")
                    .Title(GetReportTitle(message))
                    .Header(key =>
                    {
                        return key switch
                        {
                            "No" => ViewLogResources.No,
                            "Username" => ViewLogResources.Username,
                            "Action" => ViewLogResources.Action,
                            "Details" => ViewLogResources.Details,
                            "IP" => ViewLogResources.IP,
                            _ => string.Empty
                        };
                    })
                    .Body(emptyLogs, GetBodyRowMapper())
                    .ExportToBytes();

                return new ExcelFileViewModel(GetExcelFileName(message), emptyFileContent);
            }

            // Process logs to add nickname hiding logic and pre-load action names
            foreach (var log in pagingLogs.Logs)
            {
                if (log == null)
                {
                    continue; // Skip null logs
                }
                
                // Pre-load action name to avoid async issues in mapper
                log.ActionName = await LogService.GetLogActionName(log.ActionId, message.Language);
            }

            var fileContent = workbook
                .SetData(pagingLogs.Logs)
                .SetRowMappers(GetBodyRowMapper())
                .UseTemplate(GetTemplateInfo())
                .Sheet("Sheet1")
                .Title(GetReportTitle(message))
                .Header(key =>
                {
                    return key switch
                    {
                        "No" => ViewLogResources.No,
                        "Username" => ViewLogResources.Username,
                        "Action" => ViewLogResources.Action,
                        "Details" => ViewLogResources.Details,
                        "IP" => ViewLogResources.IP,
                        _ => string.Empty
                    };
                })
                .Body(pagingLogs.Logs, GetBodyRowMapper())
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message), fileContent);
        }

        internal Mapper<Log> GetBodyRowMapper()
        {
            // Reset currentRowIndex for each request to prevent shared state issues
            currentRowIndex = 1;
            
            var mapper = new Mapper<Log>()
                .Map(A, _ => currentRowIndex++)
                .Map(B, log => log.CustName)
                .Map(C, log => {
                    var actionName = log.ActionName;
                    if (LogViewModel.ShowEditedBy(log.ActionId))
                    {
                        actionName += $"{Environment.NewLine}{ViewLogResources.By} {log.Username} {log.LogDate}{Environment.NewLine}#{log.LogId}";
                    }
                    return actionName;
                })
                .Map(D, log => {
                    var details = log.Details;
                    details += $"{Environment.NewLine}{Fanex.Common.Converter.ToDecimal(log.TrackingValue).ToNumericString()}";
                    if (!string.IsNullOrEmpty(log.TrackingValueOld))
                    {
                        details += $"{Environment.NewLine}({Fanex.Common.Converter.ToDecimal(log.TrackingValueOld).ToNumericString()})";
                    }
                    return details;
                })
                .Map(E, log => log.IP);

            return mapper;
        }

        private static string GetExcelFileName(CreditLogExcelQuery message)
        {
            var fromDate = DateTime.TryParse(message.FromDate, out var from) ? from : DateTime.Now;
            var toDate = DateTime.TryParse(message.ToDate, out var to) ? to : DateTime.Now;
            return $"Credit Log {message.Username} -- {fromDate.ToShortDateWithHyphen()} -- {toDate.ToShortDateWithHyphen()}";
        }

        private TemplateInfo GetTemplateInfo()
        {
            var templatePath = GetExcelTemplatePath("CreditLogs");
            
            var templateInfo = new TemplateInfo(
                   templatePath,
                   noInfomationText: ViewLogResources.NoInfo,
                   titleStartIndex: 1,
                   headerStartIndex: 2,
                   columnStartName: A,
                   sheetIndex: 1,
                   bodyStartIndex: 3,
                   bodyEndIndex: 3);
            
            return templateInfo;
        }

        private string GetExcelTemplatePath(string templateName, string fileExtension = "xls")
        {
            if (string.IsNullOrWhiteSpace(templateName))
            {
                throw new ArgumentNullException(nameof(templateName), "Template Name cannot be null or empty");
            }

            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            if (string.IsNullOrWhiteSpace(baseDirectory))
            {
                throw new InvalidOperationException("Base directory could not be determined");
            }

            var relativePath = Path.Combine(baseDirectory, "App_Data", "ExcelTemplates", "ViewLogs", $"{templateName}.{fileExtension}");

            if (!File.Exists(relativePath))
            {
                // Try alternative paths
                var alternativePaths = new[]
                {
                    Path.Combine(baseDirectory, "App_Data", "ExcelTemplates", "ViewLogs", $"{templateName}.{fileExtension}"),
                    Path.Combine(baseDirectory, "..", "App_Data", "ExcelTemplates", "ViewLogs", $"{templateName}.{fileExtension}"),
                    Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "ExcelTemplates", "ViewLogs", $"{templateName}.{fileExtension}")
                };

                foreach (var path in alternativePaths)
                {
                    if (File.Exists(path))
                    {
                        return path;
                    }
                }

                throw new FileNotFoundException($"Template file '{templateName}.{fileExtension}' not found in any of the expected locations. Searched paths: {string.Join(", ", alternativePaths)}");
            }

            return relativePath;
        }

        private static string GetReportTitle(CreditLogExcelQuery message)
        {
            var title = new StringBuilder();
            title.Append($"{ViewLogResources.ViewLogCredit} \n");
            title.Append($"{MemberInfoResources.From}: {message.FromDate} \n");
            title.Append($"{MemberInfoResources.To}: {message.ToDate} \n");
            title.Append($"{MemberInfoResources.Username}: {message.Username} \n");
            title.Append($"{MemberInfoResources.ExportDate}: {DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")} ");

            return title.ToString();
        }
    }
} 