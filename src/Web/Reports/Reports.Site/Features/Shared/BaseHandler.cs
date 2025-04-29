using System.Text;
using AlphaPlus.Reports.Features.Products;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Reports.Site.Features.Shared;

public class BaseHandler(IProductService productService, IOptions<AppSettings> options)
{
    protected readonly IProductService productService = productService;
    protected readonly AppSettings appSettings = options.Value;

    #region Build Product and Product Detail for dropdown list
    internal async Task<(ProductMultiSelectionViewModel, string)> BuildProductViewModel(
        BaseQuery message,
        string userSelectedProductIds,
        string userSelectedResourceIds,
        IEnumerable<Product> products = null) /* SuperWinLossDetailByPT need to pass products */
    {
        products = products ?? await GetProducts(message);
        var productDetails = await productService.GetProductDetails(message.ViewerId, products);
        var (productGroups, selectedProductIds) = BuildProductGroups(products, userSelectedProductIds);
        var (productDetailDTOList, validUserSelectedResourceIds, validUserSelectedProductIds, productDetailXml) = BuildProductDetailGroups(
            productDetails,
            selectedProductIds,
            userSelectedResourceIds);

        return (new ProductMultiSelectionViewModel(
            productGroups,
            validUserSelectedProductIds.JoinWithComma(),
            message.Language,
            productDetailDTOList,
            validUserSelectedResourceIds.JoinWithComma()), productDetailXml);
    }

    internal async Task<ProductMultiSelectionViewModel> BuildProductViewModel(
        BaseQuery message,
        string userSelectedProductIds)
    {
        var products = await GetProducts(message);
        var (productGroups, validUserSelectedProductIds) = BuildProductGroups(products, userSelectedProductIds);

        return new ProductMultiSelectionViewModel(productGroups, validUserSelectedProductIds.JoinWithComma(), message.Language);
    }

    private static (IEnumerable<ProductGroupDTO>, IEnumerable<int>) BuildProductGroups(
        IEnumerable<Product> products,
        string userSelectedProductIds)
    {
        var productGroups = new List<ProductGroupDTO>();
        var validUserSelectedProductIds = new List<int>();

        var isCheckedAll = "*" == userSelectedProductIds;
        var userSelectedProductIdsList = isCheckedAll ? [] : userSelectedProductIds.ToListInt().ToList();

        var offeredProductGroups = products.GroupBy(p => new { p.Type, p.TypeName });
        foreach (var groups in offeredProductGroups)
        {
            var subProductsCheckedCount = 0;
            var subProducts = groups.Select(item =>
            {
                var isChecked = isCheckedAll || userSelectedProductIdsList.Contains(item.Id);
                if (isChecked)
                {
                    subProductsCheckedCount++;
                    validUserSelectedProductIds.Add(item.Id);
                }

                return new ProductGroupDTO(item.Id, item.Name, isChecked, null);
            }).ToList();

            if (groups.Key.Type == 0)
            {
                productGroups.AddRange(subProducts);
            }
            else
            {
                productGroups.Add(new ProductGroupDTO(
                    groups.Key.Type,
                    groups.Key.TypeName,
                    isCheckedAll || subProductsCheckedCount == subProducts.Count,
                    subProducts));
            }
        }

        return (productGroups, validUserSelectedProductIds);
    }

    private (IEnumerable<ProductDetailDTO>, IEnumerable<int>, IEnumerable<int>, string) BuildProductDetailGroups(
        IEnumerable<ProductDetail> productDetails,
        IEnumerable<int> validUserSelectedProductIds,
        string userSelectedResourceIds)
    {
        var isCheckedAll = "*" == userSelectedResourceIds;
        var userSelectedResourceIdsList = isCheckedAll ? [] : userSelectedResourceIds.ToListInt().ToList();

        var resourceIdHashSet = new HashSet<int>();
        var productDetailDTOList = new List<ProductDetailDTO>();
        var validUserSelectedResourceIds = new HashSet<int>();
        var allProductsSupportProductDetails = new HashSet<int>();
        var selectedProductIds = new HashSet<int>();

        var xml = new StringBuilder();
        foreach (var item in productDetails)
        {
            var isChecked = isCheckedAll || (validUserSelectedProductIds.Contains(item.ProductId) && userSelectedResourceIdsList.Contains(item.ResourceId));
            if (isChecked)
            {
                validUserSelectedResourceIds.Add(item.ResourceId);
                selectedProductIds.Add(item.ProductId);
                xml.Append(item.ToXml());
            }

            if (resourceIdHashSet.Add(item.ResourceId))
            {
                productDetailDTOList.Add(new ProductDetailDTO(item.ProductId, item.ResourceId, item.DisplayName, isChecked));
            }

            allProductsSupportProductDetails.Add(item.ProductId);
        }

        // Get un-selected Product Ids
        var unselectedProductIds = allProductsSupportProductDetails.Except(selectedProductIds).ToList();
        if (unselectedProductIds.Any())
        {
            validUserSelectedProductIds = validUserSelectedProductIds.Except(unselectedProductIds);
        }

        return (productDetailDTOList, validUserSelectedResourceIds, validUserSelectedProductIds.ToList(), $"<ROOT>{xml}</ROOT>");
    }
    #endregion

