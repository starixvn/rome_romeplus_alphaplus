using AlphaPlus.Shared.AppResources.Betlist.Services;
using AlphaPlus.BetLists.LastBets;
using AlphaPlus.BetLists.Shared.Helpers;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.LastBets;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.LastBets;

public class FilterConfigurationViewModel(FilterConfigurationQuery message, int[] betTypes)
{
    public string FilterName { get; } = message.FilterName;

    public int LimitRowCount { get; } = message.LimitRowCount;

    public bool AutoStart { get; } = message.AutoStart;

    public bool ShowWelcomePopup { get; } = message.Welcome;

    public IEnumerable<SelectListItem> BetTypes
    {
        get
        {
            foreach (var item in betTypes)
            {
                yield return BuildSelectListItem(BetTypeHelper.GetBetTypeName(item), item, true);
            }
        }
    }

    public IEnumerable<SelectListItem> LiveStatus
    {
        get
        {
            yield return BuildSelectListItem(Resources.All, LiveStatusConstants.All, true);
            yield return BuildSelectListItem(Resources.Live, LiveStatusConstants.Live);
            yield return BuildSelectListItem(Resources.NonLive, LiveStatusConstants.NonLive);
        }
    }

    public IEnumerable<SelectListItem> SportGroups
    {
        get
        {
            yield return BuildSelectListItem(Resources.All, SportGroupConstants.All);
            yield return BuildSelectListItem(Resources.MajorSports, SportGroupConstants.MajorSports, true);
            yield return BuildSelectListItem(Resources.MinorSports, SportGroupConstants.MinorSports);
        }
    }

    private static SelectListItem BuildSelectListItem(string text, int value, bool selected = false)
        => new SelectListItem(text, value.ToString(), selected);
}