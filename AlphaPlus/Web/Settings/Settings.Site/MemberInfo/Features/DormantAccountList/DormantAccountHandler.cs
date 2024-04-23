namespace AlphaPlus.Settings.Site.Features.DormantAccountList
{
    using System;
    using System.Threading.Tasks;
    using AlphaPlus.Settings.Site.Features.DormantAccountList.Requests;
    using AlphaPlus.Settings.Site.Features.DormantAccountList.ViewModels;
    using AlphaPlus.Settings.Site.Handlers;
    using AlphaPlus.Settings.Site.ViewModels;
    using AlphaPlus.Settings.MemberInfo.API.Features.DormantAccountList;
    using AlphaPlus.Settings.MemberInfo.Domain.AppResources;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.DormantAccountList.Models;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.DormantAccountList.Params;
    using MediatR;
    using ONEbook.SpreadSheets;
    using static ONEbook.SpreadSheets.Column;
    using AlphaPlus.Settings.MemberInfo.Domain.Extensions;

    public class DormantAccountHandler :
        ExcelBaseHandler,
        IRequestHandler<DormantAccountQuery, DormantAccountViewModel>,
        IRequestHandler<DormantAccountExcelQuery, ExcelFileViewModel>
    {
        private readonly DormantAccountService service;
        private readonly DormantAccountWorkbook workbook;

        public DormantAccountHandler(DormantAccountService service, DormantAccountWorkbook workbook)
        {
            this.service = service;
            this.workbook = workbook;
        }

        public async Task<DormantAccountViewModel> Handle(DormantAccountQuery message, CancellationToken cancellation)
        {
            var report = await service.GetDormantAccountsAsync(GetDormantAccountParam(message));

            return new DormantAccountViewModel(report, message, IsShowDateTimeColumn(message));
        }

        public async Task<ExcelFileViewModel> Handle(DormantAccountExcelQuery message, CancellationToken cancellation)
        {
            message.PageIndex = 1;
            message.PageSize = int.MaxValue - 1;
            var report = await service.GetDormantAccountsAsync(GetDormantAccountParam(message));

            var isShowDateTimeColumn = IsShowDateTimeColumn(message);
            var totalMapper = GetTotalRowMapper();

            var fileContent = workbook
                .SetData(report.DormantAccounts, report.TotalAccount)
                .SetRowMappers(GetBodyRowMapper(isShowDateTimeColumn), totalMapper)
                .UseTemplate(GetTemplateInfo(isShowDateTimeColumn))
                .Title(GetReportTitle())
                .Header(Resources.ResourceManager.GetString)
                .Sheet(Resources.DormantAccountList)
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(), fileContent);
        }

        private static DormantAccountParam GetDormantAccountParam(DormantAccountQuery message)
            => new DormantAccountParam(message.ViewerId, message.Username, message.LevelId, message.StatusId, message.PageIndex, message.PageSize);

        private static bool IsShowDateTimeColumn(DormantAccountQuery message)
            => message.LimitedDateToShowInfo == default(DateTime);

        #region Excel

        private TemplateInfo GetTemplateInfo(bool isShowDateTimeColumn)
            => isShowDateTimeColumn
            ? new TemplateInfo(GetExcelTemplatePath("DormantAccountList"), Resources.NoInformationIsAvailable)
            : new TemplateInfo(
                GetExcelTemplatePath("DormantAccountListHideDateTime"),
                Resources.NoInformationIsAvailable,
                bodyStartIndex: 3,
                bodyEndIndex: 4);

        private static string GetReportTitle()
            => $"{Resources.DormantAccountList} {DateTime.Today.ToShortDateWithSlash()}";

        private static Mapper<DormantAccount> GetBodyRowMapper(bool isShowDateTimeColumn)
        {
            var mapper = new Mapper<DormantAccount>()
                .Map(A, dormantAccount => dormantAccount.Username)
                .Map(B, dormantAccount => dormantAccount.RoleName)
                .Map(C, dormantAccount => dormantAccount.Status)
                .Map(D, dormantAccount => dormantAccount.ArchiveStage)
                .Map(E, dormantAccount => MapDecimal(dormantAccount.ToBeClearedGivenCredit))
                .Map(F, dormantAccount => MapDecimal(dormantAccount.ClearedGivenCredit))
                .Map(G, dormantAccount => dormantAccount.YesterdayBalance);

            if (isShowDateTimeColumn)
            {
                mapper.Map(H, dormantAccount => dormantAccount.LastLogged.ToLongDateTime12h())
                    .Map(I, dormantAccount => dormantAccount.ClosedDate.ToLongDateTime12h())
                    .Map(J, dormantAccount => dormantAccount.DisabledDate.ToLongDateTime12h())
                    .Map(K, dormantAccount => dormantAccount.SettingDeletedDate.ToLongDateTime12h())
                    .Map(L, dormantAccount => dormantAccount.AccountDeletedDate.ToLongDateTime12h());
            }

            return mapper;
        }

        private static Mapper<DormantAccount> GetTotalRowMapper()
            => new Mapper<DormantAccount>()
                .Map(A, total => Resources.Total)
                .Map(E, total => MapDecimal(total.ToBeClearedGivenCredit))
                .Map(F, total => MapDecimal(total.ClearedGivenCredit))
                .Map(G, total => total.YesterdayBalance);

        private static object MapDecimal(decimal? value)
        {
            if (value.HasValue)
            {
                return value;
            }

            return string.Empty;
        }

        private static string GetExcelFileName()
            => $"{Resources.DormantAccountList} - {DateTime.Today.ToString("MMddyyyy")}";

        protected override string GetExcelTemplateDir()
            => "/app/App_Data/ExcelTemplates/DormantAccountList";

        #endregion
    }
}