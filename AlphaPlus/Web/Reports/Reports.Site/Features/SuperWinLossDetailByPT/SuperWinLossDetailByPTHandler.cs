using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.Products;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.SuperWinLossDetailByPT;
using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Reports.Site.Features.SuperWinLossDetailByPT;

public class SuperWinLossDetailByPTHandler(
    IProductService productService,
    IOptions<AppSettings> options,
    IBrowserHelper browserHelper,
    ISuperWinLossDetailByPTService superWinLossDetailByPTService,
    ISuperWinLossDetailByWorkbook workbook) :
        BaseHandler(productService, options),
        IRequestHandler<SuperWinLossDetailByPTQuery, SuperWinLossDetailByPTViewModel>,
        IRequestHandler<SuperWinLossDetailByPTExcelQuery, ExcelFileViewModel>
{
    private readonly IBrowserHelper browserHelper = browserHelper;
    private readonly ISuperWinLossDetailByPTService superWinLossDetailByPTService = superWinLossDetailByPTService;
    private readonly ISuperWinLossDetailByWorkbook workbook = workbook;

    public async Task<SuperWinLossDetailByPTViewModel> Handle(SuperWinLossDetailByPTQuery message, CancellationToken cancellationToken)
    {
        var products = await GetProducts(message);
        var (productViewModel, productDetailXml) = await BuildProductViewModel(message, message.UserSelectedProductIds, message.UserSelectedResourceIds, products);
        var (reportData, total, ptList) = await GetSuperWinLossDetailByPT(message, productViewModel.UserSelectedProductIds, productDetailXml);
        var isOnlyMobile = browserHelper.IsMobile(isOnlyTablet: false) && !browserHelper.IsMobile(isOnlyTablet: true);

        return new SuperWinLossDetailByPTViewModel(
            message,
            productViewModel,
            reportData,
            total,
            BuildSuperPTProductViewModels(products, ptList).ToList(),
            isOnlyMobile);
    }

    public async Task<ExcelFileViewModel> Handle(SuperWinLossDetailByPTExcelQuery message, CancellationToken cancellationToken)
    {
        var (validUserSelectedProductIds, productDetailXml, productNames) = await FilterProductAndProductDetailIdsForExcel(
            message,
            message.UserSelectedProductIds,
            message.UserSelectedResourceIds);
        var (reportData, total, _) = await GetSuperWinLossDetailByPT(message, validUserSelectedProductIds.JoinWithComma(), productDetailXml);

        return workbook.BuildExcelFile(message, reportData, total, productNames);
    }

    private Task<(IEnumerable<SuperWinLossDetailByPTModel>, SuperWinLossDetailByPTEntity, IEnumerable<PositionTakingModel>)> GetSuperWinLossDetailByPT(
        SuperWinLossDetailByPTQuery message,
        string userSelectedProductIds,
        string productDetailXml)
    {
        var param = new SuperWinLossDetailByPTParam(
            message.ViewerId,
            message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
            message.ToDate,
            userSelectedProductIds,
            productDetailXml,
            appSettings.SettingStatus,
            message.Site);

        return superWinLossDetailByPTService.GetSuperWinLossDetailByPT(param);
    }

    private IEnumerable<SuperPTProductViewModel> BuildSuperPTProductViewModels(IEnumerable<Product> products, IEnumerable<PositionTakingModel> ptList)
        => products
            .Join(
                ptList,
                product => product.Id,
                pt => pt.ProductId,
                (product, pt) => new { pt.ValuePT, product.Name })
            .GroupBy(
                ptProduct => ptProduct.ValuePT,
                (pt, ptProducts) => new SuperPTProductViewModel(pt, ptProducts.Select(product => product.Name)))
            .OrderBy(vm => vm.PT);
}