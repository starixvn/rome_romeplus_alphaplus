using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WinLossAnalysis;
using AlphaPlus.Reports.Site.Features.Shared;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using ONEbook.SpreadSheets;
using static ONEbook.SpreadSheets.Column;

namespace AlphaPlus.Reports.Site.Features.WinLossAnalysis;

public interface IWinLossAnalysisWorkbook
{
    ExcelFileViewModel BuildExcelFile(
        WinLossAnalysisQuery message,
        ReportData<WinLossAnalysisModel> reportData,
        string reportTitle,
        string reportTypeName,
        string productNames);
}

public class WinLossAnalysisWorkbook(IWebHostEnvironment webHostEnvironment) :
    ReportNpoiWorkbook<WinLossAnalysisModel>(webHostEnvironment),
    IWinLossAnalysisWorkbook
{
    public ExcelFileViewModel BuildExcelFile(
        WinLossAnalysisQuery message,
        ReportData<WinLossAnalysisModel> reportData,
        string reportTitle,
        string reportTypeName,
        string productNames)
    {
        var fromDate = message.FromDate.ToShortDateWithSlash();
        var toDate = message.ToDate.ToShortDateWithSlash();

        var fileContent = UseTemplate(new TemplateInfo(
            GetExcelTemplatePath(message.ViewerLevel.GetDirectDownlineLevel().DisplayName),
                noInfomationText: Resources.NoInformationIsAvailable))
            .Title($"{reportTitle} {fromDate} --> {toDate} {Resources.of}: {productNames}")
            .Header(key => key.Equals("ReportTypeName") ? reportTypeName : Resources.ResourceManager.GetString(key))
            .Body(reportData.DataSource, GetRowMapper(message.ViewerLevel))
            .Footer(reportData.Total, GetRowMapper(message.ViewerLevel, isTotal: true))
            .Sheet(Resources.DefaultSheetName)
            .ExportToBytes();

        return new ExcelFileViewModel($"{reportTitle}{message.CustName}-{fromDate}-{toDate}", fileContent);
    }

    private static Mapper<WinLossAnalysisModel> GetRowMapper(UserLevel custLevel, bool isTotal = false)
    {
        var mapper = new Mapper<WinLossAnalysisModel>()
            .Map(A, winLossAnalysis => isTotal ? Resources.Total : winLossAnalysis.RollupKey)
            .Map(B, winLossAnalysis => winLossAnalysis.BetCount)
            .Map(C, winLossAnalysis => winLossAnalysis.Turnover)
            .Map(D, winLossAnalysis => winLossAnalysis.MemberWinLoss)
            .Map(E, winLossAnalysis => winLossAnalysis.MemberComm)
            .Map(F, winLossAnalysis => winLossAnalysis.MemberTotal)
            .Map(G, winLossAnalysis => winLossAnalysis.AgWinLoss)
            .Map(H, winLossAnalysis => winLossAnalysis.AgComm)
            .Map(I, winLossAnalysis => winLossAnalysis.AgTotal);

        if (custLevel.IsAgentLevel())
        {
            mapper.Map(J, winLossAnalysis => winLossAnalysis.Company);
        }

        if (custLevel.IsMasterOrSuperLevel())
        {
            mapper
                .Map(J, winLossAnalysis => winLossAnalysis.MAgWinLoss)
                .Map(K, winLossAnalysis => winLossAnalysis.MAgComm)
                .Map(L, winLossAnalysis => winLossAnalysis.MAgTotal);
        }

        if (custLevel.IsMasterLevel())
        {
            mapper.Map(M, winLossAnalysis => winLossAnalysis.Company);
        }

        if (custLevel.IsSuperLevel())
        {
            mapper
                .Map(M, winLossAnalysis => winLossAnalysis.SAgWinLoss)
                .Map(N, winLossAnalysis => winLossAnalysis.SAgComm)
                .Map(O, winLossAnalysis => winLossAnalysis.SAgTotal)
                .Map(P, winLossAnalysis => winLossAnalysis.Company);
        }

        return mapper;
    }
}