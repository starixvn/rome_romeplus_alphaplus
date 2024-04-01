using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.WinLossByProduct;
using AlphaPlus.Reports.Site.Features.Shared;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using ONEbook.SpreadSheets;
using static ONEbook.SpreadSheets.Column;

namespace AlphaPlus.Reports.Site.Features.WinLossByProduct;

public interface IWinLossByProductWorkbook
{
    ExcelFileViewModel BuildExcelFile(
        WinLossByProductExcelQuery message,
        IEnumerable<WinLossByProductModel> reportData,
        WinLossByBetTypeModel total,
        string productNames);
}

public class WinLossByProductWorkbook(IWebHostEnvironment webHostEnvironment) :
    ReportNpoiWorkbook<WinLossByBetTypeModel>(webHostEnvironment),
    IWinLossByProductWorkbook
{
    private IEnumerable<WinLossByProductModel> reportData;
    private Mapper<WinLossByBetTypeModel> winlossByBetTypeMapper;
    private Row winlossByProductRowTemplate;
    private Row winlossBySportRowTemplate;
    private int currentRowIndex;

    public ExcelFileViewModel BuildExcelFile(
        WinLossByProductExcelQuery message,
        IEnumerable<WinLossByProductModel> reportData,
        WinLossByBetTypeModel total,
        string productNames)
    {
        var betTypeName = nameof(WinLossByBetTypeModel.BetTypeName);
        var fromDate = message.FromDate.ToShortDateWithSlash();
        var toDate = message.ToDate.ToShortDateWithSlash();

        this.reportData = reportData;
        winlossByBetTypeMapper = GetRowMapper(message.ViewerLevel);

        var fileContent = UseTemplate(
            new TemplateInfo(
                GetExcelTemplatePath(message.ViewerLevel.GetDirectDownlineLevel().DisplayName),
                noInfomationText: Resources.NoInformationIsAvailable))
            .Title($"{Resources.WinLossByProduct} {fromDate} --> {toDate} - {Resources.of2}: {productNames}")
            .Header(key => key.Equals(betTypeName) ? $"{Resources.Product}/{Resources.BetType}" : Resources.ResourceManager.GetString(key))
            .Footer(total, GetRowMapper(message.ViewerLevel, isTotal: true))
            .Sheet(Resources.DefaultSheetName)
            .ExportToBytes();

        return new ExcelFileViewModel($"{Resources.WinLossByProduct}-{message.CustName}-{fromDate}-{toDate}", fileContent);
    }

    protected override Sheet LoadTemplateSheet()
    {
        var sheet = GetSheetAt(TemplateInfo.SheetIndex);
        var currentRowIndex = TemplateInfo.BodyStartIndex;

        winlossByProductRowTemplate = sheet.GetRow(currentRowIndex++);
        winlossBySportRowTemplate = sheet.GetRow(currentRowIndex++);
        sheet.OddRowTemplate = sheet.GetRow(currentRowIndex++);
        sheet.EvenRowTemplate = sheet.GetRow(currentRowIndex++);
        sheet.FooterRowsTemplate = [sheet.GetRow(currentRowIndex)];

        return sheet;
    }

    protected override void RemoveTemplateRows(Sheet sheet)
    {
        sheet.RemoveRow(winlossByProductRowTemplate);
        sheet.RemoveRow(winlossBySportRowTemplate);

        base.RemoveTemplateRows(sheet);
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
            FillProductTitle(sheet, currentRowIndex++, data);

            foreach (var winloss in data.WinLossBySports)
            {
                FillSport(sheet, winloss, data.ProductId);
            }
        }
    }

    private void FillProductTitle(Sheet sheet, int rowIndex, WinLossByProductModel data)
    {
        var row = sheet.GetRow(rowIndex);
        row.CopyCellsFrom(winlossByProductRowTemplate);
        FillCells(row, data.Total, SpreadSheet.FooterRowMapper.Mappings);
        row.SetCellValue(row.FirstCellIndex, data.ProductName.ToUpperInvariant());
    }

    private void FillSport(Sheet sheet, WinLossBySportModel winlossBySport, int productId)
    {
        if (!WinLossBySportModel.IsShowBetTypeDirectly(productId))
        {
            FillSportTitle(sheet, currentRowIndex++, winlossBySport);
        }

        var isEvenRow = false;

        foreach (var winlossByBetType in winlossBySport.WinLossByBetTypes)
        {
            FillWinLossByBetType(sheet, currentRowIndex++, isEvenRow, winlossByBetType);
            isEvenRow = !isEvenRow;
        }
    }

    private void FillSportTitle(Sheet sheet, int rowIndex, WinLossBySportModel data)
    {
        var row = sheet.GetRow(rowIndex);
        row.CopyCellsFrom(winlossBySportRowTemplate);
        FillCells(row, data.Total, SpreadSheet.FooterRowMapper.Mappings);
        row.SetCellValue(row.FirstCellIndex, data.SportTypeName.ToUpperInvariant());
    }

    private void FillWinLossByBetType(
        Sheet sheet,
        int rowIndex,
        bool isEvenRow,
        WinLossByBetTypeModel winlossByBetType)
    {
        var row = sheet.GetRow(rowIndex);
        var templateRow = isEvenRow ? sheet.EvenRowTemplate : sheet.OddRowTemplate;

        row.CopyCellsFrom(templateRow);
        FillCells(row, winlossByBetType, winlossByBetTypeMapper.Mappings);
    }

    protected override void FillFooterRows(Sheet sheet)
    {
        var row = sheet.GetRow(currentRowIndex);
        row.CopyCellsFrom(sheet.FooterRowsTemplate.First());
        FillCells(row, SpreadSheet.FooterRows.First(), SpreadSheet.FooterRowMapper.Mappings);
    }

    private static Mapper<WinLossByBetTypeModel> GetRowMapper(UserLevel custLevel, bool isTotal = false)
    {
        var mapper = new Mapper<WinLossByBetTypeModel>()
            .Map(A, data => isTotal ? Resources.Total : data.BetTypeName)
            .Map(B, data => data.NumberOfTickets)
            .Map(C, data => data.Turnover)
            .Map(D, data => data.GrossComm);

        if (custLevel.IsAgentLevel())
        {
            mapper
                .Map(E, data => data.MemberWinLoss)
                .Map(F, data => data.MemberComm)
                .Map(G, data => data.MemberTotal)
                .Map(H, data => data.WinLoss)
                .Map(I, data => data.Comm)
                .Map(J, data => data.Total)
                .Map(K, data => data.Company);
        }
        else
        {
            mapper
                .Map(E, data => data.DownlineTotal)
                .Map(F, data => data.WinLoss)
                .Map(G, data => data.Comm)
                .Map(H, data => data.Total)
                .Map(I, data => data.Company);
        }

        return mapper;
    }
}