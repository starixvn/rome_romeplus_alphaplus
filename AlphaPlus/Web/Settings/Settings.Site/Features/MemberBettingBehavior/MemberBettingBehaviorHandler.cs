using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.MemberBettingBehavior.BettingStatistic;
using AlphaPlus.Settings.Site.Features.MemberBettingBehavior.Profile;
using AlphaPlus.Settings.Site.Features.MemberInfo.Handlers;
using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
using MediatR;
using ONEbook.SpreadSheets;
using static ONEbook.SpreadSheets.Column;

namespace AlphaPlus.Settings.Site.Features.MemberBettingBehavior
{
    public class MemberBettingBehaviorHandler :
        ExcelBaseHandler,
        IRequestHandler<MemberProfileQuery, CustomerProfile>,
        IRequestHandler<MemberBettingStatisticQuery, MemberBettingStatistic>,
        IRequestHandler<MemberBettingBehaviorExcelQuery, ExcelFileViewModel>
    {
        private readonly IMemberBettingBehaviorWorkbook workbook;
        private readonly ICustomerProfileService customerProfileService;
        private readonly IMemberBettingStatisticService memberBettingStatisticService;
        public MemberBettingBehaviorHandler(
            IMemberBettingBehaviorWorkbook workbook,
            ICustomerProfileService customerProfileService,
            IMemberBettingStatisticService memberBettingStatisticService)
        {
            this.workbook = workbook;
            this.customerProfileService = customerProfileService;
            this.memberBettingStatisticService = memberBettingStatisticService;
        }

        public async Task<CustomerProfile> Handle(MemberProfileQuery request, CancellationToken cancellationToken)
        {
            var profile = await customerProfileService.GetCustomerProfile(request.MemberCustId, request.Site);
            profile.CanUpdateAccount = request.CanUpdateAccount;

            if (!request.ViewerLevel.IsAgentLevel())
            {
                profile.IsMonitoredMember = null;
            }

            return profile;
        }

        public Task<MemberBettingStatistic> Handle(MemberBettingStatisticQuery request, CancellationToken cancellationToken)
            => memberBettingStatisticService.GetMemberBettingStatistics(
                request.MemberCustId,
                request.StartDate,
                request.EndDate,
                request.Site,
                request.LangKey);

        public async Task<ExcelFileViewModel> Handle(MemberBettingBehaviorExcelQuery request, CancellationToken cancellationToken)
        {
            var memberBettingBehaviors = await memberBettingStatisticService.GetMemberBettingStatisticExcelData(request.MemberCustId, request.StartDate, request.EndDate, request.Site, request.LangKey);

            var fileName = $"{Resources.MemberBettingBehavior}-{request.UserName}-{request.EndDate.AddDays(-request.DateRange):MM/dd/yyyy}-{request.EndDate:MM/dd/yyyy}";
            var fileContent = workbook
                .SetData(memberBettingBehaviors.BettingStatisticExportRows, memberBettingBehaviors.Total)
                .SetRowMappers(GetNormalRowMapper(), GetTotalRowMapper())
                .UseTemplate(GetTemplateInfo())
                .Title(fileName)
                .Header(Resources.ResourceManager.GetString)
                .ExportToBytes();

            return new ExcelFileViewModel(fileName, fileContent);
        }

        protected override string GetExcelTemplateDir() => "~/App_Data/ExcelTemplates/MemberBettingBehavior";

        private static Mapper<BettingStatisticExportRow> GetBodyRowMapper()
            => new Mapper<BettingStatisticExportRow>()
                .Map(C, bettingStatistics => bettingStatistics.WinningWinLoss)
                .Map(D, bettingStatistics => bettingStatistics.LosingWinLoss)
                .Map(E, bettingStatistics => bettingStatistics.TotalWinLoss)
                .Map(F, bettingStatistics => bettingStatistics.Margin)
                .Map(G, bettingStatistics => bettingStatistics.LiveTurnover)
                .Map(H, bettingStatistics => bettingStatistics.NonLiveTurnover)
                .Map(I, bettingStatistics => bettingStatistics.MixTurnover)
                .Map(J, bettingStatistics => bettingStatistics.Turnover)
                .Map(K, bettingStatistics => bettingStatistics.NetTurnOver);

        private static Mapper<BettingStatisticExportRow> GetNormalRowMapper()
            => GetBodyRowMapper()
                .Map(A, bettingStatistics => bettingStatistics.Sporttype)
                .Map(B, bettingStatistics => bettingStatistics.Bettype);

        private static Mapper<BettingStatisticExportRow> GetTotalRowMapper()
            => GetBodyRowMapper().Map(A, total => Resources.Total);

        private TemplateInfo GetTemplateInfo()
            => new(GetExcelTemplatePath("MemberBettingBehavior"), Resources.NoInformationIsAvailable, bodyStartIndex: 4);
    }
}