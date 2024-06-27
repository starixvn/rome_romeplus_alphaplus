namespace AlphaPlus.Settings.Site.Features.MemberInfo.Handlers
{
    using System;
    using System.Text;
    using AlphaPlus.Settings.Site.Features.MemberInfo.Requests.ProblemAccount;
    using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels;
    using AlphaPlus.Settings.MemberInfo.API.Services;
    using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
    using AlphaPlus.Settings.MemberInfo.Domain.Constants;
    using AlphaPlus.Settings.MemberInfo.Domain.Models.ProblemAccount;
    using MediatR;
    using ONEbook.SpreadSheets;
    using static ONEbook.SpreadSheets.Column;
    using AlphaPlus.Settings.MemberInfo.Domain.Extensions;

    public class ProblemAccountExcelHandler : ExcelBaseHandler, IRequestHandler<ProblemAccountExcelQuery, ExcelFileViewModel>
    {
        private int currentRowIndex = 1;
        private readonly IProblemAccountService problemAccountService;
        private readonly ProblemAccountWorkbook workbook;

        public ProblemAccountExcelHandler(
            IProblemAccountService problemAccountService,
            ProblemAccountWorkbook workbook)
        {
            this.problemAccountService = problemAccountService;
            this.workbook = workbook;
        }

        public  async Task<ExcelFileViewModel> Handle(ProblemAccountExcelQuery message, CancellationToken cancellation)
        {
            var param = ProblemAccountHandler.CreateParam(message);

            var problemAccounts = problemAccountService.Get(param);

            var fileContent = workbook
                .SetData(problemAccounts)
                .SetRowMappers(GetBodyRowMapper(message))
                .UseTemplate(GetTemplateInfo(message))
                .Sheet("Sheet 1")
                .Title(GetReportTitle(message))
                .Header(Resources.ResourceManager.GetString)
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message), fileContent);
        }

        protected override string GetExcelTemplateDir() => $"{AppDomain.CurrentDomain.BaseDirectory}/App_Data/ExcelTemplates/ProblemAccount";

        internal Mapper<ProblemAccountModel> GetBodyRowMapper(ProblemAccountExcelQuery message)
        {
            if (message.SearchRole.IsMasterLevel())
            {
                return GetBodyRowMapperForMaster();
            }
            else if (message.SearchRole.IsAgentLevel())
            {
                return GetBodyRowMapperForAgent(message.IsViewDirectDownLine);
            }
            else
            {
                return GetBodyRowMapperForMember();
            }
        }

        private Mapper<ProblemAccountModel> GetBodyRowMapperForMaster()
        {
            var mapper = new Mapper<ProblemAccountModel>()
                .Map(A, _ => currentRowIndex++)
                .Map(B, data => data.UserName)
                .Map(C, data => data.NickName)
                .Map(D, data => data.FirstName)
                .Map(E, data => data.LastName)
                .Map(F, data => data.StatusName)
                .Map(G, data => data.CreatedDateStr)
                .Map(H, data => data.Count)
                .Map(I, data => data.ReviewStatus);

            return mapper;
        }

        private Mapper<ProblemAccountModel> GetBodyRowMapperForAgent(bool isDirectDownLine)
        {
            if (isDirectDownLine)
            {
                return new Mapper<ProblemAccountModel>()
                .Map(A, _ => currentRowIndex++)
                .Map(B, data => data.UserName)
                .Map(C, data => data.NickName)
                .Map(D, data => data.FirstName)
                .Map(E, data => data.LastName)
                .Map(F, data => data.StatusName)
                .Map(G, data => data.CreatedDateStr)
                .Map(H, data => data.Count)
                .Map(I, data => data.ReviewStatus);
            }

            var mapper = new Mapper<ProblemAccountModel>()
                .Map(A, _ => currentRowIndex++)
                .Map(B, data => data.UserName)
                .Map(C, data => data.FirstName)
                .Map(D, data => data.LastName)
                .Map(E, data => data.StatusName)
                .Map(F, data => data.CreatedDateStr)
                .Map(G, data => data.Count)
                .Map(H, data => data.ReviewStatus);

            return mapper;
        }

        private Mapper<ProblemAccountModel> GetBodyRowMapperForMember()
        {
            var mapper = new Mapper<ProblemAccountModel>()
                .Map(A, _ => currentRowIndex++)
                .Map(B, data => data.UserName)
                .Map(C, data => data.FirstName)
                .Map(D, data => data.LastName)
                .Map(E, data => data.StatusName)
                .Map(F, data => data.CreatedDateStr)
                .Map(G, data => data.ReviewStatus);

            return mapper;
        }

        private static string GetExcelFileName(ProblemAccountExcelQuery message)
            => $"Problem Account {message.FromDate.ToShortDateWithHyphen()} -- {message.ToDate.ToShortDateWithHyphen()} - {message.SearchRole.DisplayName}";

        private TemplateInfo GetTemplateInfo(ProblemAccountExcelQuery message)
        {
            var templateName = message.SearchRole.DisplayName;
            if(message.SearchRole.IsAgentLevel() && message.IsViewDirectDownLine)
            {
                templateName = $"DirectDownLine{templateName}";
            }

            return new TemplateInfo(
                   GetExcelTemplatePath(templateName),
                   noInfomationText: Resources.NoInformationIsAvailable,
                   titleStartIndex: 1,
                   headerStartIndex: 3,
                   columnStartName: A,
                   sheetIndex: 1,
                   bodyStartIndex: 4,
                   bodyEndIndex: 7);
        }

        private static string GetReportTitle(ProblemAccountExcelQuery message)
        {
            var reviewStatus = ReviewStatusConstant.GetReviewStatus(message.UserSelectedReviewedStatusId);
            var title = new StringBuilder();
            if (message.CustLevel.GetDirectDownLineLevel().Value != message.SearchRoleId)
            {
                title.Append($"{GetSourceText(message.SearchRole.GetDirectUplineLevel().DisplayName)}: {message.UplineName} \n");
            }

            title.Append($"{Resources.DateNotifiedFrom}: {message.FromDate.ToShortDateWithSlash()} \n");
            title.Append($"{Resources.To}: {message.ToDate.ToShortDateWithSlash()} \n");
            title.Append($"{Resources.ReviewStatus}: {GetSourceText(reviewStatus)} \n");
            title.Append($"{Resources.Username}: {message.UserName} \n");
            title.Append($"{Resources.ExportDate}: {DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")} ");

            return title.ToString();
        }

        private static string GetSourceText(string key)
            => Resources.ResourceManager.GetString(key);
    }
}