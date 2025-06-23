using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.CashBillingService.Models;
using AlphaPlus.Reports.Site.Features.Shared;
using AlphaPlus.Reports.Site.Features.WinLossDetail.Requests;
using AlphaPlus.Shared.AppResources.Report;
using ONEbook.SpreadSheets;
using static ONEbook.SpreadSheets.Column;

namespace AlphaPlus.Reports.Site.Features.WinLossDetail;

public interface ICashFeeWorkbook
{
    ExcelFileViewModel BuildCashFeeExcelFile(CashFeeExcelQuery message, CashFeeModel cashFeeModel);
}

public class CashFeeWorkbook(IWebHostEnvironment webHostEnvironment) :
    ReportNpoiWorkbook<CashFeeItemModel>(webHostEnvironment),
    ICashFeeWorkbook
{
    private const int BodyStartRowIndex = 3;
    private CashFeeModel cashFeeModel;
    private Mapper<CashFeeItemModel> cashFeeItemMapper;
    private Row cashFeeTypeRowTemplate;
    private Row cashFeeItemRowTemplate;
    private int currentRowIndex;

    public ExcelFileViewModel BuildCashFeeExcelFile(CashFeeExcelQuery message, CashFeeModel cashFeeModel)
    {
        var fromDateString = (message.WeekFromDate ?? message.FromDate).ToShortDateWithSlash();
        var toDateString = (message.WeekToDate ?? message.ToDate).ToShortDateWithSlash();
        var custName = message.CustName;
        this.cashFeeModel = cashFeeModel;
        cashFeeItemMapper = GetRowMapper();

        var fileContent = UseTemplate(
            new TemplateInfo(
                GetExcelTemplatePath("CashFee"),
                noInfomationText: Resources.NoInformationIsAvailable,
                bodyStartIndex: BodyStartRowIndex))
            .Title($"{custName} --> {Resources.CashServiceBilling} {fromDateString} --> {toDateString}")
            .Header(Resources.ResourceManager.GetString)
            .Footer(cashFeeModel.GrandTotal, GetTotalRowMapper())
            .Sheet(Resources.DefaultSheetName)
            .ExportToBytes();

        return new ExcelFileViewModel($"{message.CustName}-CASH-SERVICE-BILLING-{fromDateString}-{toDateString}", fileContent);
    }

    private static Mapper<CashFeeItemModel> GetRowMapper()
    {
        var mapper = new Mapper<CashFeeItemModel>()
            .Map(A, cashFeeItem => cashFeeItem.DisplayType)
            .Map(B, cashFeeItem => cashFeeItem.Fee)
            .Map(C, cashFeeItem => cashFeeItem.Count)
            .Map(D, cashFeeItem => cashFeeItem.Agent)
            .Map(E, cashFeeItem => cashFeeItem.Company);

        return mapper;
    }

    private static Mapper<CashFeeItemModel> GetTotalRowMapper()
    {
        var mapper = new Mapper<CashFeeItemModel>()
            .Map(A, cashFeeItem => Resources.Total)
            .Map(D, cashFeeItem => cashFeeItem.Agent)
            .Map(E, cashFeeItem => cashFeeItem.Company);

        return mapper;
    }

    protected override Sheet LoadTemplateSheet()
    {
        var sheet = GetSheetAt(TemplateInfo.SheetIndex);
        var startRowIndex = TemplateInfo.BodyStartIndex;

        cashFeeTypeRowTemplate = sheet.GetRow(startRowIndex++);
        cashFeeItemRowTemplate = sheet.GetRow(startRowIndex++);
        sheet.FooterRowsTemplate = [sheet.GetRow(startRowIndex)];

        return sheet;
    }

    protected override void RemoveTemplateRows(Sheet sheet)
    {
        sheet.RemoveRow(cashFeeTypeRowTemplate);
        sheet.RemoveRow(cashFeeItemRowTemplate);

        foreach (Row item in sheet.FooterRowsTemplate)
        {
            sheet.RemoveRow(item);
        }
    }

    protected override void FillBody(Sheet sheet)
    {
        if (sheet == null)
        {
            throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
        }

        RemoveTemplateRows(sheet);

        if (cashFeeModel.FeeGroups.Any())
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

        foreach (var data in cashFeeModel.FeeGroups)
        {
            FillCashFeeTypeRow(sheet, currentRowIndex++, data.Total);

            foreach (var item in data.FeeItems)
            {
                FillCashFeeItemRow(sheet, currentRowIndex++, item);
            }
        }
    }

    private void FillCashFeeTypeRow(
        Sheet sheet,
        int rowIndex,
        CashFeeItemModel cashFeeItem)
    {
        var row = sheet.GetRow(rowIndex);
        var templateRow = cashFeeTypeRowTemplate;

        row.CopyCellsFrom(templateRow);
        FillCells(row, cashFeeItem, cashFeeItemMapper.Mappings);
    }

    private void FillCashFeeItemRow(
        Sheet sheet,
        int rowIndex,
        CashFeeItemModel cashFeeItem)
    {
        var row = sheet.GetRow(rowIndex);
        var templateRow = cashFeeItemRowTemplate;

        row.CopyCellsFrom(templateRow);
        FillCells(row, cashFeeItem, cashFeeItemMapper.Mappings);
    }

    protected override void FillFooterRows(Sheet sheet)
    {
        var row = sheet.GetRow(currentRowIndex);
        row.CopyCellsFrom(sheet.FooterRowsTemplate.First());
        FillCells(row, SpreadSheet.FooterRows.First(), SpreadSheet.FooterRowMapper.Mappings);
    }
}