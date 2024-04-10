using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WinLossDetail;
using AlphaPlus.Reports.Site.Features.Shared;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using ONEbook.SpreadSheets;
using static ONEbook.SpreadSheets.Column;

namespace AlphaPlus.Reports.Site.Features.WinLossDetail;

public interface IWinLossDetailWorkbook
{
    ExcelFileViewModel BuildExcelFile(WinLossDetailExcelQuery message, ReportData<WinLossDetailModel> reportData, string productNames);

    ExcelFileViewModel BuildExcelFile(
        DateTime fromDate,
        DateTime toDate,
        string custName,
        UserLevel custLevel,
        ReportData<WinLossDetailModel> reportData,
        string productNames);
}

public class WinLossDetailWorkbook(IWebHostEnvironment webHostEnvironment) :
    ReportNpoiWorkbook<WinLossDetailModel>(webHostEnvironment),
    IWinLossDetailWorkbook
{
    public ExcelFileViewModel BuildExcelFile(WinLossDetailExcelQuery message, ReportData<WinLossDetailModel> reportData, string productNames)
        => BuildExcelFile(message.FromDate, message.ToDate, message.CustName, message.CustLevel, reportData, productNames);

    public ExcelFileViewModel BuildExcelFile(
        DateTime fromDate,
        DateTime toDate,
        string custName,
        UserLevel custLevel,
        ReportData<WinLossDetailModel> reportData,
        string productNames)
    {
        var fromDateString = fromDate.ToShortDateWithSlash();
        var toDateString = toDate.ToShortDateWithSlash();

        var fileContent = UseTemplate(
            new TemplateInfo(
                GetExcelTemplatePath(custLevel.GetDirectDownlineLevel().DisplayName),
                noInfomationText: Resources.NoInformationIsAvailable,
                freezeRowIndex: 4,
                freezeColumnName: B))
            .Title($"{Resources.WinLossDetail} {fromDateString} --> {toDateString} - {Resources.of2}: {productNames}")
            .Header(Resources.ResourceManager.GetString)
            .Body(reportData.DataSource, GetRowMapper(custLevel))
            .Footer(reportData.Total, GetRowMapper(custLevel, true))
            .Sheet(Resources.DefaultSheetName)
            .ExportToBytes();

        return new ExcelFileViewModel($"{Resources.WinLossDetail}-{custName}-{fromDateString}-{toDateString}", fileContent);
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
            mapper.Map(L, winLossDetail => winLossDetail.Company);
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
            mapper.Map(O, winLossDetail => winLossDetail.Company);
        }

        if (custLevel.IsSuperLevel())
        {
            mapper
                .Map(O, winLossDetail => winLossDetail.SAgWinLoss)
                .Map(P, winLossDetail => winLossDetail.SAgComm)
                .Map(Q, winLossDetail => winLossDetail.SAgTotal)
                .Map(R, winLossDetail => winLossDetail.Company);
        }

        return mapper;
    }
}