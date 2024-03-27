using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WinLoss;
using AlphaPlus.Reports.Site.Features.Shared;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using ONEbook.SpreadSheets;
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
        var isAgentLevel = message.CustLevel.IsAgentLevel();

        var fileContent = UseTemplate(
            new TemplateInfo(
                GetExcelTemplatePath(message.CustLevel.GetDirectDownlineLevel().DisplayName),
                noInfomationText: Resources.NoInformationIsAvailable))
            .Title($"{Resources.WinLossTitle} {fromDate} --> {toDate} - {Resources.of2}: {productNames}")
            .Header(Resources.ResourceManager.GetString)
            .Body(reportData.DataSource, GetRowMapper(isAgentLevel))
            .Footer(reportData.Total, GetRowMapper(isAgentLevel, true))
            .Sheet(Resources.DefaultSheetName)
            .ExportToBytes();

        return new ExcelFileViewModel($"{Resources.WinLossTitle}-{message.CustName}-{fromDate}-{toDate}", fileContent);
    }

    private static Mapper<WinLossModel> GetRowMapper(bool isAgentLevel, bool isTotal = false)
    {
        var mapper = new Mapper<WinLossModel>()
            .Map(A, winLoss => isTotal ? Resources.Total : (winLoss.CustId == 0 ? Resources.DormantAccounts : winLoss.CustName))
            .Map(B, winLoss => winLoss.Turnover)
            .Map(C, winLoss => winLoss.GrossComm);

        if (isAgentLevel)
        {
            mapper
                .Map(D, winLoss => winLoss.MemberWinLoss)
                .Map(E, winLoss => winLoss.MemberComm)
                .Map(F, winLoss => winLoss.MemberTotal)
                .Map(G, winLoss => winLoss.WinLoss)
                .Map(H, winLoss => winLoss.Comm)
                .Map(I, winLoss => winLoss.Total)
                .Map(J, winLoss => winLoss.Company);
        }
        else
        {
            mapper
                .Map(D, winLoss => winLoss.DownlineTotal)
                .Map(E, winLoss => winLoss.WinLoss)
                .Map(F, winLoss => winLoss.Comm)
                .Map(G, winLoss => winLoss.Total)
                .Map(H, winLoss => winLoss.Company);
        }

        return mapper;
    }
}