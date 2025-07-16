namespace AlphaPlus.CashService.API.Features.Promotion.Services;

using AlphaPlus.CashService.API.Features.Shared.Services;
using Criteria;
using Fanex.Data.Repository;
using Models;
using Newtonsoft.Json;
using Shared.Models;
using Shared.Requests;

public interface IPromotionService
{
    Task<IEnumerable<Product>> GetProductAsync(string site, int currency, int settingStatus, string language);

    Task<IEnumerable<Promotion>> GetPromotionAsync(PromotionRequest request);

    Task<Promotion> GetPromotionByIdAsync(int promotionId, int custId);

    Task<ResultModel> AddPromotionAsync(PromotionRequest request);

    Task<ResultModel> EditPromotion(int promotionId, PromotionRequest request);

    Task<ResultModel> DeletePromotion(int promotionId, BaseActionRequest request);
}

public class PromotionService(IDynamicRepository repository, ICommonService commonService, ICacheRefService cacheRef) : IPromotionService
{
    public async Task<IEnumerable<Product>> GetProductAsync(string site, int currency, int settingStatus, string language)
    {
        var products = await repository.FetchAsync<Product>(new GetProductListCriteria(site, currency, settingStatus));
        foreach (var product in products)
        {
            product.ProductName = cacheRef.GetProductName(product.ProductId, language);
            product.ProductCategoryName = cacheRef.GetProductName(product.ProductCategoryId, language);
        }
        return products;
    }

    public async Task<IEnumerable<Promotion>> GetPromotionAsync(PromotionRequest request)
    {
        var criteria = new GetPromotionCriteria(request.PromotionType, request.StatusId, request.SiteDBGroupId, request.ActorId);
        var model = await repository.FetchMultipleAsync<Promotion, PromotionProductSetting>(criteria);

        foreach (var item in model.FirstResultSet)
        {
            item.ProductSettings = model.SecondResultSet.Where(i => i.PromotionId == item.PromotionId).ToList();
            foreach (var productSetting in item.ProductSettings)
            {
                productSetting.Products = string.IsNullOrEmpty(productSetting.ProductList)
                                            ? new List<Product>()
                                            : productSetting.ProductList.Split(',')
                                                .Select(p => p.Trim())
                                                .Select(p => new Product
                                                {
                                                    ProductCategoryId = productSetting.ProductCategoryId,
                                                    ProductId = Convert.ToInt32(p),
                                                    ProductName = cacheRef.GetProductName(Convert.ToInt32(p), request.Language)
                                                }).ToList();
                productSetting.ProductCategoryName = cacheRef.GetProductName(productSetting.ProductCategoryId, request.Language);
            }
        }

        return model.FirstResultSet;
    }

    public async Task<Promotion> GetPromotionByIdAsync(int promotionId, int custId)
    {
        var promotion = CreateFakePromotionModel().FirstOrDefault(p => p.PromotionId == promotionId);
        return await Task.FromResult(promotion);
    }

    public async Task<ResultModel> AddPromotionAsync(PromotionRequest request)
    {
        var addPromotionCriteria = new AddPromotionCriteria
        {
            PromotionType = request.PromotionType,
            FromDate = request.FromDate,
            ToDate = request.ToDate,
            PromotionDetails = request.ProductSettings != null
                ? JsonConvert.SerializeObject(request.ProductSettings)
                : string.Empty,
            PromotionContents = request.Contents != null
                ? JsonConvert.SerializeObject(request.Contents)
                : string.Empty,
            CreatedBy = request.UserId,
            IP = request.IP,
            SiteDbGroupId = request.SiteDBGroupId,
            Language = request.Language,
            Enabled = request.Enabled,
        };
        Console.WriteLine(JsonConvert.SerializeObject(addPromotionCriteria));
        await repository.ExecuteAsync(addPromotionCriteria);

        var result = new ResultModel(addPromotionCriteria.ErrCode, addPromotionCriteria.ErrMsg);

        return result;
    }

    public async Task<ResultModel> EditPromotion(int promotionId, PromotionRequest addPromotionMdoel)
    {
        var editPromotionCriteria = new EditPromotionCriteria
        {
            PromotionId = promotionId,
            PromotionType = addPromotionMdoel.PromotionType,
            StatusId = addPromotionMdoel.StatusId,
            FromDate = addPromotionMdoel.FromDate,
            ToDate = addPromotionMdoel.ToDate,
            //Title = addPromotionMdoel.Title,
            //Content = addPromotionMdoel.Content,
            // Banner = addPromotionMdoel.Banner,
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

    public async Task<ResultModel> DeletePromotion(int promotionId, BaseActionRequest request)
    {
        var command = new DeletePromotionCriteria()
        {
            PromotionId = promotionId,
            AgentId = request.ActorId,
            SubAccId = request.SubAccId,
            IP = request.IP,
            Language = request.Language,
            SiteDbGroupId = request.SiteDBGroupId,
            CustDbGroupId = request.CustDBGroupId
        };

        var logs = await repository.FetchAsync<WriteLogDBCommandModel>(command);
        if (logs is not null && logs.Any())
        {
            WriteLog(logs.ToList(), request.SiteDBGroupId);
        }

        return new ResultModel(command.ErrCode, command.ErrMsg);
    }

    private void WriteLog(List<WriteLogDBCommandModel> logs, int siteDbGroupId)
    {
        _ = Task.Run(async () =>
        {
            foreach (var log in logs)
            {
                await WriteLogRunSp(log, siteDbGroupId);
            }
        });
    }

    private async Task WriteLogRunSp(WriteLogDBCommandModel log, int siteDbGroupId)
    {
        var request = new BaseActionRequest
        {
            ActorId = log.CustId,
            CustDBGroupId = siteDbGroupId,
            UserId = log.UserId,
        };
        await commonService.WriteLogDBCommand(request, log);
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