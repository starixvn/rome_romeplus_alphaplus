namespace AlphaPlus.Settings.Site.Features.MyWatchlist.ReachedThreshold
{
    using AlphaPlus.Settings.Site.Features.MyWatchlist.ReachedThreshold.Requests;
    using AlphaPlus.Settings.Site.Handlers;
    using AlphaPlus.Settings.Site.ViewModels;
    using AlphaPlus.Settings.MemberInfo.API.Features.MyWatchlist.ReachedThreshold;
    using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
    using AlphaPlus.Settings.MemberInfo.Domain.Constants;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.ReachedThreshold.Models;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.ReachedThreshold.Params;
    using MediatR;
    using ONEbook.SpreadSheets;
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using static ONEbook.SpreadSheets.Column;
    using AlphaPlus.Settings.MemberInfo.Domain.Extensions;

    public class ReachedThresholdExcelHandler : ExcelBaseHandler, IRequestHandler<ReachedThresholdExcelQuery, ExcelFileViewModel>
    {
        private readonly ReachedThresholdService service;
        private readonly ReachedThresholdWorkbook workbook;

        public ReachedThresholdExcelHandler(ReachedThresholdService service, ReachedThresholdWorkbook workbook)
        {
            this.service = service;
            this.workbook = workbook;
        }

        public async Task<ExcelFileViewModel> Handle(ReachedThresholdExcelQuery message, CancellationToken cancellation)
        {
            var param = new SelectReachListParam(
                 message.CustId,
                 message.Username,
                 message.FromDate,
                 message.ToDate,
                 message.Status != 0 ? ReachedThresholdStatusConstants.GetReachedThresholdStatus(message.Status) : string.Empty,
                 message.LimitType,
                 message.SortType,
                 message.IsDesc,
                 int.MaxValue - 1,
                 1,
                 message.LimitedDateToShowInfo,
                 message.Site);

            var reachlistAccountReport = await service.GetReachlistAccounts(param);

            var isShowLastLoginColumn = message.LimitedDateToShowInfo == default(DateTime);
            var fileContent = workbook
               .SetData(reachlistAccountReport.ReachlistAccounts)
               .SetRowMappers(GetBodyRowMapper(isShowLastLoginColumn))
               .UseTemplate(GetTemplateInfo(isShowLastLoginColumn))
               .Sheet("Sheet 1")
               .Title(GetReportTitle(message))
               .Header(Resources.ResourceManager.GetString)
               .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message), fileContent);
        }

        protected override string GetExcelTemplateDir() => $"{AppDomain.CurrentDomain.BaseDirectory}/App_Data/ExcelTemplates/ReachedThreshold";

        internal Mapper<ReachlistAccountModel> GetBodyRowMapper(bool isShowLastLoginColumn)
        {
            var mapper = new Mapper<ReachlistAccountModel>()
                .Map(A, data => data.RowNumber)
                .Map(B, data => data.Username);

            if (isShowLastLoginColumn)
            {
                mapper
                    .Map(C, data => data.LastLoggedString)
                    .Map(D, data => data.TranslatedStatus)
                    .Map(E, data => data.IsPAText)
                    .Map(F, data => data.LastReachedDateString)
                    .Map(G, data => data.TranslatedLastLimitTypeReach)
                    .Map(H, data => data.WinLossAmountString)
                    .Map(I, data => data.WinningThresholdString)
                    .Map(J, data => data.LosingThresholdString)
                    .Map(K, data => data.ReachedCountString);
            }
            else
            {
                mapper
                    .Map(C, data => data.TranslatedStatus)
                    .Map(D, data => data.IsPAText)
                    .Map(E, data => data.LastReachedDateString)
                    .Map(F, data => data.TranslatedLastLimitTypeReach)
                    .Map(G, data => data.WinLossAmountString)
                    .Map(H, data => data.WinningThresholdString)
                    .Map(I, data => data.LosingThresholdString)
                    .Map(J, data => data.ReachedCountString);
            }

            return mapper;
        }

        private static string GetExcelFileName(ReachedThresholdExcelQuery message)
            => $"{Resources.ReachedThreshold} {message.FromDate.ToShortDateWithHyphen()} -- {message.ToDate.ToShortDateWithHyphen()}";

        private TemplateInfo GetTemplateInfo(bool isShowLastLoginColumn)
            => new TemplateInfo(
                   GetExcelTemplatePath(isShowLastLoginColumn ? "ReachedThreshold" : "ReachedThresholdHideDateTime"),
                   noInfomationText: Resources.NoInformationIsAvailable,
                   titleStartIndex: 1,
                   headerStartIndex: 2,
                   columnStartName: A,
                   sheetIndex: 1,
                   bodyStartIndex: 3,
                   bodyEndIndex: 4);

        private static string GetReportTitle(ReachedThresholdExcelQuery message)
        {
            var title = new StringBuilder();

            title.Append($"{Resources.ReachedThreshold} - ");
            title.Append($"{Resources.From}: {message.FromDate.ToShortDateWithSlash()} - ");
            title.Append($"{Resources.To}: {message.ToDate.ToShortDateWithSlash()} - ");
            title.Append($"{Resources.Status}: " +
                $"{Resources.ResourceManager.GetString(ReachedThresholdStatusConstants.GetReachedThresholdStatus(message.Status))} - ");
            title.Append($"{Resources.ThresholdType}: " +
                $"{Resources.ResourceManager.GetString(ReachedThresholdLimitTypeConstants.GetReachedThresholdLimitType(message.LimitType))} - ");
            title.Append($"{Resources.Username}: {message.Username}");

            return title.ToString();
        }
    }
}