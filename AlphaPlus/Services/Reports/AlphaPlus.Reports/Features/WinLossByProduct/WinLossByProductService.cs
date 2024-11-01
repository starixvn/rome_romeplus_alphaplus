using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Constants;
using AlphaPlus.Shared.AppResources.Report;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.WinLossByProduct;

public interface IWinLossByProductService
{
    Task<(IEnumerable<WinLossByProductModel>, WinLossByBetTypeModel)> GetWinLossByProduct(WinLossByProductParam param);
}

public class WinLossByProductService(
    IDynamicRepository repository,
    IReportDateService reportDateService,
    ICacheRefService cacheRefService,
    IPartialUMService partialUMService) : IWinLossByProductService
{
    private const string MMROddTypeId = "6";

    public async Task<(IEnumerable<WinLossByProductModel>, WinLossByBetTypeModel)> GetWinLossByProduct(WinLossByProductParam param)
    {
        var reportDate = await reportDateService.GetBalanceUpDayAndMoveBetsDay();
        var criteria = new WinLossByProductCriteria(
            param.CustId,
            param.FromDate,
            param.ToDate,
            reportDate.BalanceUpDay,
            reportDate.MoveBetsDay,
            param.ProductIds,
            param.CustLevelId,
            param.ProductDetailXml);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        var grandTotal = new WinLossByBetTypeModel();
        var winlossByProducts = Enumerable.Empty<WinLossByProductModel>();
        var rawdata = await repository.FetchAsync<WinLossByBetTypeModel>(criteria);

        if (rawdata.Any())
        {
            var mMROddTypeName = cacheRefService.GetOddsTypeName(MMROddTypeId);

            winlossByProducts = rawdata.GroupBy(
                winloss => winloss.ProductId,
                (productId, winloss) =>
                {
                    var sportTotal = new WinLossByBetTypeModel();
                    var winlossBySports = BuildWinLossBySports(winloss, mMROddTypeName, sportTotal).ToList();

                    grandTotal += sportTotal;

                    return new WinLossByProductModel(
                        productId,
                        cacheRefService.GetProductName(productId),
                        winlossBySports,
                        sportTotal);
                })
                .ToList();
        }

        return (winlossByProducts, grandTotal);
    }

    private IEnumerable<WinLossBySportModel> BuildWinLossBySports(
        IEnumerable<WinLossByBetTypeModel> winlossByProducts,
        string mMROddTypeName,
        WinLossByBetTypeModel sportTotal)
        => winlossByProducts.GroupBy(wl => wl.SportType, (sportType, winloss) =>
        {
            var total = new WinLossByBetTypeModel();
            foreach (var item in winloss)
            {
                item.BetTypeName = BuildBetTypeName(item.ProductId, item.BetType, mMROddTypeName, item.IsMMR, item.LiveIndicator);
                total += item;
            }

            sportTotal += total;

            return new WinLossBySportModel(
                sportType,
                sportType == 0 ? string.Empty : cacheRefService.GetSportName(sportType),
                winloss,
                total);
        });

    private string BuildBetTypeName(int productId, int betType, string mMROddTypeName, bool isMMR, bool liveIndicator)
    {
        var betTypeName = cacheRefService.GetBetTypeName(betType);
        betTypeName = isMMR ? $"{betTypeName} ({mMROddTypeName})" : betTypeName;

        return productId == ProductIdConstants.NumberGame
            ? BuildNumberGameBetTypeName(betType, betTypeName, liveIndicator)
            : betTypeName;
    }

    private static string BuildNumberGameBetTypeName(int betType, string betTypeName, bool liveIndicator)
        => betType switch
        {
            BetTypeConstants.N_Over_Under => $"{SharedResources.Next} {betTypeName}",
            BetTypeConstants.N_Odd_Even => $"{(liveIndicator ? SharedResources.Next : SharedResources.FT)} {betTypeName}",
            BetTypeConstants.NumberWheel => $"{betTypeName} {(liveIndicator ? SharedResources.LiveGame : SharedResources.PreGame)}",
            _ => betTypeName
        };
}