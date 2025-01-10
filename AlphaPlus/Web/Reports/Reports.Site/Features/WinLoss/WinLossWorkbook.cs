using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WinLoss;
using ONEbook.SpreadSheets;
using AlphaPlus.Shared.AppResources.Report;
using AlphaPlus.Reports.Site.Features.Shared;
using static ONEbook.SpreadSheets.Column;

namespace AlphaPlus.Reports.Site.Features.WinLoss;

public interface IWinLossWorkbook
{
    ExcelFileViewModel BuildExcelFile(WinLossExcelQuery message, ReportData<WinLossModel> reportData, string productNames);
}

public class WinLossWorkbook(IWebHostEnvironment webHostEnvironment) :
    ReportNpoiWorkbook<WinLossModel>(webHostEnvironment),
    IWinLossWorkbook
{
    public ExcelFileViewModel BuildExcelFile(WinLossExcelQuery message, ReportData<WinLossModel> reportData, string productNames)
    {
        var fromDate = message.FromDate.ToShortDateWithSlash();
        var toDate = message.ToDate.ToShortDateWithSlash();
        var custLevel = message.CustLevel;
        var isAgentLevel = custLevel.IsAgentLevel();
        var levelTypeValue = custLevel.IsSuperLevel() ? Resources.MasterAgent : Resources.Agent;

        var fileContent = UseTemplate(
            new TemplateInfo(
                GetExcelTemplatePath(custLevel.GetDirectDownLineLevel().DisplayName),
                noInfomationText: Resources.NoInformationIsAvailable))
            .Title($"{Resources.WinLossTitle} {fromDate} --> {toDate} {Resources.of2} {productNames}")
            .Header(Resources.ResourceManager.GetString)
            .Body(reportData.DataSource, isAgentLevel ? GetRowMapperMember() : GetRowMapper(levelTypeValue))
            .Footer(reportData.Total, isAgentLevel ? GetRowMapperMember(isTotal: true) : GetRowMapper(levelTypeValue, isTotal: true))
            .Sheet(Resources.DefaultSheetName)
            .ExportToBytes();

        return new ExcelFileViewModel($"{Resources.WinLossTitle}-{message.CustName}-{fromDate}-{toDate}", fileContent);
    }

    private static Mapper<WinLossModel> GetRowMapper(string levelTypeValue, bool isTotal = false)
        => new Mapper<WinLossModel>()
        .Map(A, winLoss => isTotal ? Resources.Total : (winLoss.CustId == 0 ? Resources.DormantAccounts : winLoss.CustName))
        .Map(B, winLoss => winLoss.IsDirectMember ? Resources.DirectMember : levelTypeValue)
        .Map(C, winLoss => winLoss.Turnover)
        .Map(D, winLoss => winLoss.GrossComm)
        .Map(E, winLoss => winLoss.DownlineTotal)
        .Map(F, winLoss => winLoss.WinLoss)
        .Map(G, winLoss => winLoss.Comm)
        .Map(H, winLoss => winLoss.Total)
        .Map(I, winLoss => winLoss.Company);

    private static Mapper<WinLossModel> GetRowMapperMember(bool isTotal = false)
        => new Mapper<WinLossModel>()
        .Map(A, winLoss => isTotal ? Resources.Total : (winLoss.CustId == 0 ? Resources.DormantAccounts : winLoss.CustName))
        .Map(B, winLoss => winLoss.Turnover)
        .Map(C, winLoss => winLoss.GrossComm)
        .Map(D, winLoss => winLoss.MemberWinLoss)
        .Map(E, winLoss => winLoss.MemberComm)
        .Map(F, winLoss => winLoss.MemberTotal)
        .Map(G, winLoss => winLoss.WinLoss)
        .Map(H, winLoss => winLoss.Comm)
        .Map(I, winLoss => winLoss.Total)
        .Map(J, winLoss => winLoss.Company);
}