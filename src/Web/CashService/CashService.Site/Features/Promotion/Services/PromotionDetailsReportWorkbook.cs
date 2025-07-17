using AlphaPlus.CashService.Site.Features.Promotion.Models;
using AlphaPlus.CashService.Site.Features.Promotion.Requests;
using AlphaPlus.CashService.Site.Features.Shared;
using AlphaPlus.CashService.Site.Features.Transactions.Enum;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.Shared.AppResources.CashServices;
using AlphaPlus.Shared.AppResources.Common;
using AlphaPlus.Shared.AppResources.Report;
using ONEbook.SpreadSheets;
using static ONEbook.SpreadSheets.Column;

namespace AlphaPlus.CashService.Site.Features.Promotion.Services;

public interface IPromotionDetailsReportWorkbook
{
    ExcelFileViewModel BuildExcelFile(GetPromotionDetailsReportRequest request, PromotionDetailsReport reportData);
}

public class PromotionDetailsReportWorkbook(IWebHostEnvironment webHostEnvironment) :
    ReportNpoiWorkbook<PromotionDetails>(webHostEnvironment),
    IPromotionDetailsReportWorkbook
{
    private int number = 1;

    public ExcelFileViewModel BuildExcelFile(GetPromotionDetailsReportRequest request, PromotionDetailsReport reportData)
    {
        var fromDate = request.FromDate.ToShortDateWithSlash();
        var toDate = request.ToDate.ToShortDateWithSlash();
        var fileContent = UseTemplate(
            new TemplateInfo(
                GetExcelTemplatePath("PromotionDetailsReport"),
                noInfomationText: CommonResx.NoInformationIsAvailable,
                bodyStartIndex: 3,
                bodyEndIndex: 4))
            .Title(BuildTitle(request.PromotionTitle, fromDate, toDate, reportData.Summary))
            .Header(key => key switch
            {
                "#" => "#",
                "UsernameNickname" => $"{Resources.Username}\n{CommonResx.Nickname}",
                _ => CashServicesResx.ResourceManager.GetString(key)
            })
            .Body(reportData.PromotionDetails, GetRowMapper())
            .ExportToBytes();

        return new ExcelFileViewModel($"{CashServicesResx.PromotionReport}-{request.Username}-{fromDate}-{toDate}", fileContent);
    }

    private static string BuildTitle(
        string promotionTitle,
        string fromDate,
        string toDate,
        PromotionDetailsSummary summary)
        => string.Join("\n", [
            $"{promotionTitle} {fromDate} --> {toDate}",
            $"{CashServicesResx.TotalBonusGiven}: {summary.TotalBonus}",
            $"{CashServicesResx.TotalRequestApproved}: {summary.TotalApproved}",
            $"{CashServicesResx.TotalRequestRejected}: {summary.TotalRejected}"
        ]);

    private Mapper<PromotionDetails> GetRowMapper()
        => new Mapper<PromotionDetails>()
        .Map(A, p => number++)
        .Map(B, p => BuildTextBreakline(p.Username, p.Nickname))
        .Map(C, p => BuildTextBreakline(p.FirstName, p.LastName))
        .Map(D, p => p.Period)
        .Map(E, p => p.Amount)
        .Map(F, p => string.IsNullOrWhiteSpace(p.SubAccName) ? p.OperatedBy : p.SubAccName)
        .Map(G, p => p.LastUpdated)
        .Map(H, p => p.StatusId switch
        {
            (byte)TransactionStatus.Completed => CashServicesResx.Approved,
            (byte)TransactionStatus.Pending => Resources.Pending,
            (byte)TransactionStatus.Rejected => Resources.Rejected,
            _ => string.Empty
        });

    private static string BuildTextBreakline(string text1, string text2)
    {
        if (string.IsNullOrWhiteSpace(text1) && string.IsNullOrWhiteSpace(text2))
        {
            return string.Empty;
        }

        if (!string.IsNullOrWhiteSpace(text1) && !string.IsNullOrWhiteSpace(text2))
        {
            return $"{text1}\n{text2}";
        }

        return string.IsNullOrWhiteSpace(text1) ? text2 : text1;
    }
}