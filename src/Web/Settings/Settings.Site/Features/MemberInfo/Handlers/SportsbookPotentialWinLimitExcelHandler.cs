using AlphaPlus.Settings.MemberInfo.API.Services.WinLossLimit;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests.WinLossLimit;
using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.WinLossLimit;
using AlphaPlus.Settings.Site.Features.MemberInfo.Excel;
using MediatR;
using AlphaPlus.Core.Models;
using AlphaPlus.Settings.MemberInfo.Domain.Params;
using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
using ONEbook.SpreadSheets;
using static ONEbook.SpreadSheets.Column;
using AlphaPlus.Settings.MemberInfo.Domain.Models.WinLossLimit;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Handlers;

public class SportsbookPotentialWinLimitExcelHandler(
    IPotentialWinLimitService potentialWinLimitService) :ExcelBaseHandler, IRequestHandler<ExcelSportsbookPotentialWinLimitQuery, ExcelFileViewModel>
{
    private readonly IPotentialWinLimitService potentialWinLimitService = potentialWinLimitService;

    public async Task<ExcelFileViewModel> Handle(ExcelSportsbookPotentialWinLimitQuery request, CancellationToken cancellationToken)
    {
        var param = new PotentialWinLimitParam(
            request.CustId,
            0,
            0,
            request.Username,
            request.Status,
            request.ProblemAccount,
            request.PageSize,
            request.PageIndex,
            request.AGIP);
        var potentialWinLimitPage = await potentialWinLimitService.GetPotentialWinLimitPage(param, true);

        var viewModel = new SportsbookPotentialWinLimitViewModel(request, potentialWinLimitPage, param);
        var workbook = new PotentialWinLimitWorkbook();
        var fileName = $"{Resources.SportsbookPotentialWinLimit}";

        var fileContent = workbook
                .SetData(viewModel)
                .SetRowMappers(GetNormalRowMapper(viewModel.GetMaxWinLimit, viewModel.GetWinPercent))
                .UseTemplate(GetTemplateInfo())
                .Title(fileName)
                .Sheet("Sheet1")
                .Header(Resources.ResourceManager.GetString)
                .ExportToBytes();
        
        return new ExcelFileViewModel("SportsbookPotentialWinLimit.xlsx", fileContent);
    }
        protected override string GetExcelTemplateDir() => $"{AppDomain.CurrentDomain.BaseDirectory}/App_Data/ExcelTemplates/WinLossLimit";


        private static Mapper<PotentialWinLimitDetail> GetNormalRowMapper(Func<decimal, string> getMaxWinLimit, Func<decimal?, decimal, string> getWinPercent)
        => new Mapper<PotentialWinLimitDetail>()
                .Map(A, bettingStatistics => bettingStatistics.Username)
                .Map(B, bettingStatistics => bettingStatistics.LastLogged)
                .Map(C, bettingStatistics => bettingStatistics.Status)
                .Map(D, bettingStatistics => bettingStatistics.IsProblemAccount ? Resources.Yes : Resources.No2)
                .Map(E, bettingStatistics => bettingStatistics.Credit)
                .Map(F, bettingStatistics => bettingStatistics.IsAutoReset ? Resources.Yes : Resources.No2)
                .Map(G, bettingStatistics => bettingStatistics.AccumToWin)
                .Map(H, bettingStatistics => bettingStatistics.AccumWinLoss)
                .Map(I, bettingStatistics => bettingStatistics.PotentialWin)
                .Map(J, bettingStatistics => getMaxWinLimit(bettingStatistics.PotentialWinLimit))
                .Map(K, bettingStatistics => getWinPercent(bettingStatistics.PotentialWinPercent, bettingStatistics.PotentialWinLimit));
    
        private TemplateInfo GetTemplateInfo()
            => new(GetExcelTemplatePath("SportsbookPotentialWinLimit"), Resources.NoInformationIsAvailable, bodyStartIndex: 2);
} 