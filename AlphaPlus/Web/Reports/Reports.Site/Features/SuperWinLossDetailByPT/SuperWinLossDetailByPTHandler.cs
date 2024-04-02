using AlphaPlus.Core.Helpers;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.SuperWinLossDetailByPT;
using AlphaPlus.Reports.Site.Features.Shared;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
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
    public async Task<SuperWinLossDetailByPTViewModel> Handle(SuperWinLossDetailByPTQuery message, CancellationToken cancellationToken)
    {
        var (productViewModel, productDetailXml) = BuildProductViewModel(message, message.UserSelectedProductIds, message.UserSelectedResourceIds);
        var (reportData, total, ptList) = await GetSuperWinLossDetailByPT(message, productViewModel.UserSelectedProductIds, productDetailXml);
        var isOnlyMobile = browserHelper.IsMobile(isOnlyTablet: false) && !browserHelper.IsMobile(isOnlyTablet: true);

        return new SuperWinLossDetailByPTViewModel(
            message,
            productViewModel,
            reportData,
            total,
            BuildSuperPTProductViewModels(message, ptList).ToList(),
            isOnlyMobile);
    }

    public async Task<ExcelFileViewModel> Handle(SuperWinLossDetailByPTExcelQuery message, CancellationToken cancellationToken)
    {
        var (validUserSelectedProductIds, productDetailXml) = FilterProductAndProductDetailIds(
            message,
            message.UserSelectedProductIds,
            message.UserSelectedResourceIds);
        var (reportData, total, _) = await GetSuperWinLossDetailByPT(message, validUserSelectedProductIds.JoinWithComma(), productDetailXml);
        var productNames = productService.ConcatProductNames(validUserSelectedProductIds);

        return workbook.BuildExcelFile(message, reportData, total, productNames);
    }

    private Task<(IEnumerable<SuperWinLossDetailByPTModel>, SuperWinLossDetailByPTEntity, IEnumerable<SuperPTModel>)> GetSuperWinLossDetailByPT(
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

    private IEnumerable<SuperPTProductViewModel> BuildSuperPTProductViewModels(SuperWinLossDetailByPTQuery message, IEnumerable<SuperPTModel> ptList)
        => GetProducts(message)
            .Join(
                ptList,
                product => product.Id,
                pt => pt.ProductId,
                (product, pt) => new { pt.SuperPT, product.Name })
            .GroupBy(
                ptProduct => ptProduct.SuperPT,
                (key, ptProducts) => new SuperPTProductViewModel(key, ptProducts.Select(product => product.Name).JoinWithComma(", ")))
            .OrderBy(vm => vm.PositionTaking);
}