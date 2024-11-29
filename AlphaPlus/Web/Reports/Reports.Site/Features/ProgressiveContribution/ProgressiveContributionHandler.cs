using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.ProgressiveContribution;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Legends;
using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.Extensions.Options;
using AlphaPlus.Shared.AppResources.Report;
using AlphaPlus.Reports.Features.Products;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

namespace AlphaPlus.Reports.Site.Features.ProgressiveContribution;

public class ProgressiveContributionHandler(
    IProductService productService,
    IOptions<AppSettings> options,
    IProgressiveContributionService progressiveContributionService,
    IProgressiveContributionWookbook wookbook,
    Fanex.Logging.ILogger logger) :
        BaseHandler(productService, options),
        IRequestHandler<ProgressiveContributionQuery, ProgressiveContributionViewModel>,
        IRequestHandler<ProgressiveContributionExcelQuery, ExcelFileViewModel>,
        IRequestHandler<ProgressiveContributionLegendQuery, LegendViewModel>
{
    private readonly IProgressiveContributionService progressiveContributionService = progressiveContributionService;
    private readonly IProgressiveContributionWookbook wookbook = wookbook;
    private readonly Fanex.Logging.ILogger logger = logger;

    public async Task<ProgressiveContributionViewModel> Handle(ProgressiveContributionQuery message, CancellationToken cancellationToken)
    {
        var productViewModel = await BuildProductViewModel(message, message.UserSelectedProductIds);
        var (progressiveContributions, total) = await GetProgressiveContributions(message, productViewModel.UserSelectedProductIds);

        return new ProgressiveContributionViewModel(
            message,
            progressiveContributions,
            total,
            productViewModel);
    }

    public async Task<ExcelFileViewModel> Handle(ProgressiveContributionExcelQuery message, CancellationToken cancellationToken)
    {
        var validUserSelectedProductIds = await FilterProductIds(message, message.UserSelectedProductIds);
        var (progressiveContributions, total) = await GetProgressiveContributions(message, validUserSelectedProductIds.JoinWithComma());
        var productNames = productService.ConcatProductNames(validUserSelectedProductIds);

        return wookbook.BuildExcelFile(message, progressiveContributions, total, productNames);
    }

    private Task<(IEnumerable<ProgressiveContributionModel>, ProgressiveContributionEntity)> GetProgressiveContributions(
        ProgressiveContributionQuery message,
        string validUserSelectedProductIds)
    {
        var param = new ProgressiveContributionParam(
            message.ViewerId,
            message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
            message.ToDate,
            validUserSelectedProductIds,
            message.ViewerLevel.Value,
            message.Site);
        return progressiveContributionService.GetProgressiveContributions(param);
    }

    public async Task<LegendViewModel> Handle(ProgressiveContributionLegendQuery message, CancellationToken cancellationToken)
    {
        var legends = new List<string>
        {
            Resources.ContributionAmountRoundNote
        };

        var productParam = new GetProductsParam(
            message.Site,
            message.CurrencyId,
            appSettings.SettingStatus,
            message.Language,
            message.MinDate);
        try
        {
            var productNames = await productService.GetProductsNeedDelayTimeToSummarizeData(productParam, message.EnableJackpotProductIds);

            if (!string.IsNullOrWhiteSpace(productNames))
            {
                legends.Add(string.Format(Resources.DelayIssueLegend, productNames));
            }
        }
        catch (Exception ex)
        {
            await logger.ErrorAsync(ex.Message, ex);
        }

        return new LegendViewModel(legends);
    }

    private async Task<ProductMultiSelectionViewModel> BuildProductViewModel(
        ProgressiveContributionQuery message,
        string userSelectedProductIds)
    {
        var products = (await GetProducts(message)).Where(p => message.EnableJackpotProductIds.Contains(p.Id));
        var (productGroups, validUserSelectedProductIds) = BuildProductGroups(products, userSelectedProductIds);

        return new ProductMultiSelectionViewModel(productGroups, validUserSelectedProductIds.JoinWithComma(), message.Language);
    }

    private static (IEnumerable<ProductGroupDTO>, IEnumerable<int>) BuildProductGroups(
        IEnumerable<Product> products,
        string userSelectedProductIds)
    {
        var productGroups = new List<ProductGroupDTO>();
        var validUserSelectedProductIds = new List<int>();

        var isCheckedAll = "*" == userSelectedProductIds;
        var userSelectedProductIdsList = isCheckedAll ? [] : userSelectedProductIds.ToListInt().ToList();

        foreach (var item in products)
        {
            var isChecked = isCheckedAll || userSelectedProductIdsList.Contains(item.Id);
            if (isChecked)
            {
                validUserSelectedProductIds.Add(item.Id);
            }

            productGroups.Add(new ProductGroupDTO(item.Id, item.Name, isChecked, null));
        }

        return (productGroups, validUserSelectedProductIds);
    }

    private async Task<IEnumerable<int>> FilterProductIds(ProgressiveContributionExcelQuery message, string userSelectedProductIds)
    {
        var products = (await GetProducts(message)).Where(p => message.EnableJackpotProductIds.Contains(p.Id));

        if ("*" != userSelectedProductIds)
        {
            var userSelectedProductIdsList = userSelectedProductIds.ToListInt().ToList();
            products = products.Where(product => userSelectedProductIdsList.Contains(product.Id));
        }

        return products.Select(product => product.Id).ToList();
    }
}
