using AlphaPlus.Reports.Features.IPMatching;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Site.Features.Shared;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using ONEbook.SpreadSheets;
using static ONEbook.SpreadSheets.Column;

namespace AlphaPlus.Reports.Site.Features.IPMatching;

public interface IIPMatchingWorkbook
{
    ExcelFileViewModel BuildExcelFile(
        IPMatchingQuery message,
        IEnumerable<IPMatchingModel> reportData,
        IPMatchingModel total);
}

public class IPMatchingWorkbook(IWebHostEnvironment webHostEnvironment) :
    ReportNpoiWorkbook<IPMatchingModel>(webHostEnvironment),
    IIPMatchingWorkbook
{
    private readonly Mapper<IPMatchingModel> rowMapper
        = new Mapper<IPMatchingModel>()
        .Map(A, data => data.IP)
        .Map(B, data => data.MemberCount)
        .Map(C, data => data.BetCount)
        .Map(D, data => data.Stake)
        .Map(E, data => data.MemberWinLoss)
        .Map(F, data => data.SuperWinLoss)
        .Map(G, data => data.Company);

    public ExcelFileViewModel BuildExcelFile(
        IPMatchingQuery message,
        IEnumerable<IPMatchingModel> reportData,
        IPMatchingModel total)
    {
        var dateString = message.Date.ToShortDateWithSlash();

        var fileContent = UseTemplate(new TemplateInfo(
                GetExcelTemplatePath("Super"),
                noInfomationText: Resources.NoInformationIsAvailable,
                bodyStartIndex: 3,
                bodyEndIndex: 4,
                footerEndIndex: 5))
            .Title($"{Resources.IPMatching} {dateString} - {Resources.MemberCount}: >= {message.MemberCount}")
            .Header(Resources.ResourceManager.GetString)
            .Body(reportData, rowMapper)
            .Footer(total, rowMapper)
            .Sheet(Resources.DefaultSheetName)
            .ExportToBytes();

        return new ExcelFileViewModel($"{Resources.IPMatching}-{message.CustName}-{dateString}", fileContent);
    }
}
