using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WinLossDetail;
using AlphaPlus.Reports.Site.Features.Shared;
using ONEbook.SpreadSheets;
using AlphaPlus.Shared.AppResources.Report;
using static ONEbook.SpreadSheets.Column;

namespace AlphaPlus.Reports.Site.Features.Consolidation;

public interface IConsolidationDetailWorkbook
{
    ExcelFileViewModel BuildExcelFile(ConsolidationDetailExcelQuery message, ReportData<WinLossDetailModel> reportData, string productNames);
}

public class ConsolidationDetailWorkbook(IWebHostEnvironment webHostEnvironment) :
    ReportNpoiWorkbook<WinLossDetailModel>(webHostEnvironment),
    IConsolidationDetailWorkbook
{
    public ExcelFileViewModel BuildExcelFile(ConsolidationDetailExcelQuery message, ReportData<WinLossDetailModel> reportData, string productNames)
    {
        var fromDateString = message.FromDate.ToShortDateWithSlash();
        var toDateString = message.ToDate.ToShortDateWithSlash();

        var fileContent = UseTemplate(
            new TemplateInfo(
                GetExcelTemplatePath($"{message.CustLevel.GetDirectDownLineLevel().DisplayName}"),
                noInfomationText: Resources.NoInformationIsAvailable,
                freezeRowIndex: 4,
                freezeColumnName: B))
            .Title($"{Resources.WinLossDetail} {fromDateString} --> {toDateString} {Resources.of2} {productNames}")
            .Header(Resources.ResourceManager.GetString)
            .Body(reportData.DataSource, GetRowMapper(message.CustLevel))
            .Footer(reportData.Total, GetRowMapper(message.CustLevel, true))
            .Sheet(Resources.DefaultSheetName)
            .ExportToBytes();

        return new ExcelFileViewModel($"{Resources.WinLossDetail}-{message.CustName}-{fromDateString}-{toDateString}", fileContent);
    }

    private static Mapper<WinLossDetailModel> GetRowMapper(UserLevel custLevel, bool isTotal = false)
    {
        var mapper = new Mapper<WinLossDetailModel>()
            .Map(A, winLossDetail => isTotal ? Resources.Total : (winLossDetail.CustId == 0 ? Resources.DormantAccounts : winLossDetail.CustName))
            .Map(B, winLossDetail => winLossDetail.NumberOfTickets)
            .Map(C, winLossDetail => winLossDetail.Turnover)
            .Map(D, winLossDetail => winLossDetail.NetTurnover)
            .Map(E, winLossDetail => winLossDetail.GrossComm)
            .Map(F, winLossDetail => winLossDetail.MemberWinLoss)
            .Map(G, winLossDetail => winLossDetail.MemberComm)
            .Map(H, winLossDetail => winLossDetail.MemberTotal)
            .Map(I, winLossDetail => winLossDetail.AgWinLoss)
            .Map(J, winLossDetail => winLossDetail.AgComm)
            .Map(K, winLossDetail => winLossDetail.AgTotal);

        if (custLevel.IsAgentLevel())
        {
            mapper.Map(L, winLossDetail => winLossDetail.Company)
                .Map(M, winLossDetail => winLossDetail.RewardAmount)
                .Map(N, winLossDetail => isTotal ? string.Empty : winLossDetail.RewardCurrency);
        }

        if (custLevel.IsMasterOrSuperLevel())
        {
            mapper
                .Map(L, winLossDetail => winLossDetail.MAgWinLoss)
                .Map(M, winLossDetail => winLossDetail.MAgComm)
                .Map(N, winLossDetail => winLossDetail.MAgTotal);
        }

        if (custLevel.IsMasterLevel())
        {
            mapper.Map(O, winLossDetail => winLossDetail.Company)
                .Map(P, winLossDetail => winLossDetail.RewardAmount)
                .Map(Q, winLossDetail => isTotal ? string.Empty : winLossDetail.RewardCurrency);
        }

        if (custLevel.IsSuperLevel())
        {
            mapper
                .Map(O, winLossDetail => winLossDetail.SAgWinLoss)
                .Map(P, winLossDetail => winLossDetail.SAgComm)
                .Map(Q, winLossDetail => winLossDetail.SAgTotal)
                .Map(R, winLossDetail => winLossDetail.Company)
                .Map(S, winLossDetail => winLossDetail.RewardAmount)
                .Map(T, winLossDetail => isTotal ? string.Empty : winLossDetail.RewardCurrency);
        }

        return mapper;
    }
}
