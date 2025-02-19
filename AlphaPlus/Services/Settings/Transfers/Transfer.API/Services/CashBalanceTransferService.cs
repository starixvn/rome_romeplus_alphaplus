using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Transfer.API.Configurations;
using AlphaPlus.Settings.Transfer.API.Criteria.CashBalanceTransfer;
using AlphaPlus.Settings.Transfer.API.Models;
using AlphaPlus.Settings.Transfer.API.Models.CashBalanceTransfer;
using AlphaPlus.Shared.AppResources.Settings.Transfer;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Transfer.API.Services;

public interface ICashBalanceTransferService
{
    Task<CashBalanceTransfer> GetCashBalanceTransfer(int custId, bool isDirectMember, string agIP);

    Task<Result> AddCashBalanceTransfer(
        int fromCustId,
        int toCustId,
        decimal amount,
        int creator,
        string creatorName,
        int subAccId,
        string ip,
        string languageKey,
        string agIP,
        bool isDirectMember);
}

public class CashBalanceTransferService(IDynamicRepository repository, ITransferCacheService transferCacheService) : ICashBalanceTransferService
{
    private const string AgentTransferMemberListCache = "AgentMultipleTransfer.{0}.{1}";
    private const string ConcurrentSingleTransferResultMessage = "<span style=\"color: red\">The transfer has been proceeding by {0}<br/>";

    private readonly IDynamicRepository repository = repository;

    public Task<CashBalanceTransfer> GetCashBalanceTransfer(int custId, bool isDirectMember, string agIP)
    {
        var criteria = new GetCashBalanceTransferCriteria(custId, isDirectMember);

        return repository.GetAsync<CashBalanceTransfer>(criteria, agIP);
    }

    public async Task<Result> AddCashBalanceTransfer(
        int fromCustId,
        int toCustId,
        decimal amount,
        int creator,
        string creatorName,
        int subAccId,
        string ip,
        string languageKey,
        string agIP,
        bool isDirectMember)
    {
        string cacheKey = string.Format(AgentTransferMemberListCache, AppSettings.SiteLabel, creator);
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
        transferCacheService.Set(cacheKey, custIdCache, creator, creatorName);

        var criteria = new AddCashBalanceTransferCriteria(
            fromCustId,
            toCustId,
            amount * -1,
            creator,
            subAccId,
            ip,
            languageKey,
            isDirectMember);
        try
        {
            await repository.ExecuteAsync(criteria, agIP);
        }
        finally
        {
            transferCacheService.CacheRemoveSuccessTransferCustIds(cacheKey, new List<int> { toCustId }, creator, creatorName);
        }

        return new Result
        {
            Code = criteria.ErrCode,
            Message = criteria.ErrCode == 0 ? Resources.TransferSuccessfully : criteria.ErrMsg
        };
    }
}
