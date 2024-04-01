using System.Globalization;
using AlphaPlus.Reports.Features.MatchWinLossDetail;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Site.Features.Shared;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using ONEbook.SpreadSheets;
using static ONEbook.SpreadSheets.Column;

namespace AlphaPlus.Reports.Site.Features.MatchWinLossDetail;

public interface IMatchWinLossDetailWorkbook
{
    ExcelFileViewModel BuildExcelFile(
        MatchWinLossDetailExcelQuery message,
        MatchWinLossDetailReport reportData,
        string productNames);
}

public class MatchWinLossDetailWorkbook(IWebHostEnvironment webHostEnvironment) :
    ReportNpoiWorkbook<BetTypeWinLossModel>(webHostEnvironment),
    IMatchWinLossDetailWorkbook
{
    private const int ReportSheetIndex = 1;
    private const int RowTypeData = 1;
    private const int RowTypeSubTotal = 2;
    private const int RowTypeTotal = 3;
    private const int RowTypeGrandTotal = 4;
    private UserLevel custLevel;
    MatchWinLossDetailReport reportData;

    private IDictionary<string, Func<BetTypeWinLossModel, object>> betTypeRowMapper;
    private IDictionary<string, Func<BetTypeWinLossModel, object>> subTotalRowMapper;
    private IDictionary<string, Func<BetTypeWinLossModel, object>> totalRowMapper;

    private Row matchTitleRowTemplate;
    private Row oddRowTemplate;
    private Row evenRowTemplate;
    private Row subTotalRowTemplate;
    private Row totalRowTemplate;
    private Row mixParlayRowTemplate;
    private Row systemParlayRowTemplate;
    private Row grandTotalRowTemplate;
    private int currentRowIndex;

    public ExcelFileViewModel BuildExcelFile(
        MatchWinLossDetailExcelQuery message,
        MatchWinLossDetailReport reportData,
        string productNames)
    {
        var fromDate = message.FromDate.ToShortDateWithSlash();
        var toDate = message.ToDate.ToShortDateWithSlash();
        custLevel = message.ViewerLevel;
        this.reportData = reportData;

        betTypeRowMapper = GetRowMapper(RowTypeData, custLevel).Mappings;
        subTotalRowMapper = GetRowMapper(RowTypeSubTotal, custLevel).Mappings;
        totalRowMapper = GetRowMapper(RowTypeTotal, custLevel).Mappings;

        var fileContent = UseTemplate(
            new TemplateInfo(
                GetExcelTemplatePath(message.ViewerLevel.GetDirectDownlineLevel().DisplayName),
                noInfomationText: Resources.NoInformationIsAvailable,
                bodyEndIndex: 12))
            .Title($"{Resources.MatchWinLossDetail} {fromDate} --> {toDate} - {Resources.of}: {productNames}")
            .Header(Resources.ResourceManager.GetString)
            .Footer(reportData.GrandTotal, GetRowMapper(RowTypeGrandTotal, custLevel))
            .ExportToBytes();

        return new ExcelFileViewModel($"{Resources.MatchWinLossDetail}-{fromDate}-{toDate}", fileContent);
    }

    protected override Sheet LoadTemplateSheet()
    {
        var sheet = GetSheetAt(TemplateInfo.SheetIndex);
        currentRowIndex = TemplateInfo.BodyStartIndex + 1;

        matchTitleRowTemplate = sheet.GetRow(currentRowIndex++);
        oddRowTemplate = sheet.GetRow(currentRowIndex++);
        evenRowTemplate = sheet.GetRow(currentRowIndex++);
        subTotalRowTemplate = sheet.GetRow(currentRowIndex++);
        totalRowTemplate = sheet.GetRow(currentRowIndex++);
        mixParlayRowTemplate = sheet.GetRow(currentRowIndex++);
        systemParlayRowTemplate = sheet.GetRow(currentRowIndex++);
        grandTotalRowTemplate = sheet.GetRow(currentRowIndex);

        return GetSheetAt(ReportSheetIndex);
    }

    protected override void RemoveTemplateRows(Sheet sheet)
        => Workbook.RemoveSheetAt(TemplateInfo.SheetIndex);

    protected override void FillBody(Sheet sheet)
    {
        if (sheet == null)
        {
            throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
        }

        RemoveTemplateRows(sheet);

        if (reportData.HasData)
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

        FillFooterRows(sheet);
        currentRowIndex++;

        foreach (var data in reportData.NormalMatchWinLoss)
        {
            FillMatchTitle(sheet, currentRowIndex++, data);

            var isEvenRow = false;
            foreach (var betTypeWinLoss in data.BetTypeWinLoss)
            {
                FillBetTypeWinLoss(sheet, currentRowIndex++, isEvenRow, betTypeWinLoss);
                isEvenRow = !isEvenRow;
            }

            if (data.RenderSubTotal)
            {
                FillMatchTotal(sheet, currentRowIndex++, data.Total);
            }
        }

        if (reportData.NormalMatchWinLoss.Any() && HasParlay())
        {
            FillTotal(sheet, currentRowIndex++, reportData.TotalNormalMatchWinLoss);
        }

        if (reportData.MixParlay != null)
        {
            FillMixParlayWinLoss(sheet, currentRowIndex++, reportData.MixParlay);
        }

        if (reportData.SystemParlay != null)
        {
            FillSystemParlayWinLoss(sheet, currentRowIndex++, reportData.SystemParlay);
        }

        foreach (var parlay in reportData.ParlayMatchWinLoss)
        {
            var row = sheet.GetRow(currentRowIndex++);
            row.CopyCellsFrom(systemParlayRowTemplate);
            FillCells(row, parlay, betTypeRowMapper);
        }
    }

    private void FillMatchTitle(Sheet sheet, int rowIndex, MatchWinLossDetailModel data)
    {
        var row = sheet.GetRow(rowIndex);
        row.CopyCellsFrom(matchTitleRowTemplate);

        var title = (data.LeagueName + (string.IsNullOrEmpty(data.MatchName) ? string.Empty : $" >> {data.MatchName}"))
            .ToUpper(CultureInfo.InvariantCulture);
        sheet.SetCellRangeValue(rowIndex, row.FirstCellIndex, rowIndex, row.LastCellIndex - 1, title);
    }

    private void FillBetTypeWinLoss(Sheet sheet, int rowIndex, bool isEvenRow, BetTypeWinLossModel betTypeWinLoss)
    {
        var row = sheet.GetRow(rowIndex);
        var templateRow = isEvenRow ? evenRowTemplate : oddRowTemplate;

        row.CopyCellsFrom(templateRow);
        FillCells(row, betTypeWinLoss, betTypeRowMapper);
    }

    private void FillMatchTotal(Sheet sheet, int rowIndex, BetTypeWinLossModel data)
    {
        var row = sheet.GetRow(rowIndex);
        row.CopyCellsFrom(subTotalRowTemplate);
        FillCells(row, data, subTotalRowMapper);
    }

    private void FillTotal(Sheet sheet, int rowIndex, BetTypeWinLossModel data)
    {
        var row = sheet.GetRow(rowIndex);
        row.CopyCellsFrom(totalRowTemplate);
        FillCells(row, data, totalRowMapper);
    }

    private void FillMixParlayWinLoss(Sheet sheet, int rowIndex, BetTypeWinLossModel data)
    {
        var row = sheet.GetRow(rowIndex);
        row.CopyCellsFrom(mixParlayRowTemplate);
        FillCells(row, data, betTypeRowMapper);
    }

    private void FillSystemParlayWinLoss(Sheet sheet, int rowIndex, BetTypeWinLossModel data)
    {
        var row = sheet.GetRow(rowIndex);
        row.CopyCellsFrom(systemParlayRowTemplate);
        FillCells(row, data, betTypeRowMapper);
    }

    protected override void FillFooterRows(Sheet sheet)
    {
        var row = sheet.GetRow(currentRowIndex);
        row.CopyCellsFrom(grandTotalRowTemplate);
        FillCells(row, SpreadSheet.FooterRows.First(), SpreadSheet.FooterRowMapper.Mappings);
    }

    protected override void FillNoInfoRow(Sheet sheet)
    {
        var rowIndex = TemplateInfo.BodyStartIndex;
        var colIndex = GetColIndex(TemplateInfo.ColumnStartName);

        sheet.SetCellRangeValue(
            rowIndex,
            colIndex,
            rowIndex,
            matchTitleRowTemplate.LastCellIndex - 1,
            TemplateInfo.NoInfomationText);

        sheet.GetCell(rowIndex, colIndex).SetCenterHorizontalAlignment();
    }

    private bool HasParlay()
        => reportData.MixParlay != null
        || reportData.SystemParlay != null
        || reportData.ParlayMatchWinLoss.Any();

    private static Mapper<BetTypeWinLossModel> GetRowMapper(int rowType, UserLevel custLevel)
    {
        var mapper = new Mapper<BetTypeWinLossModel>();

        switch (rowType)
        {
            case RowTypeData:
                mapper.Map(A, betTypeWinLoss => betTypeWinLoss.Name);
                break;

            case RowTypeSubTotal:
                mapper.Map(A, betTypeWinLoss => Resources.SubTotal);
                break;

            case RowTypeTotal:
                mapper.Map(A, betTypeWinLoss => Resources.Total);
                break;

            case RowTypeGrandTotal:
                mapper.Map(A, betTypeWinLoss => Resources.GrandTotal);
                break;
        }

        mapper
            .Map(B, betTypeWinLoss => betTypeWinLoss.Turnover)
            .Map(C, betTypeWinLoss => betTypeWinLoss.GrossComm)
            .Map(D, betTypeWinLoss => betTypeWinLoss.MemberWinLoss)
            .Map(E, betTypeWinLoss => betTypeWinLoss.MemberComm)
            .Map(F, betTypeWinLoss => betTypeWinLoss.MemberTotal)
            .Map(G, betTypeWinLoss => betTypeWinLoss.AgWinLoss)
            .Map(H, betTypeWinLoss => betTypeWinLoss.AgComm)
            .Map(I, betTypeWinLoss => betTypeWinLoss.AgTotal);

        if (custLevel.IsAgentLevel())
        {
            mapper
                .Map(J, betTypeWinLoss => betTypeWinLoss.CompanyComm)
                .Map(K, betTypeWinLoss => betTypeWinLoss.Company);
        }

        if (custLevel.IsMasterLevel())
        {
            mapper.Map(J, betTypeWinLoss => betTypeWinLoss.MAgWinLoss)
                .Map(K, betTypeWinLoss => betTypeWinLoss.MAgComm)
                .Map(L, betTypeWinLoss => betTypeWinLoss.MAgTotal)
                .Map(M, betTypeWinLoss => betTypeWinLoss.CompanyComm)
                .Map(N, betTypeWinLoss => betTypeWinLoss.Company);
        }

        if (custLevel.IsSuperLevel())
        {
            mapper.Map(J, betTypeWinLoss => betTypeWinLoss.MAgWinLoss)
                .Map(K, betTypeWinLoss => betTypeWinLoss.MAgComm)
                .Map(L, betTypeWinLoss => betTypeWinLoss.MAgTotal)
                .Map(M, betTypeWinLoss => betTypeWinLoss.SAgWinLoss)
                .Map(N, betTypeWinLoss => betTypeWinLoss.SAgComm)
                .Map(O, betTypeWinLoss => betTypeWinLoss.SAgTotal)
                .Map(P, betTypeWinLoss => betTypeWinLoss.CompanyComm)
                .Map(Q, betTypeWinLoss => betTypeWinLoss.Company);
        }

        return mapper;
    }
}