using AlphaPlus.CashService.Site.Features.Promotion.Models;
using AlphaPlus.CashService.Site.Features.Shared;
using AlphaPlus.Core.Models;
using AlphaPlus.Core.Extensions;
using ONEbook.SpreadSheets;
using AlphaPlus.Shared.AppResources.CashServices;
using AlphaPlus.Shared.AppResources.Common;
using AlphaPlus.Shared.AppResources.Main;
using static ONEbook.SpreadSheets.Column;

namespace AlphaPlus.CashService.Site.Features.Promotion.Services;

public interface IPromotionReportWorkbook
{
    ExcelFileViewModel BuildExcelFile(IEnumerable<PromotionReport> reportData);
}

public class PromotionReportWorkbook(IWebHostEnvironment webHostEnvironment) :
    ReportNpoiWorkbook<PromotionReport>(webHostEnvironment),
    IPromotionReportWorkbook
{
    private int number = 1;

    public ExcelFileViewModel BuildExcelFile(IEnumerable<PromotionReport> reportData)
    {
        var fromDate = DateTime.Today.AddDays(-7).ToShortDateWithSlash();
        var toDate = DateTime.Today.ToShortDateWithSlash();
        var username = "SUPERMAY690200";
        var fileContent = UseTemplate(
            new TemplateInfo(
                GetExcelTemplatePath("PromotionReport"),
                noInfomationText: CommonResx.NoInformationIsAvailable,
                bodyStartIndex: 3,
                bodyEndIndex: 4))
            .Title($"{CashServicesResx.PromotionReport} {fromDate} --> {toDate}")
            .Header(key => key switch
            {
                "#" => "#",
                "TotalRequestApprovedRejected" => $"{CashServicesResx.TotalRequestApproved}\n{CashServicesResx.TotalRequestRejected}",
                _ => CashServicesResx.ResourceManager.GetString(key)
            })
            .Body(reportData, GetRowMapper())
            .ExportToBytes();

        return new ExcelFileViewModel($"{CashServicesResx.PromotionReport}-{username}-{fromDate}-{toDate}", fileContent);
    }

    private Mapper<PromotionReport> GetRowMapper()
        => new Mapper<PromotionReport>()
        .Map(A, p => number++)
        .Map(B, p => p.PromotionTitle)
        .Map(C, p => p.PromotionType)
        .Map(D, p => p.DataRange)
        .Map(E, p => p.TotalRequest)
        .Map(F, p => $"{p.TotalApproved}\n{p.TotalRejected}")
        .Map(G, p => p.TotalBonus)
        .Map(H, p => p.Status == 0 ? Resources.Disabled : Resources.Enabled);
}