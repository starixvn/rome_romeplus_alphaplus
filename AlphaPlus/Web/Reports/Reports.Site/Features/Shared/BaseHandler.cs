using System.Text;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared.Helpers;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Reports.Site.Features.Shared;

public class BaseHandler(IProductService productService, IOptions<AppSettings> options)
{
    protected readonly IProductService productService = productService;
    protected readonly AppSettings appSettings = options.Value;

    protected (ProductMultiSelectionViewModel, string) BuildProductViewModel(
        BaseQuery message,
        string userSelectedProductIds,
        string userSelectedResourceIds)
    {
        var allProducts = GetProducts(message);

        var (productDetails, validUserSelectedResourceIds, unselectedProductIds, productDetailXml) = BuildProductDetailViewModel(
            message.ViewerId,
            allProducts,
            userSelectedResourceIds);

        if ("*" != userSelectedProductIds && unselectedProductIds.Any())
        {
            userSelectedProductIds = userSelectedProductIds.ToListInt().Except(unselectedProductIds).JoinWithComma();
        }

        var (products, validUserSelectedProductIds) = BuildProductViewModel(allProducts, userSelectedProductIds);

        return (new ProductMultiSelectionViewModel(
            products,
            validUserSelectedProductIds.JoinWithComma(),
            message.Language,
            productDetails,
            validUserSelectedResourceIds.JoinWithComma()), productDetailXml);
    }

    protected ProductMultiSelectionViewModel BuildProductViewModel(
        BaseQuery message,
        string userSelectedProductIds,
        IEnumerable<Product> offeredProducts = null)
    {
        offeredProducts = offeredProducts ?? GetProducts(message);
        var (products, validUserSelectedProductIds) = BuildProductViewModel(offeredProducts, userSelectedProductIds);

        return new ProductMultiSelectionViewModel(products, validUserSelectedProductIds.JoinWithComma(), message.Language);
    }

    private static (IEnumerable<ProductGroupDTO>, IEnumerable<int>) BuildProductViewModel(
        IEnumerable<Product> offeredProducts,
        string userSelectedProductIds)
    {
        var productGroups = new List<ProductGroupDTO>();
        var validUserSelectedProductIds = new List<int>();

        var isCheckedAll = "*" == userSelectedProductIds;
        var userSelectedProductIdsList = isCheckedAll ? [] : userSelectedProductIds.ToListInt();

        var offeredProductGroups = offeredProducts.GroupBy(p => new { p.Type, p.TypeName });
        foreach (var groups in offeredProductGroups)
        {
            var subProducts = groups.Select(item =>
            {
                var isChecked = isCheckedAll || userSelectedProductIdsList.Contains(item.Id);
                if (isChecked)
                {
                    validUserSelectedProductIds.Add(item.Id);
                }

                return new ProductGroupDTO(item.Id, item.Name, isChecked, null);
            });

            if (groups.Key.Type == 0)
            {
                productGroups.AddRange(subProducts);
            }
            else
            {
                productGroups.Add(new ProductGroupDTO(groups.Key.Type, groups.Key.TypeName, false, subProducts.ToList()));
            }
        }

        return (productGroups, validUserSelectedProductIds);
    }

    private (IEnumerable<ProductDetailDTO>, IEnumerable<int>, IEnumerable<int>, string) BuildProductDetailViewModel(
        int custId,
        IEnumerable<Product> products,
        string userSelectedResourceIds,
        bool isExportExcel = false)
    {
        var allProductDetails = productService.GetProductDetails(custId, products);
        var isCheckedAll = "*" == userSelectedResourceIds;
        var userSelectedResourceIdsList = isCheckedAll ? [] : userSelectedResourceIds.ToListInt();

        var productDetails = new List<ProductDetailDTO>();
        var validUserSelectedResourceIds = new List<int>();
        var allProductsSupportProductDetails = new HashSet<int>();
        var selectedProductIds = new HashSet<int>();

        var xml = new StringBuilder();

        foreach (var item in allProductDetails)
        {
            var isChecked = isCheckedAll || userSelectedResourceIdsList.Contains(item.ResourceId);
            if (isChecked)
            {
                validUserSelectedResourceIds.Add(item.ResourceId);
                selectedProductIds.Add(item.ProductId);
                xml.Append(productService.BuildProductDetailXml(item));
            }

            // If export excel --> no need to build ProductDetailDTO
            if (!isExportExcel)
            {
                productDetails.Add(new ProductDetailDTO(item.ProductId, item.ResourceId, item.DisplayName, isChecked));
            }

            allProductsSupportProductDetails.Add(item.ProductId);
        }

        var unselectedProductIds = allProductsSupportProductDetails.Except(selectedProductIds);

        return (productDetails, validUserSelectedResourceIds, unselectedProductIds, $"<ROOT>{xml}</ROOT>");
    }

