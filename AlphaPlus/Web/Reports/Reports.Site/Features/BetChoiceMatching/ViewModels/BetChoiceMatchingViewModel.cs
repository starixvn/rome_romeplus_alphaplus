using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Reports.Site.Features.BetChoiceMatching;

public class BetChoiceMatchingViewModel(BetChoiceMatchingQuery message, DateTime balanceUpDay)
{
    public string Today { get; } = DateTime.Today.ToShortDateWithSlash();

    public string MinDate { get; } = balanceUpDay.ToShortDateWithSlash();

    public string EventDate { get; } = message.EventDate.ToShortDateWithSlash();

    public int BetCount { get; } = message.BetCount;

    public PageHeaderViewModel PageHeader { get; }
        = new PageHeaderViewModel(Resources.BetChoiceMatching, IsFilterVisible: message.IsFilterVisible);

    public IEnumerable<SelectListItem> BetCounts
        => new int[] { 2, 3, 4, 5, 8, 10, 15, 20, 30, 50, 100 }
        .Select(item => new SelectListItem($">= {item}", item.ToString()));
}