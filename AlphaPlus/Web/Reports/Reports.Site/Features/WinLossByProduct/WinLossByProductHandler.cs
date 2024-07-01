using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WinLossByProduct;
using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Reports.Site.Features.WinLossByProduct;

public class WinLossByProductHandler(
    IProductService productService,
    IOptions<AppSettings> options,
    IWinLossByProductService winLossByProductService,
    IWinLossByProductWorkbook workbook) :
        BaseHandler(productService, options),
        IRequestHandler<WinLossByProductQuery, WinLossByProductViewModel>,
        IRequestHandler<WinLossByProductExcelQuery, ExcelFileViewModel>
{
    public async Task<WinLossByProductViewModel> Handle(WinLossByProductQuery message, CancellationToken cancellationToken)
    {
        var (productViewModel, productDetailXml) = BuildProductViewModel(message, message.UserSelectedProductIds, message.UserSelectedResourceIds);
        var (reportData, total) = await GetWinLossByProduct(message, productViewModel.UserSelectedProductIds, productDetailXml);

        return new WinLossByProductViewModel(message, productViewModel, reportData, total);
    }

    public async Task<ExcelFileViewModel> Handle(WinLossByProductExcelQuery message, CancellationToken cancellationToken)
    {
        var (validUserSelectedProductIds, productDetailXml) = FilterProductAndProductDetailIds(
            message,
            message.UserSelectedProductIds,
            message.UserSelectedResourceIds);
        var (reportData, total) = await GetWinLossByProduct(message, validUserSelectedProductIds.JoinWithComma(), productDetailXml);
        var productNames = productService.ConcatProductNames(validUserSelectedProductIds);

        return workbook.BuildExcelFile(message, reportData, total, productNames);
    }

    private Task<(IEnumerable<WinLossByProductModel>, WinLossByBetTypeModel)> GetWinLossByProduct(
        WinLossByProductQuery message,
        string userSelectedProductIds,
        string productDetailXml)
    {
        var param = new WinLossByProductParam(
            message.ViewerId,
            message.ViewerLevel,
            message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
            message.ToDate,
            userSelectedProductIds,
            productDetailXml,
            message.Site);

        return winLossByProductService.GetWinLossByProduct(param);
    }
}