    protected (IEnumerable<int>, string) FilterProductAndProductDetailIds(BaseQuery message, string userSelectedProductIds, string userSelectedResourceIds)
    {
        var products = FilterProducts(message, userSelectedProductIds).ToList();
        var (unselectedProductIds, productDetailXml) = FilterProductDetailIds(message.ViewerId, products, userSelectedResourceIds);

        // Remove invalid ProductIds
        var productIds = products.Select(product => product.Id).Except(unselectedProductIds).ToList();

        return (productIds, productDetailXml);
    }

    protected IEnumerable<int> FilterProductIds(BaseQuery message, string userSelectedProductIds, IEnumerable<int> offeredProductIds = null)
        => FilterProducts(message, userSelectedProductIds, offeredProductIds).Select(product => product.Id).ToList();

    private IEnumerable<Product> FilterProducts(BaseQuery message, string userSelectedProductIds, IEnumerable<int> offeredProductIds = null)
    {
        var allProducts = GetProducts(message);
        if (offeredProductIds != null)
        {
            allProducts = allProducts.Where(product => offeredProductIds.Contains(product.Id));
        }

        if ("*" != userSelectedProductIds)
        {
            var userSelectedProductIdsList = userSelectedProductIds.ToListInt();
            allProducts = allProducts.Where(product => userSelectedProductIdsList.Contains(product.Id));
        }

        return allProducts;
    }

    private (IEnumerable<int>, string) FilterProductDetailIds(
        int custId,
        IEnumerable<Product> products,
        string userSelectedResourceIds)
    {
        var productDetails = productService.GetProductDetails(custId, products);
        var isCheckedAll = "*" == userSelectedResourceIds;
        var userSelectedResourceIdsList = isCheckedAll ? [] : userSelectedResourceIds.ToListInt();

        var productsSupportProductDetails = new HashSet<int>();
        var userSelectedProductIds = new HashSet<int>();

        var xml = new StringBuilder();

        foreach (var item in productDetails)
        {
            var isChecked = isCheckedAll || userSelectedResourceIdsList.Contains(item.ResourceId);
            if (isChecked)
            {
                userSelectedProductIds.Add(item.ProductId);
                xml.Append(productService.BuildProductDetailXml(item));
            }

            productsSupportProductDetails.Add(item.ProductId);
        }

        var unselectedProductIds = productsSupportProductDetails.Except(userSelectedProductIds);

        return (unselectedProductIds, $"<ROOT>{xml}</ROOT>");
    }

    protected IEnumerable<Product> GetProducts(BaseQuery message)
    {
        var param = new GetProductsParam(message.Site, message.CurrencyId, appSettings.SettingStatus, message.Language, message.MinDate);

        return productService.GetProducts(param);
    }

    protected static BreadcrumbViewModel BuildBreadcrumbViewModel(BaseQuery message)
    {
        var breadcrumbs = new List<Breadcrumb>();

        if (JsonHelper.TryParse<Breadcrumb[]>(message.BreadcrumbsJson, out var breadcrumbList))
        {
            foreach (var item in breadcrumbList)
            {
                if (item.CustLevelId == message.CustLevelId)
                {
                    break;
                }

                breadcrumbs.Add(item);
            }
        }

        breadcrumbs.Add(new Breadcrumb(message.CustId, message.CustName, message.CustLevelId));

        return new BreadcrumbViewModel(breadcrumbs, JsonHelper.ToJson(breadcrumbs));
    }
}
