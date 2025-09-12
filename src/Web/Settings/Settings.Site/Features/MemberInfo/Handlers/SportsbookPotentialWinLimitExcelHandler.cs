using AlphaPlus.Core.Models;
using AlphaPlus.Settings.MemberInfo.API.Services.WinLossLimit;
using AlphaPlus.Settings.MemberInfo.Domain.Models.WinLossLimit;
using AlphaPlus.Settings.MemberInfo.Domain.Params;
using AlphaPlus.Settings.Site.Features.MemberInfo.Excel;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests.WinLossLimit;
using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.WinLossLimit;
using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
using MediatR;
using ONEbook.SpreadSheets;
using static ONEbook.SpreadSheets.Column;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Handlers;

public class SportsbookPotentialWinLimitExcelHandler(
    IPotentialWinLimitService potentialWinLimitService) : ExcelBaseHandler, IRequestHandler<ExcelSportsbookPotentialWinLimitQuery, ExcelFileViewModel>
{
    public async Task<ExcelFileViewModel> Handle(ExcelSportsbookPotentialWinLimitQuery request, CancellationToken cancellationToken)
    {
        var param = new PotentialWinLimitParam(
            request.CustId,
            request.CustLevelId,
            request.Username,
            request.Status,
            request.ProblemAccount,
            request.PageSize,
            request.PageIndex,
            request.SiteDbGroupId,
            request.BetType,
            request.SportType,
            request.AccountType);
        var potentialWinLimitPage = await potentialWinLimitService.GetPotentialWinLimitPage(param, true);

        var isOfferedCashService = Core.Configurations.AppSettings.IsOfferedCashService(request.Site);
        var viewModel = new SportsbookPotentialWinLimitViewModel(isOfferedCashService, request, potentialWinLimitPage, param);
        var workbook = new PotentialWinLimitWorkbook();
        var fileName = $"{Resources.SportsbookPotentialWinLimit}";

        var fileContent = workbook
                .SetData(viewModel)
                .SetRowMappers(GetNormalRowMapper(viewModel.GetLastLoginDate, viewModel.GetMaxWinLimit, viewModel.GetWinPercent, SportsbookPotentialWinLimitViewModel.GetStatusText))
                .UseTemplate(GetTemplateInfo())
                .Title(fileName)
                .Sheet("Sheet1")
                .Header(Resources.ResourceManager.GetString)
                .ExportToBytes();

        return new ExcelFileViewModel("SportsbookPotentialWinLimit", fileContent);
    }

    protected override string GetExcelTemplateDir() => $"{AppDomain.CurrentDomain.BaseDirectory}/App_Data/ExcelTemplates/WinLossLimit";

    private static Mapper<PotentialWinLimitDetail> GetNormalRowMapper(Func<DateTime?, string> getLastLoginDate, Func<decimal, string> getMaxWinLimit, Func<decimal?, decimal, string> getWinPercent, Func<string, string> getStatusText)
    => new Mapper<PotentialWinLimitDetail>()
            .Map(A, potentialWinLimitDetail => potentialWinLimitDetail.Username)
            .Map(B, potentialWinLimitDetail => getStatusText(potentialWinLimitDetail.Status))
            .Map(C, potentialWinLimitDetail => potentialWinLimitDetail.IsProblemAccount ? Resources.Yes : Resources.No2)
            .Map(D, potentialWinLimitDetail => potentialWinLimitDetail.Credit)
            .Map(E, potentialWinLimitDetail => potentialWinLimitDetail.IsAutoReset ? Resources.Yes : Resources.No2)
            .Map(F, potentialWinLimitDetail => potentialWinLimitDetail.AccumToWin)
            .Map(G, potentialWinLimitDetail => potentialWinLimitDetail.AccumWinLoss)
            .Map(H, potentialWinLimitDetail => potentialWinLimitDetail.PotentialWin)
            .Map(I, potentialWinLimitDetail => getMaxWinLimit(potentialWinLimitDetail.PotentialWinLimit))
            .Map(J, potentialWinLimitDetail => getWinPercent(potentialWinLimitDetail.PotentialWinPercent, potentialWinLimitDetail.PotentialWinLimit))
            .Map(K, potentialWinLimitDetail => getLastLoginDate(potentialWinLimitDetail.LastLogged));

    private TemplateInfo GetTemplateInfo()
        => new(GetExcelTemplatePath("SportsbookPotentialWinLimit"), Resources.NoInformationIsAvailable, headerStartIndex: 2, bodyStartIndex: 3);
}