using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.CommissionByBetType;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared;
using ONEbook.SpreadSheets;
using static ONEbook.SpreadSheets.Column;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.CommissionByBetType;

public interface ICommissionByBetTypeWorkbook
{
    ExcelFileViewModel BuildExcelFile(CommissionByBetTypeExcelQuery message, ReportData<CommissionByBetTypeModel> reportData);
}

public class CommissionByBetTypeWorkbook(IWebHostEnvironment webHostEnvironment) :
    ReportNpoiWorkbook<CommissionByBetTypeModel>(webHostEnvironment),
    ICommissionByBetTypeWorkbook
{
    public ExcelFileViewModel BuildExcelFile(CommissionByBetTypeExcelQuery message, ReportData<CommissionByBetTypeModel> reportData)
    {
        var fromDate = message.FromDate.ToShortDateWithSlash();
        var toDate = message.ToDate.ToShortDateWithSlash();

        var fileContent = UseTemplate(
            new TemplateInfo(
                GetExcelTemplatePath(message.CustLevel.GetDirectDownlineLevel().DisplayName),
                noInfomationText: Resources.NoInformationIsAvailable,
                bodyStartIndex: 3,
                bodyEndIndex: 4))
            .Title($"{Resources.CommissionByBettype} {fromDate} --> {toDate}")
            .Header(Resources.ResourceManager.GetString)
            .Body(reportData.DataSource, GetRowMapper())
            .Footer(reportData.Total, GetRowMapper(true))
            .Sheet(Resources.DefaultSheetName)
            .ExportToBytes();

        return new ExcelFileViewModel($"{Resources.CommissionByBettype}-{message.CustName}-{fromDate}-{toDate}", fileContent);
    }

    private static Mapper<CommissionByBetTypeModel> GetRowMapper(bool isTotal = false)
        => new Mapper<CommissionByBetTypeModel>()
            .Map(A, commission => isTotal ? Resources.Total : commission.CustName)
            .Map(B, commission => commission.Comm1)
            .Map(C, commission => commission.Comm2)
            .Map(D, commission => commission.Comm3)
            .Map(E, commission => commission.CasinoComm);


    protected override void FillNoInfoRow(Sheet sheet)
    {
        var rowIndex = TemplateInfo.BodyStartIndex;
        var colIndex = GetColIndex(TemplateInfo.ColumnStartName);

        var lastColumnIndex = sheet.OddRowTemplate.LastCellIndex - 2;

        sheet.SetCellRangeValue(
            rowIndex,
            colIndex,
            rowIndex,
            lastColumnIndex,
            TemplateInfo.NoInfomationText);

        sheet.GetCell(rowIndex, colIndex).SetCenterHorizontalAlignment();
    }
}