using Fanex.Caching;
using AlphaPlus.Settings.Transfer.API.Configurations;
using AlphaPlus.Settings.Transfer.API.Models;
using Fanex.Logging;
using Newtonsoft.Json;

namespace AlphaPlus.Settings.Transfer.API.Services;

public interface ITransferCacheService
{
    List<TransferCacheModel> Get(string cacheKey);

    TransferCacheModel CacheContains(List<TransferCacheModel> listTransferCacheModel, int custId);

    void CacheAdd(List<TransferCacheModel> listTransferCacheModel, int custId, int adminId, string adminName);

    bool Set(string cacheKey, List<TransferCacheModel> value, int adminId, string adminName);

    void CacheRemove(List<TransferCacheModel> listTransferCacheModel, int custId);

    void CacheRemoveSuccessTransferCustIds(string cacheKey, List<int> listSuccessTransferCustIds, int adminId, string adminName);
}

public class TransferCacheService(ICacheService cacheService) : ITransferCacheService
{
    private readonly ICacheService cacheService = cacheService;
    private static readonly object lockObject = new object();

    public List<TransferCacheModel> Get(string cacheKey)
    {
        lock (lockObject)
        {
            string value = cacheService.Get<string>(cacheKey);

            return string.IsNullOrWhiteSpace(value)
                ? null
                : JsonConvert.DeserializeObject<List<TransferCacheModel>>(value);
        }
    }

    public TransferCacheModel CacheContains(List<TransferCacheModel> listTransferCacheModel, int custId)
    {
        foreach (var item in listTransferCacheModel)
        {
            if (item.CustIds.Contains(0) || item.CustIds.Contains(custId))
            {
                return item;
            }
        }

        return null;
    }

    public void CacheAdd(List<TransferCacheModel> listTransferCacheModel, int custId, int adminId, string adminName)
    {
        foreach (var item in listTransferCacheModel)
        {
            if (item.AdminId == adminId)
            {
                item.CustIds.Add(custId);
                return;
            }
        }

        var newAdminTransferCache = new TransferCacheModel
        {
            AdminId = adminId,
            AdminName = adminName
        };

        newAdminTransferCache.CustIds.Add(custId);
        listTransferCacheModel.Add(newAdminTransferCache);
    }

    public bool Set(string cacheKey, List<TransferCacheModel> value, int adminId, string adminName)
    {
        lock (lockObject)
        {
            var cachePolicy = new CacheItemOptions();
            cacheService.Set(cacheKey, JsonConvert.SerializeObject(value), cachePolicy);

            if (AppSettings.EnableTransferLogInfo)
            {
                LogService.Info(InfoMessageFormat(value, adminId, adminName), AppSettings.TransferLogCategory);
            }

            if (!CompareAfterSet(cacheKey, value))
            {
                LogService.Error(ErrorMessageFormat(value));
                return false;
            }

            return true;
        }
    }

    private bool CompareAfterSet(string cacheKey, List<TransferCacheModel> value)
    {
        var temp = Get(cacheKey);
        if (temp == null && value == null)
        {
            return true;
        }

        if (temp != null && value != null && value.Count == temp.Count)
        {
            for (int i = 0; i < value.Count; i++)
            {
                if (!value[i].Equals(temp[i]))
                {
                    return false;
                }
            }

            return true;
        }

        return false;
    }

    private static string ErrorMessageFormat(List<TransferCacheModel> value)
        => $"[Agent Transfer Process] Redis cannot set value: {JsonConvert.SerializeObject(value)}";

    private string InfoMessageFormat(List<TransferCacheModel> value, int adminId, string adminName)
        => $"[Agent Transfer Process - PID: {Thread.CurrentThread.ManagedThreadId}] {adminName}({adminId}) set redis value to: {JsonConvert.SerializeObject(value)}";

    public void CacheRemoveSuccessTransferCustIds(string cacheKey, List<int> listSuccessTransferCustIds, int adminId, string adminName)
    {
        var listMemberTransferCache = Get(cacheKey);
        if (listMemberTransferCache != null)
        {
            foreach (var successTransferCustId in listSuccessTransferCustIds)
            {
                CacheRemove(listMemberTransferCache, successTransferCustId);
            }

            Set(cacheKey, listMemberTransferCache, adminId, adminName);
        }
    }

    public void CacheRemove(List<TransferCacheModel> listTransferCacheModel, int custId)
    {
        foreach (var cacheModel in listTransferCacheModel)
        {
            if (cacheModel.CustIds.Contains(custId))
            {
                cacheModel.CustIds.Remove(custId);
                return;
            }
        }
    }
}
