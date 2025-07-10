namespace AlphaPlus.CashService.API.Features.Promotion.Services;

using Criteria;
using Fanex.Data.Repository;
using Models;
using Shared.Models;

public interface IPromotionService
{
    Task<IEnumerable<Product>> GetProductAsync();

    Task<IEnumerable<Promotion>> GetPromotionAsync(PromotionRequest request);

    Task<Promotion> GetPromotionByIdAsync(int promotionId, int custId);

    Task<ResultModel> AddPromotionAsync(PromotionRequest request);

    Task<ResultModel> EditPromotion(int promotionId, PromotionRequest request);
}

public class PromotionService(IDynamicRepository repository) : IPromotionService
{
    public async Task<IEnumerable<Product>> GetProductAsync()
    {
        var products = await repository.FetchAsync<Product>(new GetProductListCriteria());
        return products;
    }

    public async Task<IEnumerable<Promotion>> GetPromotionAsync(PromotionRequest request)
    {
        var criteria = new GetPromotionCriteria(request.PromotionTypeId, request.StatusId, request.SiteDBGroupId, request.ActorId);
        var model = await repository.FetchMultipleAsync<Promotion, PromotionProductSetting>(criteria);

        foreach (var item in model.FirstResultSet)
        {
            item.ProductSettings = model.SecondResultSet.Where(i => i.PromotionId == item.PromotionId).ToList();
        }

        return model.FirstResultSet;
    }

    public async Task<Promotion> GetPromotionByIdAsync(int promotionId, int custId)
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

    private static List<Promotion> CreateFakePromotionModel()
    {
        var fakeList = new List<Promotion>();
        for (var i = 1; i <= 10; i++)
        {
            fakeList.Add(new Promotion
            {
                PromotionId = i,
                PromotionTitle = $"Promotion Title {i}",
                SiteDbGroupId = i,
                PromotionTypeId = i % 3 + 1,
                FromDate = DateTime.Now.AddDays(-i * 10),
                ToDate = DateTime.Now.AddDays(i * 10),
                Enabled = i % 2 == 0,

                ProductSettings =
                [
                    new()
                    {
                        PromotionId = i,
                        SiteDbGroupId = i,
                        ProductCategoryId = i % 4 + 1,
                        Percentage = 10 + i,
                        MaxPayout = 5000 + i * 200,
                        ProductList = $"Product {i}, Product {i + 10}"
                    }
                ],
                LanguageSettings = new List<PromotionLanguageSetting>()
            });
        }
        return fakeList;
    }
}