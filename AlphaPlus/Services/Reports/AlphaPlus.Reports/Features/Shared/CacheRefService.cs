using Fanex.RemotingRef;

namespace AlphaPlus.Reports.Features.Shared;

public interface ICacheRefService
{
    string GetProductName(int productId);

    string GetProductDetailName(int resourceId);

    string GetResourceDataByRefIdAndResourceId(string refId, int resourceId);
}

public class CacheRefService(IRemotingRefService remotingRefService) : ICacheRefService
{
    public string GetProductName(int productId)
        => GetResourceDataByRefIdAndResourceId("Product", productId);

    public string GetProductDetailName(int resourceId)
        => GetResourceDataByRefIdAndResourceId("ProductDetail", resourceId);

    public string GetResourceDataByRefIdAndResourceId(string refId, int resourceId)
        => remotingRefService.GetResourceDataByRefIdAndResourceId(
        refId,
        resourceId.ToString(),
        LanguageCode,
        string.Empty);

    private static string LanguageCode => Thread.CurrentThread.CurrentCulture.Name;
}