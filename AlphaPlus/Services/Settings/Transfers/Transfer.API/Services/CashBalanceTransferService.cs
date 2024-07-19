using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Transfer.API.Configurations;
using AlphaPlus.Settings.Transfer.API.Criteria.CashBalanceTransfer;
using AlphaPlus.Settings.Transfer.API.Models;
using AlphaPlus.Settings.Transfer.API.Models.CashBalanceTransfer;
using AlphaPlus.Shared.AppResources.Settings.Transfer;
using Fanex.Caching;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Transfer.API.Services;

public interface ICashBalanceTransferService
{
    Task<CashBalanceTransfer> GetCashBalanceTransfer(int custId);

    Task<Result> AddCashBalanceTransfer(
        int fromCustId,
        int toCustId,
        decimal amount,
        int creator,
        string creatorName,
        int subAccId,
        string ip,
        string languageKey,
        string agIP);
}

public class CashBalanceTransferService(
    IDynamicRepository repository,
    ICacheService cacheService) : ICashBalanceTransferService
{
    private const string AgentTransferMemberListCache = "AgentMultipleTransfer.{0}.{1}";
    private const string ConcurrentSingleTransferResultMessage = "<span style=\"color: red\">The transfer has been proceeding by {0}<br/>";

    private readonly IDynamicRepository repository = repository;
    private readonly ICacheService cacheService = cacheService;

    public Task<CashBalanceTransfer> GetCashBalanceTransfer(int custId)
        => repository.GetAsync<CashBalanceTransfer>(new GetCashBalanceTransferCriteria(custId));

    public async Task<Result> AddCashBalanceTransfer(
        int fromCustId,
        int toCustId,
        decimal amount,
        int creator,
        string creatorName,
        int subAccId,
        string ip,
        string languageKey,
        string agIP)
    {
        string cacheKey = string.Format(AgentTransferMemberListCache, AppSettings.SiteLabel, creator);
        TransferCacheService transferCacheService = new TransferCacheServiceImpl(cacheService) { AdminId = creator, AdminName = creatorName };

        var custIdCache = transferCacheService.Get(cacheKey) ?? new List<TransferCacheModel>();
        var concurrentTrasfer = transferCacheService.CacheContains(custIdCache, toCustId);

        if (concurrentTrasfer != null)
        {
            return new Result
            {
                Code = 105,
                Message = string.Format(ConcurrentSingleTransferResultMessage, concurrentTrasfer.AdminName)
            };
        }

        transferCacheService.CacheAdd(custIdCache, toCustId, creator, creatorName);
        transferCacheService.Set(cacheKey, custIdCache);

        var criteria = new AddCashBalanceTransferCriteria(
            fromCustId,
            toCustId,
            amount * -1,
            creator,
            subAccId,
            ip,
            languageKey);

        try
        {
            await repository.ExecuteAsync(criteria, agIP);
        }
        finally
        {
            transferCacheService.CacheRemoveSuccessTransferCustIds(cacheKey, new List<int> { toCustId });
        }

        return new Result
        {
            Code = criteria.ErrCode,
            Message = criteria.ErrCode == 0 ? Resources.TransferSuccessfully : criteria.ErrMsg
        };
    }
}
