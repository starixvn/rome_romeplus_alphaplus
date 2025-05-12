using Fanex.RemotingRef;

namespace AlphaPlus.CashService.API.Services;

public interface ICacheRefService
{
    string GetProviderName(int providerId, string defaultName);
    string GetPaymentMethodName(int methodId, string defaultName);
    string GetBankName(int bankId, string defaultName);
    string GetDepositAutoModeName(string sourceType, string defaultName);

    string GetResourceDataByRefIdAndResourceId(string refId, int resourceId);
}

public class CacheRefService(IRemotingRefService remotingRefService) : ICacheRefService
{
    public string GetProviderName(int providerId, string defaultName) {
        var result = GetResourceDataByRefIdAndResourceId("CashPaymentProvider", providerId);
        return string.IsNullOrWhiteSpace(result) ? defaultName : result;
    }

    public string GetPaymentMethodName(int methodId, string defaultName) {
        var result = GetResourceDataByRefIdAndResourceId("CashPaymentMethod", methodId);
        return string.IsNullOrWhiteSpace(result) ? defaultName : result;
    }

    public string GetBankName(int bankId, string defaultName) {
        var result = GetResourceDataByRefIdAndResourceId("CashBankList", bankId);
        return string.IsNullOrWhiteSpace(result) ? defaultName : result;
    }

    public string GetDepositAutoModeName(string sourceType, string defaultName) {
        var result = GetResourceDataByRefIdAndResourceId("CashDepositAutoMode", sourceType);
        return string.IsNullOrWhiteSpace(result) ? defaultName : result;
    }

    public string GetResourceDataByRefIdAndResourceId(string refId, int resourceId)
        => GetResourceDataByRefIdAndResourceId(refId, resourceId.ToString());

    private string GetResourceDataByRefIdAndResourceId(string refId, string resourceId)
        => remotingRefService.GetResourceDataByRefIdAndResourceId(
        refId,
        resourceId,
        LanguageCode,
        string.Empty);

    private static string LanguageCode => Thread.CurrentThread.CurrentCulture.Name;
}