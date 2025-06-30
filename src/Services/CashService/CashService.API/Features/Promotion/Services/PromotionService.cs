namespace AlphaPlus.CashService.API.Features.Promotion.Services;

using Criteria;
using Fanex.Data.Repository;
using Models;
using Shared.Models;

public interface IPromotionService
{
    Task<IEnumerable<Product>> GetProductAsync();

    Task<IEnumerable<PromotionModel>> GetPromotionAsync(int promotionTypeId, int statusId, int custId);

    Task<PromotionModel> GetPromotionByIdAsync(int promotionId, int custId);

    Task<ResultModel> AddPromotionAsync(PromotionRequest request);

    Task<ResultModel> EditPromotion(int promotionId, PromotionRequest request);
}

public class PromotionService(IDynamicRepository repository) : IPromotionService
{
    public async Task<IEnumerable<Product>> GetProductAsync()
    {
        var products = FakeProducts();
        var treeProducts = MapProductsToTree(products);
        return await Task.FromResult(treeProducts);
    }

    public async Task<IEnumerable<PromotionModel>> GetPromotionAsync(int promotionTypeId, int statusId, int custId)
    {
        var result = await Task.FromResult(CreateFakePromotionModel());
        return result;
    }

    public async Task<PromotionModel> GetPromotionByIdAsync(int promotionId, int custId)
    {
        var promotion = CreateFakePromotionModel().FirstOrDefault(p => p.PromotionId == promotionId);
        return await Task.FromResult(promotion);
    }

    public async Task<ResultModel> AddPromotionAsync(PromotionRequest addPromotionMdoel)
    {
        var addPromotionCriteria = new AddPromotionCriteria
        {
            PromotionType = addPromotionMdoel.PromotionTypeId,
            StatusId = addPromotionMdoel.StatusId,
            FromDate = addPromotionMdoel.FromDate,
            ToDate = addPromotionMdoel.ToDate,
            Title = addPromotionMdoel.Title,
            Content = addPromotionMdoel.Content,
            Banner = addPromotionMdoel.Banner,
            ProductSettings = addPromotionMdoel.ProductSettings != null
                ? System.Text.Json.JsonSerializer.Serialize(addPromotionMdoel.ProductSettings)
                : string.Empty,
            CreatedBy = addPromotionMdoel.UserId,
            IP = addPromotionMdoel.IP,
            SiteDbGroupId = addPromotionMdoel.SiteDBGroupId,
        };

        await repository.ExecuteAsync(addPromotionCriteria);

        var result = new ResultModel(addPromotionCriteria.ErrCode, addPromotionCriteria.ErrMsg);

        return result;
    }

    public async Task<ResultModel> EditPromotion(int promotionId, PromotionRequest addPromotionMdoel)
    {
        var editPromotionCriteria = new EditPromotionCriteria
        {
            PromotionId = promotionId,
            PromotionType = addPromotionMdoel.PromotionTypeId,
            StatusId = addPromotionMdoel.StatusId,
            FromDate = addPromotionMdoel.FromDate,
            ToDate = addPromotionMdoel.ToDate,
            Title = addPromotionMdoel.Title,
            Content = addPromotionMdoel.Content,
            Banner = addPromotionMdoel.Banner,
            ProductSettings = addPromotionMdoel.ProductSettings != null
                ? System.Text.Json.JsonSerializer.Serialize(addPromotionMdoel.ProductSettings)
                : string.Empty,
            CreatedBy = addPromotionMdoel.UserId,
            IP = addPromotionMdoel.IP,
            SiteDbGroupId = addPromotionMdoel.SiteDBGroupId,
        };

        await repository.ExecuteAsync(editPromotionCriteria);

        var result = new ResultModel(editPromotionCriteria.ErrCode, editPromotionCriteria.ErrMsg);

        return result;
    }

    private static List<Product> MapProductsToTree(List<Product> products)
    {
        var productDict = products.ToDictionary(p => p.ProductId);
        var tree = new List<Product>();
        foreach (var product in products)
        {
            product.ChildProducts = [];
        }
        var childrenDict = new Dictionary<int, List<Product>>();
        foreach (var product in products)
        {
            if (product.ParentId == 0)
            {
                tree.Add(product);
            }
            else if (productDict.TryGetValue(product.ParentId, out var parent))
            {
                if (!childrenDict.ContainsKey(parent.ProductId))
                    childrenDict[parent.ProductId] = [];
                childrenDict[parent.ProductId].Add(product);
            }
        }
        foreach (var kvp in childrenDict)
        {
            productDict[kvp.Key].ChildProducts = kvp.Value.ToArray();
        }
        return tree;
    }

