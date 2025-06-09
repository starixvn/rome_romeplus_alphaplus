using Fanex.RemotingRef;

namespace AlphaPlus.CashService.API.Features.Shared.Services;

public interface ICacheRefService
{
    string GetProviderName(int providerId, string defaultName, string languageCode);
    string GetPaymentMethodName(int methodId, string defaultName, string languageCode);
    string GetBankName(int bankId, string defaultName, string languageCode);
    string GetDepositAutoModeName(string sourceType, string defaultName, string languageCode);
    string GetResourceDataByRefIdAndResourceId(string refId, int resourceId, string languageCode);
}

public class CacheRefService(IRemotingRefService remotingRefService, Fanex.Logging.ILogger logger) : ICacheRefService
{
    public string GetProviderName(int providerId, string defaultName, string languageCode) {
        var result = GetResourceDataByRefIdAndResourceId("CashPaymentProvider", providerId, languageCode);
        return string.IsNullOrWhiteSpace(result) ? defaultName : result;
    }

    public string GetPaymentMethodName(int methodId, string defaultName, string languageCode) {
        var result = GetResourceDataByRefIdAndResourceId("CashPaymentMethod", methodId, languageCode);
        return string.IsNullOrWhiteSpace(result) ? defaultName : result;
    }

    public string GetBankName(int bankId, string defaultName, string languageCode) {
        var result = GetResourceDataByRefIdAndResourceId("CashBankList", bankId, languageCode);
        return string.IsNullOrWhiteSpace(result) ? defaultName : result;
    }

    public string GetDepositAutoModeName(string sourceType, string defaultName, string languageCode) {
        var result = GetResourceDataByRefIdAndResourceId("CashDepositAutoMode", sourceType, languageCode);
        return string.IsNullOrWhiteSpace(result) ? defaultName : result;
    }

    public string GetResourceDataByRefIdAndResourceId(string refId, int resourceId, string languageCode)
        => GetResourceDataByRefIdAndResourceId(refId, resourceId.ToString(), languageCode);

    private string GetResourceDataByRefIdAndResourceId(string refId, string resourceId, string languageCode) {
        try {
            return remotingRefService.GetResourceDataByRefIdAndResourceId(
                refId,
                resourceId,
                languageCode,
                string.Empty);
        }
        catch (Exception e) {
            logger.Error(e.Message, e);
            return string.Empty;
        }
    }
}