using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.Shared.AppResources.Report;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WhiteLabel;
using AlphaPlus.Reports.Site.Features.Shared;
using ONEbook.SpreadSheets;
using static ONEbook.SpreadSheets.Column;

namespace AlphaPlus.Reports.Site.Features.WhiteLabel;

public interface IDailyWinLossWorkbook
{
    ExcelFileViewModel BuildExcelFile(DailyWinLossExcelQuery message, ReportData<DailyWinLossModel> reportData);
}

public class DailyWinLossWorkbook(IWebHostEnvironment webHostEnvironment) :
    ReportNpoiWorkbook<DailyWinLossModel>(webHostEnvironment),
    IDailyWinLossWorkbook
{
    public ExcelFileViewModel BuildExcelFile(DailyWinLossExcelQuery message, ReportData<DailyWinLossModel> reportData)
    {
        var fromDate = message.FromDate.ToShortDateWithSlash();
        var toDate = message.ToDate.ToShortDateWithSlash();

        var fileContent = UseTemplate(
            new TemplateInfo(
                GetExcelTemplatePath($"DailyWinLoss{message.ViewerLevel.GetDirectDownLineLevel().DisplayName}"),
                noInfomationText: Resources.NoInformationIsAvailable,
                bodyStartIndex: 3,
                bodyEndIndex: 4))
            .Title($"{Resources.DailyWinLossReport} {fromDate} --> {toDate}")
            .Header(Resources.ResourceManager.GetString)
            .Body(reportData.DataSource, GetRowMapper(message, false))
            .Footer(reportData.Total, GetRowMapper(message, true))
            .Sheet(Resources.DefaultSheetName)
            .ExportToBytes();

        return new ExcelFileViewModel($"{Resources.DailyWinLossReport}-{message.CustName}-{fromDate}-{toDate}", fileContent);
    }

    internal static Mapper<DailyWinLossModel> GetRowMapper(DailyWinLossExcelQuery message, bool isTotal)
    {
        if (message.ViewerLevel.IsSuperLevel())
        {
            return GetSuperRowMapper(isTotal);
        }

        if (message.ViewerLevel.IsMasterLevel())
        {
            return GetMaterRowMapper(isTotal);
        }

        return GetAgentRowMapper(isTotal);
    }

    internal static Mapper<DailyWinLossModel> GetSuperRowMapper(bool isTotal)
    {
        var mapper = new Mapper<DailyWinLossModel>();
        if (isTotal)
        {
            mapper.Map(A, winloss => Resources.Total);
        }
        else
        {
            mapper
                .Map(A, winloss => winloss.WinLostDate)
                .Map(B, winloss => winloss.WhiteLabelName)
                .Map(C, winloss => winloss.Agent)
                .Map(D, winloss => winloss.Master);
        }

        return mapper
            .Map(E, winloss => winloss.MemberWinLoss)
            .Map(F, winloss => winloss.Promotion);
    }

    internal static Mapper<DailyWinLossModel> GetMaterRowMapper(bool isTotal)
    {
        var mapper = new Mapper<DailyWinLossModel>();
        if (isTotal)
        {
            mapper.Map(A, winloss => Resources.Total);
        }
        else
        {
            mapper
                .Map(A, winloss => winloss.WinLostDate)
                .Map(B, winloss => winloss.WhiteLabelName)
                .Map(C, winloss => winloss.Agent);
        }

        return mapper
            .Map(D, winloss => winloss.MemberWinLoss)
            .Map(E, winloss => winloss.Promotion);
    }

    internal static Mapper<DailyWinLossModel> GetAgentRowMapper(bool isTotal)
    {
        var mapper = new Mapper<DailyWinLossModel>();
        if (isTotal)
        {
            mapper.Map(A, winloss => Resources.Total);
        }
        else
        {
            mapper.Map(A, winloss => winloss.WinLostDate);
        }

        return mapper
            .Map(B, winloss => winloss.MemberWinLoss)
            .Map(C, winloss => winloss.Promotion);
    }
}