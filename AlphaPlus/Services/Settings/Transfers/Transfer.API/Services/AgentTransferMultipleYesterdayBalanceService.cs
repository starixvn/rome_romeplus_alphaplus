using System.Text;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Transfer.API.Configurations;
using AlphaPlus.Settings.Transfer.API.Criteria.TransferYesterdayBalance;
using AlphaPlus.Settings.Transfer.API.Models;
using AlphaPlus.Settings.Transfer.API.Params.TransferList;
using AlphaPlus.Settings.Transfer.API.Params.TransferYesterdayBalance;
using AlphaPlus.Shared.AppResources.Settings.Transfer;
using Fanex.Logging;

namespace AlphaPlus.Settings.Transfer.API.Services;

public class AgentTransferMultipleYesterdayBalanceService(
    AgentTransferMultipleYesterdayBalanceParam request,
    ITransferService transferService,
    ITransferListService transferListService,
    IYesterdayBalanceTransferService yesterdayBalanceTransferService)
{
    private const string AgentTransferMemberListCache = "AgentMultipleTransfer.{0}.{1}";
    private const string SuccessTransferResultMessage = "Successful transfer(s): {0}";
    private const string ConcurrentTransferResultMessage = "<span style=\"color: red\">Transfer(s) has been proceeding by {0}:</span> {1}";
    private const string CacheErrorMessage = "Agent Transfer Process Error Log.\r\nThere is some error with Cache: {0}\r\nInner Message: {1}.";

    private readonly AgentTransferMultipleYesterdayBalanceParam request = request;
    private readonly ITransferService transferService = transferService;
    private readonly ITransferCacheService transferCacheService = new TransferCacheService();
    private readonly ITransferListService transferListService = transferListService;
    private readonly IYesterdayBalanceTransferService yesterdayBalanceTransferService = yesterdayBalanceTransferService;
    private readonly string cacheKey = string.Format(AgentTransferMemberListCache, AppSettings.SiteLabel, request.ActorId);
    private readonly IDictionary<int, string> custIdsAndNames = new Dictionary<int, string>();
    private readonly List<int> listCustIdsTransferSuccess = new List<int>();
    private readonly List<TransferCacheModel> listCustIdsTransferConcurrent = new List<TransferCacheModel>();
    private readonly List<TransferFailedModel> listCustIdsTransferError = new List<TransferFailedModel>();

    public async Task<Result> ProcessTransfer()
    {
        var isAllDayBalanceTransfer = await transferService.IsAllDayBalanceTransferAsync(request.ActorId);
        var custIds = await GetCustIds();
        var custIdsBatches = custIds.Chunk(AppSettings.MaxCustIdTransfer);

        var result = Result.CreateSuccessfulResult(Resources.TransferSuccessfully);
        foreach (var item in custIdsBatches)
        {
            result = await ProcessTransfer(item.ToList(), isAllDayBalanceTransfer);

            if (result.IsFailed)
            {
                break;
            }
        }

        result.Message = ComposeResultMessage(result);

        return result;
    }

    private async Task<IEnumerable<int>> GetCustIds()
    {
        var param = new TransferListParam(
            request.ActorId,
            request.ActorLevel.Value,
            request.Username,
            request.Status,
            request.AutoTransfer,
            LevelType: 2, // 2: Direct Member incase of Super/Master
            request.AllYesterdayBalance,
            request.AllowTotalBalanceTransfer,
            1,
            int.MaxValue - 1,
            request.AGIP);
        var transferReport = await transferListService.GetTransferList(param);

        foreach (var item in transferReport.Customers)
        {
            custIdsAndNames[item.CustId] = item.Username;
        }

        return request.IsAllPages && !string.IsNullOrEmpty(request.Username)
            ? custIdsAndNames.Keys
            : request.MemberIds.ToListInt();
    }

    private async Task<Result> ProcessTransfer(List<int> custIds, bool isAllDayBalanceTransfer)
    {
        var validCustIds = new List<int>();
        var invalidCustId = new List<int>();
        var isUsingCache = CacheFilterInputListCustIds(custIds, validCustIds, invalidCustId);

        if (validCustIds.Count > 0)
        {
            var memberIds = isUsingCache ? validCustIds : custIds;

            try
            {
                Result result;
                if (request.ActorLevel.IsAgentLevel())
                {
                    var criteria = new AgentTransferMultipleYesterdayBalanceCriteria(
                        request.ActorId,
                        string.Join(',', memberIds),
                        request.Status,
                        request.AutoTransfer,
                        isAllDayBalanceTransfer,
                        request.Language,
                        request.IP,
                        request.SubAccId);
                    result = await yesterdayBalanceTransferService.MultipleMemberTransfer(criteria, request.AGIP);
                }
                else
                {
                    var criteria = new DirectMemberTransferMultipleYesterdayBalanceCriteria(
                        request.ActorId,
                        string.Join(',', memberIds),
                        -request.ActorLevel.Value,
                        request.Status,
                        request.AutoTransfer,
                        request.Language,
                        request.IP,
                        request.SubAccId);
                    result = await yesterdayBalanceTransferService.MultipleDirectMemberTransfer(criteria, request.AGIP);
                }

                if (result.IsSuccessful)
                {
                    listCustIdsTransferSuccess.AddRange(memberIds);
                }
                else
                {
                    listCustIdsTransferError.Add(new TransferFailedModel(memberIds, result.Message));
                }

                return result;
            }
            catch (Exception ex)
            {
                return Result.CreateErrorResult(ex.Message);
            }
            finally
            {
                if (isUsingCache)
                {
                    transferCacheService.CacheRemoveSuccessTransferCustIds(cacheKey, validCustIds, request.AdminId, request.AdminName);
                }
            }
        }

        return new Result();
    }

    private bool CacheFilterInputListCustIds(List<int> custIds, List<int> validCustIds, List<int> invalidCustIds)
    {
        try
        {
            var membersTransferCache = transferCacheService.Get(cacheKey);

            if (membersTransferCache != null && membersTransferCache.Count > 0)
            {
                foreach (int custid in custIds)
                {
                    var adminsTransferAll = transferCacheService.CacheContains(membersTransferCache, 0);
                    var adminsTransferConcurrent = transferCacheService.CacheContains(membersTransferCache, custid);

                    if (adminsTransferAll != null)
                    {
                        invalidCustIds.Add(custid);
                        transferCacheService.CacheAdd(listCustIdsTransferConcurrent, custid, adminsTransferAll.AdminId, adminsTransferAll.AdminName);
                    }
                    else if (adminsTransferConcurrent != null)
                    {
                        invalidCustIds.Add(custid);
                        transferCacheService.CacheAdd(listCustIdsTransferConcurrent, custid, adminsTransferConcurrent.AdminId, adminsTransferConcurrent.AdminName);
                    }
                    else
                    {
                        transferCacheService.CacheAdd(membersTransferCache, custid, request.AdminId, request.AdminName);
                        validCustIds.Add(custid);
                    }
                }
            }
            else
            {
                membersTransferCache = new List<TransferCacheModel>();

                foreach (int custid in custIds)
                {
                    transferCacheService.CacheAdd(membersTransferCache, custid, request.AdminId, request.AdminName);
                    validCustIds.Add(custid);
                }
            }

            transferCacheService.Set(cacheKey, membersTransferCache, request.AdminId, request.AdminName);
            return true;
        }
        catch (Exception ex)
        {
            Logger.Log.Error(string.Format(CacheErrorMessage, ex.Message, ex.InnerException), ex);
            return false;
        }
    }

    private string ComposeResultMessage(Result result)
    {
        if (result.IsFailed)
        {
            return result.Message;
        }

        if (result.IsSuccessful && request.MemberIds.Equals("0"))
        {
            return listCustIdsTransferConcurrent.Count > 0
                ? string.Format(ConcurrentTransferResultMessage, listCustIdsTransferConcurrent[0].AdminName, "All")
                : Resources.TransferSuccessfully;
        }

        var resultMessage = string.Empty;

        if (listCustIdsTransferSuccess.Count > 0)
        {
            resultMessage += string.Format(SuccessTransferResultMessage, GetCustNamesFromCustIds(listCustIdsTransferSuccess));
        }

        if (listCustIdsTransferConcurrent.Count > 0)
        {
            var concurrentBuilder = new StringBuilder();

            foreach (var item in listCustIdsTransferConcurrent)
            {
                concurrentBuilder.Append(string.Format(ConcurrentTransferResultMessage, item.AdminName, GetCustNamesFromCustIds(item.CustIds)));
            }

            resultMessage += $"<br/>{concurrentBuilder.ToString()}";
        }

        return resultMessage;
    }

    private string GetCustNamesFromCustIds(IEnumerable<int> custIds)
    {
        var custNames = new List<string>();

        foreach (var item in custIds)
        {
            if (custIdsAndNames.TryGetValue(item, out var name))
            {
                custNames.Add(name);
            }
            else
            {
                custNames.Add(item.ToString());
            }
        }

        return string.Join(", ", custNames);
    }
}
