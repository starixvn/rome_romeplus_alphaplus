namespace AlphaPlus.CashService.Site.Features.Promotion.Services;

using AlphaPlus.Core.Models;
using AlphaPlus.Shared.AppResources.CashServices;
using AlphaPlus.Shared.AppResources.Common;
using Core.Extensions;
using Models;
using ONEbook.SpreadSheets;
using Shared;
using ViewModels;
using static ONEbook.SpreadSheets.Column;

public interface IPendingPromotionReportWorkbook
{
    ExcelFileViewModel BuildExcelFile(GetPendingPromotionViewModel request, IEnumerable<PromotionTransaction> records);
}

public class PendingPromotionReportWorkbook(IWebHostEnvironment webHostEnvironment) :
    ReportNpoiWorkbook<PromotionTransaction>(webHostEnvironment), IPendingPromotionReportWorkbook
{
    private int number = 1;

    private Mapper<PromotionTransaction> GetRowMapper()
        => new Mapper<PromotionTransaction>()
            .Map(A, _ => number++)
            .Map(B, item => item.Username + WriteNickname(item.Nickname))
            .Map(C, item => $"{item.FirstName} {item.LastName}")
            .Map(D, item => item.Title)
            .Map(E, item => item.PromotionTypeName)
            .Map(F, item => item.Amount)
            .Map(G, item => $"{item.FromDate.ToShortDateWithSlash()}-{item.ToDate.ToShortDateWithSlash()}");

    public ExcelFileViewModel BuildExcelFile(GetPendingPromotionViewModel request, IEnumerable<PromotionTransaction> records)
    {
        var fileContent = UseTemplate(
                new TemplateInfo(
                    GetExcelTemplatePath("PendingPromotion"),
                    noInfomationText: AlphaPlus.Shared.AppResources.Report.Resources.NoInformationIsAvailable,
                    bodyStartIndex: 3,
                    bodyEndIndex: 4))
            .Title(GetTitle(request))
            .Header(AlphaPlus.Shared.AppResources.Report.Resources.ResourceManager.GetString)
            .Body(records, GetRowMapper())
            .Sheet(AlphaPlus.Shared.AppResources.Report.Resources.DefaultSheetName)
            .ExportToBytes();

        return new ExcelFileViewModel($"{CashServicesResx.PendingPromotion}-{request.Username}-{request.FromDate.ToShortDateWithSlash()}-{request.ToDate.ToShortDateWithSlash()}", fileContent);
    }

    protected override void FillHeader(Sheet sheet)
    {
        var row = sheet.GetRow(TemplateInfo.HeaderStartIndex);
        int colIndex = row.FirstCellIndex;

        row.SetCellValue(colIndex++, AlphaPlus.Shared.AppResources.Betlist.Services.Resources.number);
        row.SetCellValue(colIndex++, $"{AlphaPlus.Shared.AppResources.Report.Resources.Username}\n{CommonResx.Nickname}");
        row.SetCellValue(colIndex++, CashServicesResx.FirstAndLastName);
        row.SetCellValue(colIndex++, CashServicesResx.Title);
        row.SetCellValue(colIndex++, CashServicesResx.PromotionType);
        row.SetCellValue(colIndex++, AlphaPlus.Shared.AppResources.Report.Resources.Amount);
        row.SetCellValue(colIndex, AlphaPlus.Shared.AppResources.Report.Resources.DateRange);
    }

    private static string GetTitle(GetPendingPromotionViewModel param)
        => $"{CashServicesResx.PendingPromotion} {param.FromDate.ToShortDateWithSlash()} --> {param.ToDate.ToShortDateWithSlash()}";

    private static string WriteNickname(string nickName)
        => string.IsNullOrWhiteSpace(nickName) ? string.Empty : " (" + nickName + ")";
}