    #region Filter Product and Product Detail for AJAX or Excel
    protected async Task<(IEnumerable<int>, string)> FilterProductAndProductDetailIds(
        BaseQuery message,
        string userSelectedProductIds,
        string userSelectedResourceIds)
    {
        var products = await GetProducts(message);
        var productDetails = await productService.GetProductDetails(message.ViewerId, products);

        var selectedProductIds = FilterProducts(products, userSelectedProductIds).Select(product => product.Id).ToList();
        var (validSelectedProductIds, productDetailXml) = FilterProductDetailIds(productDetails, selectedProductIds, userSelectedResourceIds);

        return (validSelectedProductIds, productDetailXml);
    }

    protected async Task<(IEnumerable<int>, string, string)> FilterProductAndProductDetailIdsForExcel(
        BaseQuery message,
        string userSelectedProductIds,
        string userSelectedResourceIds)
    {
        var products = await GetProducts(message);
        var productDetails = await productService.GetProductDetails(message.ViewerId, products);

        var selectedProductIds = FilterProducts(products, userSelectedProductIds).Select(product => product.Id).ToList();
        var (validSelectedProductIds, productDetailXml) = FilterProductDetailIds(productDetails, selectedProductIds, userSelectedResourceIds);

        var productNames = productService.GetProductNamesExportExcel(
            productDetails,
            validSelectedProductIds,
            userSelectedResourceIds.ToListInt());

        return (validSelectedProductIds, productDetailXml, productNames);
    }

    protected async Task<IEnumerable<int>> FilterProductIdsForExcel(BaseQuery message, string userSelectedProductIds)
    {
        var products = await GetProducts(message);

        return FilterProducts(products, userSelectedProductIds).Select(product => product.Id).ToList();
    }

    private IEnumerable<Product> FilterProducts(IEnumerable<Product> products, string userSelectedProductIds)
    {
        if ("*" != userSelectedProductIds)
        {
            var userSelectedProductIdsList = userSelectedProductIds.ToListInt().ToList();
            products = products.Where(product => userSelectedProductIdsList.Contains(product.Id));
        }

        return products;
    }

    private (IEnumerable<int>, string) FilterProductDetailIds(
        IEnumerable<ProductDetail> productDetails,
        IEnumerable<int> userSelectedProductIds,
        string userSelectedResourceIds)
    {
        var userSelectedResourceIdsList = Enumerable.Empty<int>();
        var isSelectAllResourceIds = "*" == userSelectedResourceIds;
        if (!isSelectAllResourceIds)
        {
            userSelectedResourceIdsList = userSelectedResourceIds.ToListInt().ToList();
        }

        var allProductsSupportProductDetails = new HashSet<int>();
        var selectedProductIds = new HashSet<int>();

        var xml = new StringBuilder();
        foreach (var item in productDetails)
        {
            var isChecked = userSelectedProductIds.Contains(item.ProductId) && (isSelectAllResourceIds || userSelectedResourceIdsList.Contains(item.ResourceId));
            if (isChecked)
            {
                selectedProductIds.Add(item.ProductId);
                xml.Append(item.ToXml());
            }

            allProductsSupportProductDetails.Add(item.ProductId);
        }

        // Get un-selected Product Ids
        var unselectedProductIds = allProductsSupportProductDetails.Except(selectedProductIds).ToList();
        if (unselectedProductIds.Any())
        {
            userSelectedProductIds = userSelectedProductIds.Except(unselectedProductIds).ToList();
        }

        return (userSelectedProductIds, $"<ROOT>{xml}</ROOT>");
    }
    #endregion

    // Need to override when report has it's own Product list
    protected virtual Task<IEnumerable<Product>> GetProducts(BaseQuery message)
    {
        var param = new GetProductsParam(message.Site, message.CurrencyId, appSettings.SettingStatus, message.Language, message.MinDate);

        return productService.GetProducts(param);
    }
}
