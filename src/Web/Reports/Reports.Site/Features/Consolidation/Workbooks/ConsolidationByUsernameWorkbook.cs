using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.Consolidation;
using AlphaPlus.Reports.Site.Features.Shared;
using ONEbook.SpreadSheets;
using static ONEbook.SpreadSheets.Column;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.Consolidation;

public interface IConsolidationByUsernameWorkbook
{
    ExcelFileViewModel BuildExcelFile(
        ConsolidationExcelQuery message,
        IEnumerable<ConsolidationByUsername> reportData,
        string productNames,
        IEnumerable<ConsolidationCurrency> currencies);
}

public class ConsolidationByUsernameWorkbook(IWebHostEnvironment webHostEnvironment) :
    ReportNpoiWorkbook<ConsolidationEntity>(webHostEnvironment),
    IConsolidationByUsernameWorkbook
{
    private const string PaddingToProductName = "    ";
    private IEnumerable<ConsolidationByUsername> reportData;

    private Row groupByUsernameRowTemplate;
    private Row groupByPTRowTemplate;
    private Row dataRowTemplate;

    private readonly Mapper<ConsolidationEntity> groupByUsernameMapper = new Mapper<ConsolidationEntity>()
        .Map(A, winLoss => winLoss.CustLevelName)
        .Map(B, winLoss => winLoss.Username)
        .Map(C, winLoss => winLoss.CurrencyName)
        .Map(D, winLoss => winLoss.Turnover)
        .Map(E, winLoss => winLoss.GrossComm)
        .Map(F, winLoss => winLoss.MemberWinLoss)
        .Map(G, winLoss => winLoss.MemberComm)
        .Map(H, winLoss => winLoss.MemberTotal)
        .Map(I, winLoss => winLoss.OwnerTurnover)
        .Map(J, winLoss => winLoss.OwnerWinLoss)
        .Map(K, winLoss => winLoss.OwnerComm)
        .Map(L, winLoss => winLoss.OwnerTotal)
        .Map(M, winLoss => winLoss.Company)
        .Map(N, winLoss => winLoss.BetCount)
        .Map(O, winLoss => winLoss.RewardAmount)
        .Map(P, winLoss => winLoss.RewardCurrency);
    private readonly Mapper<ConsolidationEntity> groupByPTMapper = new Mapper<ConsolidationEntity>()
        .Map(A, winLoss => string.Empty)
        .Map(B, winLoss => winLoss.PositionTakingText)
        .Map(C, winLoss => string.Empty)
        .Map(D, winLoss => winLoss.Turnover)
        .Map(E, winLoss => winLoss.GrossComm)
        .Map(F, winLoss => winLoss.MemberWinLoss)
        .Map(G, winLoss => winLoss.MemberComm)
        .Map(H, winLoss => winLoss.MemberTotal)
        .Map(I, winLoss => winLoss.OwnerTurnover)
        .Map(J, winLoss => winLoss.OwnerWinLoss)
        .Map(K, winLoss => winLoss.OwnerComm)
        .Map(L, winLoss => winLoss.OwnerTotal)
        .Map(M, winLoss => winLoss.Company)
        .Map(N, winLoss => winLoss.BetCount)
        .Map(O, winLoss => string.Empty)
        .Map(P, winLoss => string.Empty);
    private readonly Mapper<ConsolidationEntity> dataMapper = new Mapper<ConsolidationEntity>()
        .Map(A, winLoss => string.Empty)
        .Map(B, winLoss => $"{PaddingToProductName}{winLoss.ProductName}")
        .Map(C, winLoss => string.Empty)
        .Map(D, winLoss => winLoss.Turnover)
        .Map(E, winLoss => winLoss.GrossComm)
        .Map(F, winLoss => winLoss.MemberWinLoss)
        .Map(G, winLoss => winLoss.MemberComm)
        .Map(H, winLoss => winLoss.MemberTotal)
        .Map(I, winLoss => winLoss.OwnerTurnover)
        .Map(J, winLoss => winLoss.OwnerWinLoss)
        .Map(K, winLoss => winLoss.OwnerComm)
        .Map(L, winLoss => winLoss.OwnerTotal)
        .Map(M, winLoss => winLoss.Company)
        .Map(N, winLoss => winLoss.BetCount)
        .Map(O, winLoss => string.Empty)
        .Map(P, winLoss => string.Empty);

    public ExcelFileViewModel BuildExcelFile(
        ConsolidationExcelQuery message,
        IEnumerable<ConsolidationByUsername> reportData,
        string productNames,
        IEnumerable<ConsolidationCurrency> currencies)
    {
        var fromDate = message.FromDate.ToShortDateWithSlash();
        var toDate = message.ToDate.ToShortDateWithSlash();
        var currencyName = message.UserSelectedCurrencyId == 0
            ? Resources.AllCurrencies
            : currencies.FirstOrDefault(c => c.Currency == message.UserSelectedCurrencyId)?.CurrencyName ?? string.Empty;

        this.reportData = reportData;

        var fileContent = UseTemplate(
            new TemplateInfo(
                GetExcelTemplatePath("ConsolidationByUsername"),
                noInfomationText: Resources.NoInformationIsAvailable,
                freezeRowIndex: 4,
                freezeColumnName: C))
            .Title($"{Resources.ConsolidationReportByUsername} {fromDate} --> {toDate} {Resources.of2} {productNames} - {currencyName}")
            .Header(Resources.ResourceManager.GetString)
            .Sheet(Resources.DefaultSheetName)
            .ExportToBytes();

        return new ExcelFileViewModel($"{Resources.Consolidation}-{fromDate}-{toDate}", fileContent);
    }

    protected override Sheet LoadTemplateSheet()
    {
        var sheet = GetSheetAt(TemplateInfo.SheetIndex);
        var currentRowIndex = TemplateInfo.BodyStartIndex;

        groupByUsernameRowTemplate = sheet.GetRow(currentRowIndex++);
        groupByPTRowTemplate = sheet.GetRow(currentRowIndex++);
        dataRowTemplate = sheet.GetRow(currentRowIndex);
        sheet.OddRowTemplate = dataRowTemplate;

        return sheet;
    }

    protected override void RemoveTemplateRows(Sheet sheet)
    {
        sheet.RemoveRow(groupByUsernameRowTemplate);
        sheet.RemoveRow(groupByPTRowTemplate);
        sheet.RemoveRow(dataRowTemplate);
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
        }
        else
        {
            FillNoInfoRow(sheet);
        }
    }

    protected override void FillBodyRows(Sheet sheet)
    {
        Row row;
        var currentRowIndex = TemplateInfo.BodyStartIndex;

        foreach (var dataByUsername in reportData)
        {
            row = sheet.GetRow(currentRowIndex++);
            row.CopyCellsFrom(groupByUsernameRowTemplate);
            FillCells(row, dataByUsername.Total, groupByUsernameMapper.Mappings);

            foreach (var dataByPT in dataByUsername.ConsolidationByPTs)
            {
                if (IsExcludeFromExcel(dataByPT.Total))
                {
                    continue;
                }
                row = sheet.GetRow(currentRowIndex++);
                row.CopyCellsFrom(groupByPTRowTemplate);
                FillCells(row, dataByPT.Total, groupByPTMapper.Mappings);

                foreach (var data in dataByPT.WinLoss)
                {
                    if (IsExcludeFromExcel(data))
                    {
                        continue;
                    }
                    row = sheet.GetRow(currentRowIndex++);
                    row.CopyCellsFrom(dataRowTemplate);
                    FillCells(row, data, dataMapper.Mappings);
                }
            }
        }
    }
}
