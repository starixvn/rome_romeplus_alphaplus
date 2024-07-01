using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.SuperWinLossDetailByPT;
using AlphaPlus.Reports.Site.Features.Shared;
using ONEbook.SpreadSheets;
using static ONEbook.SpreadSheets.Column;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.SuperWinLossDetailByPT;

public interface ISuperWinLossDetailByWorkbook
{
    ExcelFileViewModel BuildExcelFile(
        SuperWinLossDetailByPTExcelQuery message,
        IEnumerable<SuperWinLossDetailByPTModel> reportData,
        SuperWinLossDetailByPTEntity total,
        string productNames);
}

public class SuperWinLossDetailByWorkbook(IWebHostEnvironment webHostEnvironment) :
    ReportNpoiWorkbook<SuperWinLossDetailByPTEntity>(webHostEnvironment),
    ISuperWinLossDetailByWorkbook
{
    private IEnumerable<SuperWinLossDetailByPTModel> reportData;
    private IDictionary<string, Func<SuperWinLossDetailByPTEntity, object>> rowMapper;
    private int currentRowIndex;

    public ExcelFileViewModel BuildExcelFile(
        SuperWinLossDetailByPTExcelQuery message,
        IEnumerable<SuperWinLossDetailByPTModel> reportData,
        SuperWinLossDetailByPTEntity total,
        string productNames)
    {
        const string PTProductName = "PTProductName";
        var fromDate = message.FromDate.ToShortDateWithSlash();
        var toDate = message.ToDate.ToShortDateWithSlash();

        this.reportData = reportData;
        rowMapper = GetRowMapper().Mappings;

        var fileContent = UseTemplate(
            new TemplateInfo(
                GetExcelTemplatePath(message.ViewerLevel.DisplayName),
                noInfomationText: Resources.NoInformationIsAvailable))
            .Title($"{Resources.SuperWinLossDetailByPT} {fromDate} --> {toDate} - {Resources.of2}: {productNames}")
            .Header(key => key.Equals(PTProductName) ? $"{Resources.PT}/{Resources.Product}" : Resources.ResourceManager.GetString(key))
            .Footer(total, null)
            .Sheet(Resources.DefaultSheetName)
            .ExportToBytes();

        return new ExcelFileViewModel($"{Resources.SuperWinLossDetailByPT}-{message.CustName}-{fromDate}-{toDate}", fileContent);
    }

    protected override void FillBody(Sheet sheet)
    {
        if (sheet == null)
        {
            throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
        }

        RemoveTemplateRows(sheet);

        if (reportData.Any())
        {
            FillBodyRows(sheet);
            FillFooterRows(sheet);
        }
        else
        {
            FillNoInfoRow(sheet);
        }
    }

    protected override void FillBodyRows(Sheet sheet)
    {
        currentRowIndex = TemplateInfo.BodyStartIndex;

        foreach (var data in reportData)
        {
            FillWinLossDetailByPT(sheet, currentRowIndex++, data);

            foreach (var winLossDetail in data.SuperWinLossDetails)
            {
                FillWinLossDetailByProduct(sheet, currentRowIndex++, winLossDetail);
            }
        }
    }

    private void FillWinLossDetailByPT(Sheet sheet, int rowIndex, SuperWinLossDetailByPTModel data)
    {
        var row = sheet.GetRow(rowIndex);
        row.CopyCellsFrom(sheet.OddRowTemplate);
        FillCells(row, data.Total, rowMapper);
        row.SetCellValue(row.FirstCellIndex, data.SuperPTPercentage);
    }

    private void FillWinLossDetailByProduct(Sheet sheet, int rowIndex, SuperWinLossDetailByPTEntity item)
    {
        var row = sheet.GetRow(rowIndex);
        row.CopyCellsFrom(sheet.EvenRowTemplate);
        FillCells(row, item, rowMapper);
    }

    protected override void FillFooterRows(Sheet sheet)
    {
        var row = sheet.GetRow(currentRowIndex);
        row.CopyCellsFrom(sheet.FooterRowsTemplate.First());
        FillCells(row, SpreadSheet.FooterRows.First(), rowMapper);
    }

    private static Mapper<SuperWinLossDetailByPTEntity> GetRowMapper()
        => new Mapper<SuperWinLossDetailByPTEntity>()
            .Map(A, data => data.ProductName ?? Resources.Total)
            .Map(B, data => data.Turnover)
            .Map(C, data => data.NetTurnover)
            .Map(D, data => data.GrossComm)
            .Map(E, data => data.MemberWinLoss)
            .Map(F, data => data.MemberComm)
            .Map(G, data => data.MemberTotal)
            .Map(H, data => data.AgWinLoss)
            .Map(I, data => data.AgComm)
            .Map(J, data => data.AgTotal)
            .Map(K, data => data.MAgWinLoss)
            .Map(L, data => data.MAgComm)
            .Map(M, data => data.MAgTotal)
            .Map(N, data => data.SAgWinLoss)
            .Map(O, data => data.SAgComm)
            .Map(P, data => data.SAgTotal)
            .Map(Q, data => data.Company);
}