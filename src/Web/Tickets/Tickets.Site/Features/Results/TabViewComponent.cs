using AlphaPlus.Core.Models;
using AlphaPlus.Tickets.Site.Features.Shared;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Tickets.Site.Features.Results;

public class TabViewComponent(ICacheRefService cacheRefService) : ViewComponent
{
    public Task<IViewComponentResult> InvokeAsync(string tabName = "normal")
    {
        var viewModel = new TabViewModel(
            tabName,
            UserPermission.IsEnableProduct(ProductIdConstants.Keno),
            UserPermission.IsEnableProduct(ProductIdConstants.Lottery),
            UserPermission.IsEnableProduct(ProductIdConstants.VirtualGames),
            UserPermission.IsEnableProduct(ProductIdConstants.TableGames),
            UserPermission.IsEnableProduct(ProductIdConstants.SportsLottery),
            cacheRefService.GetProductName);

        return Task.FromResult((IViewComponentResult)View("~/Features/Results/Views/Tab.cshtml", viewModel));
    }
}