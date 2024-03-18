using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.ProgressiveContribution;
using AlphaPlus.Reports.Site.Features.Shared;
using ONEbook.SpreadSheets;
using static ONEbook.SpreadSheets.Column;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels;

namespace AlphaPlus.Reports.Site.Features.ProgressiveContribution;

public interface IProgressiveContributionWookbook
{
    ExcelFileViewModel BuildExcelFile(
        ProgressiveContributionExcelQuery message,
        IEnumerable<ProgressiveContributionModel> reportData,
        ProgressiveContributionEntity total,
        string productNames);
}

public class ProgressiveContributionWookbook(IWebHostEnvironment webHostEnvironment) :
    ReportNpoiWorkbook<ProgressiveContributionEntity>(webHostEnvironment),
    IProgressiveContributionWookbook
{
    private IEnumerable<ProgressiveContributionModel> reportData;
    private Mapper<ProgressiveContributionEntity> rowMapper;
    private Row productRowTemplate;
    private int currentRowIndex;

    public ExcelFileViewModel BuildExcelFile(
        ProgressiveContributionExcelQuery message,
        IEnumerable<ProgressiveContributionModel> reportData,
        ProgressiveContributionEntity total,
        string productNames)
    {
        this.reportData = reportData;
        rowMapper = GetRowMapper(message.ViewerLevel);

        var fromDate = message.FromDate.ToShortDateWithSlash();
        var toDate = message.ToDate.ToShortDateWithSlash();
        const string betTypeName = nameof(ProgressiveContributionEntity.BetTypeName);

        var fileContent = UseTemplate(new TemplateInfo(
                GetExcelTemplatePath(message.ViewerLevel.GetDirectDownlineLevel().DisplayName),
                noInfomationText: Resources.NoInformationIsAvailable,
                bodyEndIndex: 6))
            .Title(GetTitle(fromDate, toDate, productNames))
            .Header(key => key.Equals(betTypeName) ? $"{Resources.Product}/{Resources.BetType}" : Resources.ResourceManager.GetString(key))
            .Footer(total, rowMapper)
            .Sheet(Resources.DefaultSheetName)
            .ExportToBytes();

        return new ExcelFileViewModel($"{Resources.ProgressiveGamesContribution}-{message.CustName}-{fromDate}-{toDate}", fileContent);
    }

    private Mapper<ProgressiveContributionEntity> GetRowMapper(UserLevel custLevel)
    {
        var mapper = new Mapper<ProgressiveContributionEntity>()
            .Map(A, pc => pc.ProductId == 0 && pc.BetType == 0
                ? Resources.Total
                : pc.BetType)
            .Map(B, pc => pc.Turnover)
            .Map(C, pc => pc.Contribution);

        if (custLevel.IsAgentLevel())
        {
            mapper
                .Map(D, pc => pc.ContributionAgent)
                .Map(E, pc => pc.CompanyContribution);
        }
        else if (custLevel.IsMasterLevel())
        {
            mapper
                .Map(D, pc => pc.ContributionMaster)
                .Map(E, pc => pc.ContributionAgent)
                .Map(F, pc => pc.CompanyContribution);
        }
        else
        {
            mapper
                .Map(D, pc => pc.ContributionSuper)
                .Map(E, pc => pc.ContributionMaster)
                .Map(F, pc => pc.ContributionAgent)
                .Map(G, pc => pc.CompanyContribution);
        }

        return mapper;
    }

    private string GetTitle(string fromDate, string toDate, string productNames)
        => $"{Resources.ProgressiveGamesContribution} {fromDate} --> {toDate}" +
            $" {Resources.of2} {productNames}";

    protected override Sheet LoadTemplateSheet()
    {
        var sheet = GetSheetAt(TemplateInfo.SheetIndex);
        currentRowIndex = TemplateInfo.BodyStartIndex;

        productRowTemplate = sheet.GetRow(currentRowIndex++);
        sheet.OddRowTemplate = sheet.GetRow(currentRowIndex++);
        sheet.EvenRowTemplate = sheet.GetRow(currentRowIndex++);
        sheet.FooterRowsTemplate = [sheet.GetRow(currentRowIndex++)];

        return sheet;
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

    protected override void RemoveTemplateRows(Sheet sheet)
    {
        sheet.RemoveRow(productRowTemplate);
        base.RemoveTemplateRows(sheet);
    }

    protected override void FillBodyRows(Sheet sheet)
    {
        currentRowIndex = TemplateInfo.BodyStartIndex;

        foreach (var data in reportData)
        {
            FillProductTitle(sheet, currentRowIndex++, data);

            var isEvenRow = false;
            foreach (var progressiveContribution in data.ProgressiveContributions)
            {
                FillProgressiveContribution(sheet, currentRowIndex++, isEvenRow, progressiveContribution);
                isEvenRow = !isEvenRow;
            }
        }
    }

    private void FillProductTitle(Sheet sheet, int rowIndex, ProgressiveContributionModel data)
    {
        var row = sheet.GetRow(rowIndex);
        row.CopyCellsFrom(productRowTemplate);
        FillCells(row, data.Total, rowMapper.Mappings);
        row.SetCellValue(row.FirstCellIndex, data.ProductName.ToUpperInvariant());
    }

    private void FillProgressiveContribution(Sheet sheet, int rowIndex, bool isEvenRow, ProgressiveContributionEntity data)
    {
        var row = sheet.GetRow(rowIndex);
        var templateRow = isEvenRow ? sheet.EvenRowTemplate : sheet.OddRowTemplate;
        row.CopyCellsFrom(templateRow);
        FillCells(row, data, rowMapper.Mappings);
    }

    protected override void FillFooterRows(Sheet sheet)
    {
        var row = sheet.GetRow(currentRowIndex);
        row.CopyCellsFrom(sheet.FooterRowsTemplate.First());
        FillCells(row, SpreadSheet.FooterRows.First(), SpreadSheet.FooterRowMapper.Mappings);
    }
}
