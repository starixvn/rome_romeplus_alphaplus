using AlphaPlus.Core.Models;
using AlphaPlus.Settings.MemberInfo.Domain.Models.WinLossLimit;
using AlphaPlus.Settings.MemberInfo.Domain.Params;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests.WinLossLimit;
using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.Components;
using AlphaPlus.Shared.AppResources.Settings.MemberInfo;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.WinLossLimit;

public class SportsbookPotentialWinLimitViewModel
{
    private const int MaxItemToShowPager = 100;
    private const decimal MaxWinLimitToShowNotAvailable = -3.00m;
    private const int SportsbookNumberOfColumns = 9;

    public SportsbookPotentialWinLimitViewModel(
        SportsbookPotentialWinLimitQuery message,
        PotentialWinLimitPage potentialWinLimitPage,
        PotentialWinLimitParam param)
    {
        ViewerLevel = message.ActorLevel;
        ProblemAccount = param.ProblemAccount;
        Status = param.Status;
        Username = message.Username ?? string.Empty;

        PageSize = message.PageSize;
        PageIndex = message.PageIndex;
        PotentialWinLimitPage = potentialWinLimitPage;

        SportType = param.SportType;
        BetType = param.BetType;
    }

    public UserLevel ViewerLevel { get; }

    public byte ProblemAccount { get; }

    public string Status { get; }

    public string Username { get; }

    public int PageSize { get; set; }

    public int PageIndex { get; set; }

    public int SportType { get; }

    public int BetType { get; }

    public PotentialWinLimitPage PotentialWinLimitPage { get; }

    public PagerViewModel PagerViewModel => new(PageIndex, PageSize, PotentialWinLimitPage.NumberOfRows);

    public bool HasData => PotentialWinLimitPage.NumberOfRows > 0;

    public bool ShowPager => PotentialWinLimitPage.NumberOfRows >= MaxItemToShowPager;

    public int NumberOfColumns => SportsbookNumberOfColumns;

    public static string GetStatusText(string statusKey) => Resources.ResourceManager.GetString(statusKey);

    public string GetMaxWinLimit(decimal maxWinLimit)
    {
        if (maxWinLimit == MaxWinLimitToShowNotAvailable)
        {
            return "N/A";
        }

        return maxWinLimit == 0 ? Lang.Unlimited : $"{maxWinLimit.ToString("#,##0")}";
    }

    public string GetWinPercent(decimal? winPercent, decimal maxWinLimit)
    {
        if (maxWinLimit == MaxWinLimitToShowNotAvailable)
        {
            return "N/A";
        }

        if (!winPercent.HasValue)
        {
            return Lang.Unlimited;
        }

        return winPercent > 0 ? $"{winPercent.Value.ToString("#,##0.##")}%" : "0%";
    }
    public class ItemListOption(string value, string text)
    {
        public string Value { get; } = value;

        public string Text { get; } = text;
    }

    public IEnumerable<ItemListOption> StatusOptions { get; } =
    [
        new ItemListOption("All", Resources.All),
        new ItemListOption("Open", Resources.Open),
        new ItemListOption("Disabled", Resources.Disabled),
        new ItemListOption("Closed", Resources.Closed),
        new ItemListOption("Suspended", Resources.Suspended),
        new ItemListOption("Locked", Resources.Locked)
    ];
    public IEnumerable<ItemListOption> ProblemAccountOptions { get; } =
    [
        new ItemListOption("0", Resources.All),
        new ItemListOption("1", Resources.Yes),
        new ItemListOption("2", Resources.No2)
    ];
} 