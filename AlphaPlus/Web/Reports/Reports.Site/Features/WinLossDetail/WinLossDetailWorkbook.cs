using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WinLossDetail;
using AlphaPlus.Reports.Site.Features.Shared;
using ONEbook.SpreadSheets;
using static ONEbook.SpreadSheets.Column;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.WinLossDetail;

public interface IWinLossDetailWorkbook
{
    ExcelFileViewModel BuildExcelFile(WinLossDetailExcelNewQuery message, ReportData<WinLossDetailModel> reportData, string productNames);

    ExcelFileViewModel BuildExcelFile(WinLossDetailExcelQuery message, ReportData<WinLossDetailModel> reportData, string productNames);

    ExcelFileViewModel BuildExcelFile(
        DateTime fromDate,
        DateTime toDate,
        string custName,
        UserLevel custLevel,
        bool showFullName,
        ReportData<WinLossDetailModel> reportData,
        string productNames);
}

public class WinLossDetailWorkbook(IWebHostEnvironment webHostEnvironment) :
    ReportNpoiWorkbook<WinLossDetailModel>(webHostEnvironment),
    IWinLossDetailWorkbook
{
    public ExcelFileViewModel BuildExcelFile(WinLossDetailExcelNewQuery message, ReportData<WinLossDetailModel> reportData, string productNames)
        => BuildExcelFile(message.FromDate, message.ToDate, message.CustName, message.CustLevel, message.ShowFullName, reportData, productNames);

    public ExcelFileViewModel BuildExcelFile(WinLossDetailExcelQuery message, ReportData<WinLossDetailModel> reportData, string productNames)
        => BuildExcelFile(message.FromDate, message.ToDate, message.CustName, message.CustLevel, false, reportData, productNames);

    public ExcelFileViewModel BuildExcelFile(
        DateTime fromDate,
        DateTime toDate,
        string custName,
        UserLevel custLevel,
        bool showFullName,
        ReportData<WinLossDetailModel> reportData,
        string productNames)
    {
        var fromDateString = fromDate.ToShortDateWithSlash();
        var toDateString = toDate.ToShortDateWithSlash();

        var fileContent = UseTemplate(
            new TemplateInfo(
                GetExcelTemplatePath($"{custLevel.GetDirectDownlineLevel().DisplayName}{(showFullName ? "New" : string.Empty)}"),
                noInfomationText: Resources.NoInformationIsAvailable,
                freezeRowIndex: 4,
                freezeColumnName: B))
            .Title($"{Resources.WinLossDetail} {fromDateString} --> {toDateString} - {Resources.of2}: {productNames}")
            .Header(Resources.ResourceManager.GetString)
            .Body(reportData.DataSource, showFullName ? GetRowMapperWithFullName(custLevel) : GetRowMapper(custLevel))
            .Footer(reportData.Total, showFullName ? GetRowMapperWithFullName(custLevel, true) : GetRowMapper(custLevel, true))
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

    internal Mapper<WinLossDetailModel> GetRowMapperWithFullName(UserLevel custLevel, bool isTotal = false)
    {
        var mapper = new Mapper<WinLossDetailModel>();
        if (isTotal)
        {
            mapper
                .Map(A, winLossDetail => Resources.Total)
                .Map(B, winLossDetail => string.Empty);
        }
        else
        {
            mapper
                .Map(A, winLossDetail => winLossDetail.CustId == 0 ? Resources.DormantAccounts : winLossDetail.CustName)
                .Map(B, winLossDetail => winLossDetail.FullName ?? string.Empty);
        }

        mapper
            .Map(C, winLossDetail => winLossDetail.NumberOfTickets)
            .Map(D, winLossDetail => winLossDetail.Turnover)
            .Map(E, winLossDetail => winLossDetail.NetTurnover)
            .Map(F, winLossDetail => winLossDetail.GrossComm)
            .Map(G, winLossDetail => winLossDetail.MemberWinLoss)
            .Map(H, winLossDetail => winLossDetail.MemberComm)
            .Map(I, winLossDetail => winLossDetail.MemberTotal)
            .Map(J, winLossDetail => winLossDetail.AgWinLoss)
            .Map(K, winLossDetail => winLossDetail.AgComm)
            .Map(L, winLossDetail => winLossDetail.AgTotal);

        if (custLevel.IsAgentLevel())
        {
            mapper.Map(M, winLossDetail => winLossDetail.Company);
        }

        if (custLevel.IsMasterOrSuperLevel())
        {
            mapper
                .Map(M, winLossDetail => winLossDetail.MAgWinLoss)
                .Map(N, winLossDetail => winLossDetail.MAgComm)
                .Map(O, winLossDetail => winLossDetail.MAgTotal);
        }

        if (custLevel.IsMasterLevel())
        {
            mapper.Map(P, winLossDetail => winLossDetail.Company);
        }

        if (custLevel.IsSuperLevel())
        {
            mapper
                .Map(P, winLossDetail => winLossDetail.SAgWinLoss)
                .Map(Q, winLossDetail => winLossDetail.SAgComm)
                .Map(R, winLossDetail => winLossDetail.SAgTotal)
                .Map(S, winLossDetail => winLossDetail.Company);
        }

        return mapper;
    }
}