namespace AlphaPlus.CashService.API.Features.Promotion.Services;

using AlphaPlus.CashService.API.Features.Shared.Services;
using Criteria;
using Fanex.Data.Repository;
using Models;
using Newtonsoft.Json;
using PendingDeposit.Services;
using Requests;
using Shared.Models;
using Shared.Requests;
using Transactions.Services;

public interface IPromotionService
{
    Task<IEnumerable<Product>> GetProductAsync(string site, int currency, int settingStatus, string language);

    Task<IEnumerable<Promotion>> GetPromotionAsync(PromotionRequest request);

    Task<Promotion> GetPromotionByIdAsync(int promotionId, int custId, int siteDBGroupId, string language);

    Task<ResultModel> AddPromotionAsync(PromotionRequest request);

    Task<ResultModel> EditPromotionAsync(EditPromotionRequest request);

    Task<ResultModel> DeletePromotion(int promotionId, BaseActionRequest request);

    Task<ResultModel> AddBonusAsync(AddBonusRequest request);

    Task<ResultModel> UpdatePromoteTransactionStatusAsync(UpdatePromoteTransactionRequest request);

    Task<(IEnumerable<PromotionTransaction>, int)> GetPromotionTransactionAsync(GetPromotionTransactionRequest criteria);
}

public class PromotionService(IDynamicRepository repository,
    ICommonService commonService,
    ICacheRefService cacheRef,
    IPendingDepositService depositService,
    ITransactionService transactionService) : IPromotionService
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
        var (FirstResultSet, SecondResultSet) = await repository.FetchMultipleAsync<Promotion, PromotionProductSetting>(criteria);

        foreach (var item in FirstResultSet)
        {
            item.ProductSettings = SecondResultSet.Where(i => i.PromotionId == item.PromotionId).ToList();
            foreach (var productSetting in item.ProductSettings)
            {
                productSetting.Products = string.IsNullOrEmpty(productSetting.ProductList)
                                            ? []
                                            : [.. productSetting.ProductList.Split(',')
                                                .Select(p => p.Trim())
                                                .Select(p => new Product
                                                {
                                                    ProductCategoryId = productSetting.ProductCategoryId,
                                                    ProductId = Convert.ToInt32(p),
                                                    ProductName = cacheRef.GetProductName(Convert.ToInt32(p), request.Language)
                                                })];
                productSetting.ProductCategoryName = cacheRef.GetProductName(productSetting.ProductCategoryId, request.Language);
            }
        }

        return FirstResultSet;
    }

    public async Task<Promotion> GetPromotionByIdAsync(int promotionId, int custId, int siteDBGroupId, string language)
    {
        var criteria = new GetPromotionByIdCriteria(promotionId, siteDBGroupId, custId);
        var (promotion, setting, content) = await repository.FetchMultipleAsync<Promotion, PromotionProductSetting, PromotionLanguageSetting>(criteria);

        foreach (var item in promotion)
        {
            item.ProductSettings = [.. setting];
            foreach (var productSetting in item.ProductSettings)
            {
                productSetting.Products = string.IsNullOrEmpty(productSetting.ProductList)
                                            ? []
                                            : [.. productSetting.ProductList.Split(',')
                                                .Select(p => p.Trim())
                                                .Select(p => new Product
                                                {
                                                    ProductCategoryId = productSetting.ProductCategoryId,
                                                    ProductId = Convert.ToInt32(p),
                                                    ProductName = cacheRef.GetProductName(Convert.ToInt32(p), language)
                                                })];
                productSetting.ProductCategoryName = cacheRef.GetProductName(productSetting.ProductCategoryId, language);
            }
            item.LanguageSettings = [.. content];
        }

        return promotion.FirstOrDefault();
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

        var logAddPromotion = await repository.FetchAsync<WriteLogDBCommandModel>(addPromotionCriteria);

        var result = new ResultModel(addPromotionCriteria.ErrCode, addPromotionCriteria.ErrMsg);
        if (addPromotionCriteria.ErrCode == 0 && logAddPromotion is not null && logAddPromotion.Any())
        {
            WriteLog([.. logAddPromotion], request.SiteDBGroupId);
        }

        return result;
    }

    public async Task<ResultModel> EditPromotionAsync(EditPromotionRequest editPromotionMdoel)
    {
        var result = new ResultModel(0, "Success");

        if (HasContentChanged(editPromotionMdoel.Contents, editPromotionMdoel.OriginalContents))
        {
            var editPromotionCriteria = new EditPromotionCriteria
            {
                PromotionId = editPromotionMdoel.PromotionId,
                PromotionContents = editPromotionMdoel.Contents != null
                    ? JsonConvert.SerializeObject(editPromotionMdoel.Contents)
                    : string.Empty,
                AgentId = editPromotionMdoel.UserId,
                IP = editPromotionMdoel.IP,
                SiteDbGroupId = editPromotionMdoel.SiteDBGroupId,
                Language = editPromotionMdoel.Language,
            };

            var logEditPromotion = await repository.FetchAsync<WriteLogDBCommandModel>(editPromotionCriteria);
            if (editPromotionCriteria.ErrCode == 0 && logEditPromotion is not null && logEditPromotion.Any())
            {
                WriteLog(logEditPromotion.ToList(), editPromotionMdoel.SiteDBGroupId);
            }
        }

        if (editPromotionMdoel.OriginalEnabled.HasValue && editPromotionMdoel.OriginalEnabled.Value != editPromotionMdoel.Enabled)
        {
            var updateStatusCriteria = new UpdatePromotionStatusCriteria
            {
                PromotionId = editPromotionMdoel.PromotionId,
                AgentId = editPromotionMdoel.UserId,
                SubAccId = editPromotionMdoel.SubAccId,
                IP = editPromotionMdoel.IP,
                SiteDbGroupId = editPromotionMdoel.SiteDBGroupId,
                Enabled = editPromotionMdoel.Enabled,
                Language = editPromotionMdoel.Language
            };

            var logUpdateStatus = await repository.FetchAsync<WriteLogDBCommandModel>(updateStatusCriteria);
            if (!HasContentChanged(editPromotionMdoel.Contents, editPromotionMdoel.OriginalContents))
            {
                result = new ResultModel(updateStatusCriteria.ErrCode, updateStatusCriteria.ErrMsg);
                if (updateStatusCriteria.ErrCode == 0 && logUpdateStatus is not null && logUpdateStatus.Any())
                {
                    WriteLog(logUpdateStatus.ToList(), editPromotionMdoel.SiteDBGroupId);
                }
            }
        }

        return result;
    }

    public async Task<ResultModel> DeletePromotion(int promotionId, BaseActionRequest request)
    {
        var command = new DeletePromotionCriteria
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
        if (command.ErrCode == 0 && logs is not null && logs.Any())
        {
            WriteLog(logs.ToList(), request.SiteDBGroupId);
        }

        return new ResultModel(command.ErrCode, command.ErrMsg);
    }

    public async Task<ResultModel> AddBonusAsync(AddBonusRequest request)
    {
        var memberSettings = await depositService.GetMemberSettingDepositWhenProcessTransactionAsync(request.CustId, request.UserId, request.CustDBGroupId);
        var criteria = new AddBonusCriteria
        {
            CustId = request.CustId,
            UplineId = request.UserId,
            Amount = request.Amount,
            Remark = request.Remark,
            AgentGivenCredit = memberSettings.AgentGivenCredit,
            CreditMembers_GivenCredit = memberSettings.CreditMembers_GivenCredit,
            CustDbGroupId = request.CustDBGroupId,
            SiteDbGroupId = request.SiteDBGroupId,
            IP = request.IP
        };
        var logs = await repository.FetchAsync<WriteLogDBCommandModel>(criteria);
        var result = new ResultModel(criteria.ErrCode, criteria.ErrMsg);
        transactionService.OnAfterUpdateTransaction(criteria.CashAccumCreditTbl, request.SiteDBGroupId, request.CustDBGroupId, logs.ToList());
        return result;
    }

    public async Task<ResultModel> UpdatePromoteTransactionStatusAsync(UpdatePromoteTransactionRequest request)
    {
        var memberSettings = await depositService.GetMemberSettingDepositWhenProcessTransactionAsync(request.CustId, request.UserId, request.CustDBGroupId);

        var criteria = new UpdatePromoteTransactionCriteria
        {
            TransId = request.TransId,
            CustId = request.CustId,
            AgentId = request.UserId,
            SubAccId = request.SubAccId,
            StatusId = request.StatusId,
            NewAmount = request.Amount,
            IP = request.IP,
            AgentGivenCredit = memberSettings.AgentGivenCredit,
            CreditMembersGivenCredit = memberSettings.CreditMembers_GivenCredit,
            SiteDbGroupId = request.SiteDBGroupId,
        };
        var logs = await repository.FetchAsync<WriteLogDBCommandModel>(criteria);
        var result = new ResultModel(criteria.ErrCode, criteria.ErrMsg);
        transactionService.OnAfterUpdateTransaction(criteria.CashAccumCreditTbl, request.SiteDBGroupId, request.CustDBGroupId, logs.ToList());
        return result;
    }

    public async Task<(IEnumerable<PromotionTransaction>, int)> GetPromotionTransactionAsync(GetPromotionTransactionRequest request)
    {
        var criteria = new GetPromotionTransactionCriteria
        {
            CustId = request.CustId,
            SiteDbGroupId = request.SiteDbGroupId,
            FromDate = request.FromDate,
            PromotionType = request.PromotionTypeId,
            Status = request.StatusId,
            ToDate = request.ToDate,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
        var promotions = await repository.FetchAsync<PromotionTransaction>(criteria);
        return (promotions, criteria.TotalRecords);
    }

    private void WriteLog(List<WriteLogDBCommandModel> logs, short siteDbGroupId)
    {
        _ = Task.Run(async () =>
        {
            foreach (var log in logs)
            {
                await WriteLogRunSp(log, siteDbGroupId);
            }
        });
    }

    private async Task WriteLogRunSp(WriteLogDBCommandModel log, short siteDbGroupId)
    {
        var request = new BaseActionRequest
        {
            ActorId = log.CustId,
            CustDBGroupId = siteDbGroupId,
            UserId = log.UserId,
        };
        await commonService.WriteLogDBCommand(request, log);
    }

    private static bool HasContentChanged(List<PromotionLanguageSetting> current, List<PromotionLanguageSetting> original)
    {
        if (current == null && original == null) return false;
        if (current == null || original == null) return true;
        if (current.Count != original.Count) return true;

        var currentJson = JsonConvert.SerializeObject(current.OrderBy(x => x.Language));
        var originalJson = JsonConvert.SerializeObject(original.OrderBy(x => x.Language));

        return currentJson != originalJson;
    }
}