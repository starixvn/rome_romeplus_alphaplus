using AlphaPlus.CashService.Site.Features.Promotion.Models;
using AlphaPlus.CashService.Site.Features.Shared;
using AlphaPlus.Core.Models;
using AlphaPlus.Core.Extensions;
using ONEbook.SpreadSheets;
using AlphaPlus.Shared.AppResources.CashServices;
using AlphaPlus.Shared.AppResources.Common;
using AlphaPlus.Shared.AppResources.Main;
using AlphaPlus.CashService.Site.Features.Promotion.Requests;
using static ONEbook.SpreadSheets.Column;

namespace AlphaPlus.CashService.Site.Features.Promotion.Services;

public interface IPromotionReportWorkbook
{
    ExcelFileViewModel BuildExcelFile(GetPromotionReportRequest request, IEnumerable<PromotionReport> reportData);
}

public class PromotionReportWorkbook(IWebHostEnvironment webHostEnvironment) :
    ReportNpoiWorkbook<PromotionReport>(webHostEnvironment),
    IPromotionReportWorkbook
{
    private int number = 1;

    public ExcelFileViewModel BuildExcelFile(GetPromotionReportRequest request, IEnumerable<PromotionReport> reportData)
    {
        var fromDate = request.FromDate.ToShortDateWithSlash();
        var toDate = request.ToDate.ToShortDateWithSlash();
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
                "TotalRequestApprovedRejected" => $"{CashServicesResx.TotalRequestApproved}{Environment.NewLine}{CashServicesResx.TotalRequestRejected}",
                _ => CashServicesResx.ResourceManager.GetString(key)
            })
            .Body(reportData, GetRowMapper())
            .ExportToBytes();

        return new ExcelFileViewModel($"{CashServicesResx.PromotionReport}-{request.Username}-{fromDate}-{toDate}", fileContent);
    }

    private Mapper<PromotionReport> GetRowMapper()
        => new Mapper<PromotionReport>()
        .Map(A, p => number++)
        .Map(B, p => p.PromotionTitle)
        .Map(C, p => PromotionTypeName(p.PromotionType))
        .Map(D, p => $"{p.FromDate.ToShortDateWithSlash()} - {p.ToDate.ToShortDateWithSlash()}")
        .Map(E, p => p.TotalRequest)
        .Map(F, p => $"{p.TotalApproved:N0}{Environment.NewLine}{p.TotalRejected:N0}")
        .Map(G, p => p.TotalAmount)
        .Map(H, p => p.Enabled ? Resources.Enabled : Resources.Disabled);

    private static string PromotionTypeName(byte promotionType)
        => promotionType switch
        {
            1 => CashServicesResx.PromotionType_1,
            _ => string.Empty
        };
}