using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.WinLossLimit;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.ViewComponents;

public class TabViewComponent : ViewComponent
{
    public Task<IViewComponentResult> InvokeAsync(string tabName = "index")
    {
        var viewModel = new TabViewModel(tabName);

        return Task.FromResult((IViewComponentResult)View("~/Features/MemberInfo/Views/WinLossLimit/Tab.cshtml", viewModel));
    }
} 