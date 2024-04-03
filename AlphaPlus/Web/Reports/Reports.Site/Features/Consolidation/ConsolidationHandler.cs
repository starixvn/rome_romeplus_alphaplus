using AlphaPlus.Reports.Features.Consolidation;
using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Reports.Site.Features.Consolidation;

public class ConsolidationHandler(
    IProductService productService,
    IOptions<AppSettings> options,
    IEnteredResultService enteredResultService,
    IConsolidationService consolidationService) :
        BaseHandler(productService, options),
        IRequestHandler<ConsolidationByUsernameQuery, ConsolidationByUsernameViewModel>
{
    public async Task<ConsolidationByUsernameViewModel> Handle(ConsolidationByUsernameQuery message, CancellationToken cancellationToken)
    {
        var enteredResultTask = enteredResultService.GetTotalEnteredResults(message.Site);
        var productsTask = consolidationService.GetProducts(message.ViewerId, message.MinDate, appSettings.SettingStatus);
        var currenciesTask = consolidationService.GetCurrencies(message.ViewerId);
        await Task.WhenAll(enteredResultTask, productsTask, currenciesTask);

        var (productViewModel, productDetailXml) = BuildProductViewModel(message, message.UserSelectedProductIds, message.UserSelectedResourceIds, await productsTask);
        var param = new GetConsolidationParam(
            message.ViewerId,
            message.UserSelectedCurrencyId,
            message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
            message.ToDate,
            productViewModel.UserSelectedProductIds,
            productDetailXml,
            appSettings.SettingStatus,
            message.Site);
        var reportData = await consolidationService.GetConsolidationByUsername(param);


        throw new NotImplementedException();
    }
}