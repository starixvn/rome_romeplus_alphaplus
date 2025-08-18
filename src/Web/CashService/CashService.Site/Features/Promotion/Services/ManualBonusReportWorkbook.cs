using AlphaPlus.CashService.Site.Features.Promotion.Models;
using AlphaPlus.CashService.Site.Features.Promotion.Requests;
using AlphaPlus.CashService.Site.Features.Shared;
using AlphaPlus.Core.Models;
using AlphaPlus.Core.Extensions;
using ONEbook.SpreadSheets;
using AlphaPlus.Shared.AppResources.CashServices;
using AlphaPlus.Shared.AppResources.Common;
using AlphaPlus.Shared.AppResources.Report;
using AlphaPlus.CashService.Site.Features.Transactions.Enum;
using static ONEbook.SpreadSheets.Column;

namespace AlphaPlus.CashService.Site.Features.Promotion.Services;

public interface IManualBonusReportWorkbook
{
    ExcelFileViewModel BuildExcelFile(GetManualBonusRequest request, ManualBonus[] reportData);
}

public class ManualBonusReportWorkbook(IWebHostEnvironment webHostEnvironment) :
    ReportNpoiWorkbook<ManualBonus>(webHostEnvironment),
    IManualBonusReportWorkbook
{
    private int number = 1;

    public ExcelFileViewModel BuildExcelFile(GetManualBonusRequest request, ManualBonus[] reportData)
    {
        var fromDate = request.FromDate.ToShortDateWithSlash();
        var toDate = request.ToDate.ToShortDateWithSlash();
        var fileContent = UseTemplate(
            new TemplateInfo(
                GetExcelTemplatePath("ManualBonusReport"),
                noInfomationText: CommonResx.NoInformationIsAvailable,
                bodyStartIndex: 3,
                bodyEndIndex: 4))
            .Title($"{CashServicesResx.ManualBonus} {fromDate} --> {toDate}")
            .Header(key => key switch
            {
                "#" => "#",
                "UsernameNickname" => $"{Resources.Username}{Environment.NewLine} {CommonResx.Nickname}",
                _ => CashServicesResx.ResourceManager.GetString(key)
            })
            .Body(reportData, GetRowMapper())
            .ExportToBytes();

        return new ExcelFileViewModel($"{CashServicesResx.ManualBonus}-{request.Username}-{fromDate}-{toDate}", fileContent);
    }

    private Mapper<ManualBonus> GetRowMapper()
        => new Mapper<ManualBonus>()
        .Map(A, p => number++)
        .Map(B, p => BuildTextBreakline(p.Username, p.Nickname))
        .Map(C, p => BuildTextBreakline(p.FirstName, p.LastName))
        .Map(D, p => p.Amount)
        .Map(E, p => p.RefNo)
        .Map(F, p => (p.SubAccName ?? p.Creator) ?? string.Empty)
        .Map(G, p => p.CreatedTime.ToLongDateTimeWithSlash())
        .Map(H, p => p.StatusId == (byte)TransactionStatus.Completed ? Resources.Completed : string.Empty)
        .Map(I, p => p.Remark);

    private static string BuildTextBreakline(string text1, string text2)
    {
        if (string.IsNullOrWhiteSpace(text1) && string.IsNullOrWhiteSpace(text2))
        {
            return string.Empty;
        }

        if (!string.IsNullOrWhiteSpace(text1) && !string.IsNullOrWhiteSpace(text2))
        {
            return $"{text1}{Environment.NewLine}{text2}";
        }

        return string.IsNullOrWhiteSpace(text1) ? text2 : text1;
    }
}