    private static List<PromotionModel> CreateFakePromotionModel()
    {
        var fakeList = new List<PromotionModel>();
        for (var i = 1; i <= 10; i++)
        {
            fakeList.Add(new PromotionModel
            {
                PromotionId = i,
                PromotionTitle = $"Promotion {i}",
                PromotionTypeId = i % 3 + 1,
                PromotionType = i % 3 + 1,
                FromDate = DateTime.Now.AddDays(-i * 10),
                ToDate = DateTime.Now.AddDays(i * 10),
                Enabled = i % 2 == 0,
                StatusId = i % 4,
                LastUpdated = DateTime.Now.AddDays(-i),
                ProductSettings =
                [
                    new()
                    {
                        ProductId = 100 + i,
                        ProductName = $"Product {i}",
                        Percentage = 10 + i,
                        MinTurnover = 1000 + i * 100,
                        MaxPayout = 5000 + i * 200,
                        Unlimited = i % 2 == 1
                    },

                    new()
                    {
                        ProductId = 200 + i,
                        ProductName = $"Product {i + 10}",
                        Percentage = 15 + i,
                        MinTurnover = 2000 + i * 100,
                        MaxPayout = 6000 + i * 200,
                        Unlimited = i % 2 == 0
                    }
                ],
                Products =
                [
                    new()
                    {
                        ProductId = 100 + i,
                        ProductName = $"Product {i}",
                        ParentId = 0,
                        ChildProducts = []
                    },

                    new()
                    {
                        ProductId = 200 + i,
                        ProductName = $"Product {i + 10}",
                        ParentId = 0,
                        ChildProducts = []
                    }
                ]
            });
        }
        return fakeList;
    }

    private static List<Product> FakeProducts()
        =>
        [
            new() { ProductId = 1, ProductName = "Sportsbook", ParentId = 0 },
            new() { ProductId = 2, ProductName = "Number Game", ParentId = 0 },
            new() { ProductId = 3, ProductName = "Lottery", ParentId = 0 },
            new() { ProductId = 4, ProductName = "Sports Lottery", ParentId = 0 },
            new() { ProductId = 5, ProductName = "RNG Keno", ParentId = 0 },
            new() { ProductId = 6, ProductName = "Table Game", ParentId = 0 },
            new() { ProductId = 7, ProductName = "Virtual Sports", ParentId = 0 },
            new() { ProductId = 8, ProductName = "Bitcoin", ParentId = 0 },
            new() { ProductId = 9, ProductName = "Saba Promotion", ParentId = 0 },
            new() { ProductId = 10, ProductName = "Live Casino", ParentId = 0 },
            new() { ProductId = 11, ProductName = "FGG", ParentId = 10 },
            new() { ProductId = 12, ProductName = "Allbet", ParentId = 10 },
            new() { ProductId = 13, ProductName = "BBIN", ParentId = 10 },
            new() { ProductId = 14, ProductName = "SA Gaming", ParentId = 10 },
            new() { ProductId = 15, ProductName = "AE Sexy", ParentId = 10 },
            new() { ProductId = 16, ProductName = "ION", ParentId = 10 },
            new() { ProductId = 17, ProductName = "WM", ParentId = 10 },
            new() { ProductId = 18, ProductName = "BG", ParentId = 10 },
            new() { ProductId = 19, ProductName = "PP Live Casino", ParentId = 10 },
            new() { ProductId = 20, ProductName = "YeeBet", ParentId = 10 },
            new() { ProductId = 21, ProductName = "ON Casino", ParentId = 10 },
            new() { ProductId = 22, ProductName = "Microgaming Live", ParentId = 10 },
            new() { ProductId = 23, ProductName = "RNG Casino", ParentId = 0 },
            new() { ProductId = 24, ProductName = "Virtual Games", ParentId = 23 },
            new() { ProductId = 25, ProductName = "Vgaming", ParentId = 23 },
            new() { ProductId = 26, ProductName = "Macau Games", ParentId = 23 },
            new() { ProductId = 27, ProductName = "Saba Casino", ParentId = 23 },
            new() { ProductId = 28, ProductName = "Arcadia Gaming", ParentId = 23 },
            new() { ProductId = 29, ProductName = "SG", ParentId = 23 },
            new() { ProductId = 30, ProductName = "PP", ParentId = 23 },
            new() { ProductId = 31, ProductName = "PG Soft", ParentId = 23 },
            new() { ProductId = 32, ProductName = "Joker", ParentId = 23 },
            new() { ProductId = 33, ProductName = "MaxGame", ParentId = 23 },
            new() { ProductId = 34, ProductName = "Habanero", ParentId = 23 },
            new() { ProductId = 35, ProductName = "CG", ParentId = 23 },
            new() { ProductId = 36, ProductName = "AdvantPlay", ParentId = 23 },
            new() { ProductId = 37, ProductName = "AdvantPlay Mini", ParentId = 23 },
            new() { ProductId = 38, ProductName = "SEAL Entertainment", ParentId = 23 },
            new() { ProductId = 39, ProductName = "MG-RNG", ParentId = 23 },
            new() { ProductId = 40, ProductName = "Nextspin", ParentId = 23 },
            new() { ProductId = 41, ProductName = "SABA xD", ParentId = 23 },
            new() { ProductId = 42, ProductName = "GPI", ParentId = 23 },
            new() { ProductId = 43, ProductName = "PLAYSTAR", ParentId = 23 },
            new() { ProductId = 44, ProductName = "ASKMESLOT", ParentId = 23 },
            new() { ProductId = 45, ProductName = "Funky Games", ParentId = 23 },
            new() { ProductId = 46, ProductName = "Cash Service Billing", ParentId = 0 }
        ];
}
