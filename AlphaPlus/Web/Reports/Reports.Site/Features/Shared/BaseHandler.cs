using System.Text;
using AlphaPlus.Reports.Features.Shared;
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
        string userSelectedResourceIds,
        IEnumerable<Product> products = null)
    {
        products = products ?? GetProducts(message);
        var (productGroups, validUserSelectedProductIds) = BuildProductGroups(products, userSelectedProductIds);
        var (productDetails, validUserSelectedResourceIds, unselectedProductIds, productDetailXml) = BuildProductDetailGroups(
            message.ViewerId,
            products,
            validUserSelectedProductIds,
            userSelectedResourceIds);
        if (unselectedProductIds.Any())
        {
            validUserSelectedProductIds = validUserSelectedProductIds.Except(unselectedProductIds);
        }

        return (new ProductMultiSelectionViewModel(
            productGroups,
            validUserSelectedProductIds.JoinWithComma(),
            message.Language,
            productDetails,
            validUserSelectedResourceIds.JoinWithComma()), productDetailXml);
    }

    protected ProductMultiSelectionViewModel BuildProductViewModel(
        BaseQuery message,
        string userSelectedProductIds,
        IEnumerable<Product> products = null)
    {
        products = products ?? GetProducts(message);
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
        int custId,
        IEnumerable<Product> products,
        IEnumerable<int> validUserSelectedProductIds,
        string userSelectedResourceIds)
    {
        var isCheckedAll = "*" == userSelectedResourceIds;
        var userSelectedResourceIdsList = isCheckedAll ? [] : userSelectedResourceIds.ToListInt().ToList();

        var resourceIdHashSet = new HashSet<int>();
        var productDetails = new List<ProductDetailDTO>();
        var validUserSelectedResourceIds = new List<int>();
        var allProductsSupportProductDetails = new List<int>();
        var selectedProductIds = new List<int>();

        var xml = new StringBuilder();
        var allProductDetails = productService.GetProductDetails(custId, products);
        foreach (var item in allProductDetails)
        {
            var isChecked = isCheckedAll || (validUserSelectedProductIds.Contains(item.ProductId) && userSelectedResourceIdsList.Contains(item.ResourceId));
            if (isChecked)
            {
                validUserSelectedResourceIds.Add(item.ResourceId);
                selectedProductIds.Add(item.ProductId);
                xml.Append(productService.BuildProductDetailXml(item));
            }

            if (resourceIdHashSet.Add(item.ResourceId))
            {
                productDetails.Add(new ProductDetailDTO(item.ProductId, item.ResourceId, item.DisplayName, isChecked));
            }

            allProductsSupportProductDetails.Add(item.ProductId);
        }

        // Get un-selected Product Ids
        var unselectedProductIds = allProductsSupportProductDetails.Except(selectedProductIds);

        return (productDetails, validUserSelectedResourceIds, unselectedProductIds.ToList(), $"<ROOT>{xml}</ROOT>");
    }

    protected (IEnumerable<int>, string) FilterProductAndProductDetailIds(
        BaseQuery message,
        string userSelectedProductIds,
        string userSelectedResourceIds,
        IEnumerable<Product> products = null)
    {
        products = FilterProducts(products ?? GetProducts(message), userSelectedProductIds).ToList();
        var (unselectedProductIds, productDetailXml) = FilterProductDetailIds(message.ViewerId, products, userSelectedProductIds, userSelectedResourceIds);
        var productIds = products.Select(product => product.Id);

        if (unselectedProductIds.Any())
        {
            productIds = productIds.Except(unselectedProductIds);
        }

        return (productIds.ToList(), productDetailXml);
    }

    protected IEnumerable<int> FilterProductIds(BaseQuery message, string userSelectedProductIds, IEnumerable<int> offeredProductIds = null)
    {
        var products = GetProducts(message);

        if (offeredProductIds != null)
        {
            products = products.Where(product => offeredProductIds.Contains(product.Id));
        }

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
        int custId,
        IEnumerable<Product> products,
        string userSelectedProductIds,
        string userSelectedResourceIds)
    {
        var userSelectedProductIdsList = Enumerable.Empty<int>();
        var userSelectedResourceIdsList = Enumerable.Empty<int>();
        var isCheckedAll = "*" == userSelectedProductIds && "*" == userSelectedResourceIds;
        if (!isCheckedAll)
        {
            userSelectedProductIdsList = userSelectedProductIds.ToListInt().ToList();
            userSelectedResourceIdsList = userSelectedResourceIds.ToListInt().ToList();
        }

        var allProductsSupportProductDetails = new List<int>();
        var selectedProductIds = new List<int>();

        var xml = new StringBuilder();
        var productDetails = productService.GetProductDetails(custId, products);
        foreach (var item in productDetails)
        {
            var isChecked = isCheckedAll || (userSelectedProductIdsList.Contains(item.ProductId) && userSelectedResourceIdsList.Contains(item.ResourceId));
            if (isChecked)
            {
                selectedProductIds.Add(item.ProductId);
                xml.Append(productService.BuildProductDetailXml(item));
            }

            allProductsSupportProductDetails.Add(item.ProductId);
        }

        // Get un-selected Product Ids
        var unselectedProductIds = allProductsSupportProductDetails.Except(selectedProductIds);

        return (unselectedProductIds.ToList(), $"<ROOT>{xml}</ROOT>");
    }

    protected IEnumerable<Product> GetProducts(BaseQuery message)
    {
        var param = new GetProductsParam(message.Site, message.CurrencyId, appSettings.SettingStatus, message.Language, message.MinDate);

        return productService.GetProducts(param);
    }

    protected virtual BreadcrumbViewModel BuildBreadcrumbViewModel(BaseQuery message)
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
