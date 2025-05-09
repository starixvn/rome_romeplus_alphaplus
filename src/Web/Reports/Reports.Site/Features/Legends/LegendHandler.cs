using AlphaPlus.Core.Extensions;
using MediatR;
using AlphaPlus.Reports.Features.Shared.Models;
using Microsoft.Extensions.Options;
using AlphaPlus.Shared.AppResources.Report;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.Products;
using AlphaPlus.Shared.AppResources.Common;

namespace AlphaPlus.Reports.Site.Features.Legends;

public class LegendHandler(
    IOptions<AppSettings> options,
    IProductService productService,
    ICacheRefService cacheRefService,
    Fanex.Logging.ILogger logger) :
        IRequestHandler<LegendQuery, LegendViewModel>,
        IRequestHandler<WinLossLegendQuery, LegendViewModel>,
        IRequestHandler<WinLossDetailLegendQuery, LegendViewModel>,
        IRequestHandler<MatchWinLossDetailLegendQuery, LegendViewModel>,
        IRequestHandler<OutstandingLegendQuery, LegendViewModel>
{
    private readonly AppSettings appSettings = options.Value;
    private readonly IProductService productService = productService;
    private readonly ICacheRefService cacheRefService = cacheRefService;
    private readonly Fanex.Logging.ILogger logger = logger;

    public async Task<LegendViewModel> Handle(OutstandingLegendQuery message, CancellationToken cancellationToken)
    {
        var legends = new List<string>
        {
            $"- {CommonResx.MemberWinNote}"
        };

        return new LegendViewModel(legends);
    }

    public async Task<LegendViewModel> Handle(LegendQuery message, CancellationToken cancellationToken)
    {
        var legends = new List<string>();
        var productParam = new GetProductsParam(
            message.Site,
            message.CurrencyId,
            appSettings.SettingStatus,
            message.Language,
            message.MinDate);
        await AddOfferedProductsNeedDelayTimeToSummarizeData(legends, productParam);

        return new LegendViewModel(legends);
    }

    public async Task<LegendViewModel> Handle(WinLossLegendQuery message, CancellationToken cancellationToken)
    {
        var legends = new List<string>();
        if (!message.IsHistoricReport && message.MinDate != DateTime.MinValue)
        {
            legends.Add(string.Format(
                $"- {Resources.CanOnlyViewDataAfterTimeCurrentReport}", message.MinDate.ToShortDateWithSlash()));
        }

        var productParam = new GetProductsParam(
            message.Site,
            message.CurrencyId,
            appSettings.SettingStatus,
            message.Language,
            message.MinDate);
        await AddOfferedProductsNeedDelayTimeToSummarizeData(legends, productParam);
        legends.Add($"- {await GetProductDetailsBelongSabaOtherSportsNote(message.ViewerId, productParam)}");
        legends.Add(GetGrossCommNotes(message.ViewerLevel, message.IsConsolidation));
        legends.Add(Resources.GrossCommRoundingNote);
        if (message.ShowDormantAccountsNote)
        {
            legends.Add(Resources.DormantAccountsNote);
        }

        return new LegendViewModel(legends);
    }

    public async Task<LegendViewModel> Handle(WinLossDetailLegendQuery message, CancellationToken cancellationToken)
    {
        var legends = new List<string>();
        if (message.IsNew)
        {
            legends.Add(string.Format($"- {Resources.ReportDataCanViewNote}", message.MinDate.ToShortDateWithSlash()));
        }
        else if (!message.IsHistoricReport && message.MinDate != DateTime.MinValue)
        {
            legends.Add(string.Format(
                $"- {Resources.CanOnlyViewDataAfterTimeCurrentReport}", message.MinDate.ToShortDateWithSlash()));
        }

        legends.Add(Resources.Tooltip_NetTurnover);

        var productParam = new GetProductsParam(
            message.Site,
            message.CurrencyId,
            appSettings.SettingStatus,
            message.Language,
            message.MinDate);
        var liveCasinoTooltip = await GetProductNamesForLiveCasinoTooltip(productParam);
        if (!string.IsNullOrWhiteSpace(liveCasinoTooltip))
        {
            legends.Add(string.Format(Resources.Tooltip_NetTurnover_livecasino, liveCasinoTooltip));
        }

        var rngCasinoTooltip = await GetProductNamesForRNGCasinoTooltip(productParam);
        if (!string.IsNullOrWhiteSpace(rngCasinoTooltip))
        {
            legends.Add(string.Format(Resources.Tooltip_NetTurnover_RNGCasino, rngCasinoTooltip));
        }

        await AddOfferedProductsNeedDelayTimeToSummarizeData(legends, productParam);
        legends.Add($"- {await GetProductDetailsBelongSabaOtherSportsNote(message.ViewerId, productParam)}");
        legends.Add($"- {await GetProductDetailsBelongSoccerPinGoal(message.ViewerId, productParam)}");
        legends.Add(GetGrossCommNotes(message.ViewerLevel, false));
        legends.Add(Resources.GrossCommRoundingNote);
        if (message.ShowDormantAccountsNote)
        {
            legends.Add(Resources.DormantAccountsNote);
        }

        return new LegendViewModel(legends, MobileFriendly: message.IsNew);
    }

    private async Task<string> GetProductDetailsBelongSabaOtherSportsNote(int custId, GetProductsParam param)
    {
        var products = await GetProducts(param);
        var productDetailsBelongSabaOthers = (await GetProductDetails(custId, products))
            .Where(i => i.ResourceId == ProductDetailIdConstants.SabaOtherSports)
            .OrderBy(product => product.Order)
            .Select(i => cacheRefService.GetProductDetailName(i.ChildResourceId))
            .JoinWithComma(", ");

        return string.Format(
            Resources.SabaOtherSportsLegend,
            productDetailsBelongSabaOthers,
            cacheRefService.GetProductDetailName(ProductDetailIdConstants.SabaOtherSports));
    }
    private async Task<string> GetProductDetailsBelongSoccerPinGoal(int custId, GetProductsParam param)
    {
        var products = await GetProducts(param);
        var productDetailsBelongSoccerPinGoal = (await GetProductDetails(custId, products))
            .Where(i => i.ResourceId == ProductDetailIdConstants.SoccerPinGoal && i.ChildResourceId != 0)
            .OrderBy(product => product.Order)
            .Select(i => cacheRefService.GetProductDetailName(i.ChildResourceId))
            .JoinWithComma(", ");

        return string.Format(
            Resources.SabaOtherSportsLegend,
            productDetailsBelongSoccerPinGoal,
            cacheRefService.GetProductDetailName(ProductDetailIdConstants.SoccerPinGoal));
    }

    public Task<LegendViewModel> Handle(MatchWinLossDetailLegendQuery message, CancellationToken cancellationToken)
    {
        var legends = new List<string>(GetGrossCommNotesByUserLevel(message.ViewerLevel));
        var viewModel = new LegendViewModel([$"- {Resources.GrossComm}: <ul class=\"gross-comm\">{string.Join(string.Empty, legends)}</ul>"]);

        return Task.FromResult(viewModel);
    }

    private async Task AddOfferedProductsNeedDelayTimeToSummarizeData(
        IList<string> legends,
        GetProductsParam productParam)
    {
        try
        {
            var productNames = await productService.GetProductsNeedDelayTimeToSummarizeData(productParam);

            if (!string.IsNullOrWhiteSpace(productNames))
            {
                legends.Add(string.Format(Resources.DelayIssueLegend, productNames));
            }
        }
        catch (Exception ex)
        {
            await logger.ErrorAsync(ex.Message, ex);
        }
    }

    private static string GetGrossCommNotes(UserLevel viewerLevel, bool isConsolidation)
    {
        var grossCommNotes = new List<string>(
            isConsolidation ? GetConsolidationReportGrossCommNotes() : GetGrossCommNotesByUserLevel(viewerLevel))
        {
            $"<li>{Resources.GrossCommNoteForCasino}</li>"
        };

        if (viewerLevel.IsNotAgentLevel())
        {
            grossCommNotes.Add($"<li>{Resources.GrossCommNoteForRoundingProblem}</li>");
        }

        return $"- {Resources.GrossComm}: <ul class=\"gross-comm\">{string.Join(string.Empty, grossCommNotes)}</ul>";
    }

    private static IEnumerable<string> GetGrossCommNotesByUserLevel(UserLevel viewerLevel)
    {
        if (viewerLevel.IsSuperLevel())
        {
            return [
                $"<li>{Resources.GrossCommNoteForMaster}</li>",
                $"<li>{Resources.GrossCommNoteForAgent}</li>",
                $"<li>{Resources.GrossCommNoteForMember}</li>"
            ];
        }

        if (viewerLevel.IsMasterLevel())
        {
            return [
                $"<li>{Resources.GrossCommNoteForAgent}</li>",
                $"<li>{Resources.GrossCommNoteForMember}</li>"
            ];
        }

        return [$"<li>{Resources.GrossCommNoteForMember}</li>"];
    }

    private static IEnumerable<string> GetConsolidationReportGrossCommNotes()
    {
        yield return $"<li>{Resources.GrossCommNoteForConsolidationReportSuper}</li>";
        yield return $"<li>{Resources.GrossCommNoteForConsolidationReportMaster}</li>";
        yield return $"<li>{Resources.GrossCommNoteForConsolidationReportAgent}</li>";
    }

    #region Get Product Names
    private async Task<string> GetProductNamesForLiveCasinoTooltip(GetProductsParam productsParam)
    {
        try
        {
            return await productService.GetProductNames(productsParam, LegendProductIdConstants.LiveCasinoProductIds);
        }
        catch (Exception ex)
        {
            await logger.ErrorAsync(ex.Message, ex);
            return string.Empty;
        }
    }

    private async Task<string> GetProductNamesForRNGCasinoTooltip(GetProductsParam productsParam)
    {
        var productIds = (await GetProducts(productsParam))
            .Where(product => product.Type == LegendProductIdConstants.DummyRNGProductId || LegendProductIdConstants.RNGCasinoProductIds.Contains(product.Id))
            .Select(product => product.Id);

        return productService.ConcatProductNames(productIds);
    }

    private async Task<IEnumerable<Product>> GetProducts(GetProductsParam param)
    {
        try
        {
            return await productService.GetProducts(param);
        }
        catch (Exception ex)
        {
            await logger.ErrorAsync(ex.Message, ex);
            return [];
        }
    }

    private async Task<IEnumerable<ProductDetail>> GetProductDetails(int custId, IEnumerable<Product> products)
    {
        try
        {
            return await productService.GetProductDetails(custId, products);
        }
        catch (Exception ex)
        {
            await logger.ErrorAsync(ex.Message, ex);
            return [];
        }
    }
    #endregion
}