using AlphaPlus.Core.Models;
using AlphaPlus.Settings.MemberInfo.Domain.Constants;
using AlphaPlus.Settings.MemberInfo.Domain.Features.Shared.Models;
using AlphaPlus.Settings.MemberInfo.Domain.Models.WinLossLimit;
using AlphaPlus.Settings.MemberInfo.Domain.Params;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests.WinLossLimit;
using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.Components;
using AlphaPlus.Shared.AppResources.Settings.MemberInfo;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.WinLossLimit;

public class WinLossLimitViewModel(
    bool isOfferedCashService,
    WinLossLimitQuery message,
    IEnumerable<WinLossLimitGroupProductModel> groupProducts,
    IEnumerable<CustInfo> masterList,
    IEnumerable<CustInfo> agentList,
    WinLossLimitPage winLossLimitPage,
    WinLossLimitParam param)
{
    private const int MaxItemToShowPager = 100;
    private const decimal MaxWinLimitToShowNotAvailable = -3.00m;
    private const int DefaultNumberOfColumns = 13;

    private readonly IDictionary<int, int> NumberOfColumnBySportType = new Dictionary<int, int>
    {
        [SportTypeConstants.VGaming] = 14,
        [SportTypeConstants.Sabaclub] = 14,
        [SportTypeConstants.BBin] = 14
    };

    public UserLevel ViewerLevel { get; } = message.ActorLevel;

    public string UserSelectedId { get; } = $"{param.ProductId}-{param.SportType}";

    public IEnumerable<WinLossLimitGroupProductModel> GroupProducts { get; } = groupProducts;

    public int MasterId { get; } = param.MasterId;

    public int RecommendId { get; } = param.AgentId;

    public string Username { get; } = message.Username ?? string.Empty;

    public string PlayerId { get; } = message.PlayerId ?? string.Empty;

    public int PageSize { get; set; } = message.PageSize;

    public int PageIndex { get; set; } = message.PageIndex;

    public int SportType { get; } = param.SportType;

    public bool IsSabaclub => SportType is SportTypeConstants.Sabaclub or SportTypeConstants.VGaming;

    public bool IsBBin => SportType == SportTypeConstants.BBin;

    public int BetType { get; } = param.BetType;

    public int AccountType { get; } = param.AccountType;

    public IEnumerable<CustInfo> MasterList { get; } = masterList;

    public IEnumerable<CustInfo> AgentList { get; } = agentList;

    public WinLossLimitPage WinLossLimitPage { get; } = winLossLimitPage;

    public bool IsOfferedCashService { get; } = isOfferedCashService;

    public PagerViewModel PagerViewModel => new(PageIndex, PageSize, WinLossLimitPage.NumberOfRows);

    public bool HasData => WinLossLimitPage.NumberOfRows > 0;

    public bool ShowPager => WinLossLimitPage.NumberOfRows >= MaxItemToShowPager;

    public int NumberOfColumns =>
        (NumberOfColumnBySportType.ContainsKey(SportType) ? NumberOfColumnBySportType[SportType] : DefaultNumberOfColumns);

    public static string GetPlayerIdText(string playerId)
        => string.IsNullOrWhiteSpace(playerId) ? "-" : playerId;

    public static string GetStatusText(string statusKey) => Resources.ResourceManager.GetString(statusKey);

    public static string GetCurrentWinAmount(decimal? currentWinAmount)
        => currentWinAmount.HasValue ? currentWinAmount.Value.ToString("#,##0.00") : Lang.Unlimited;

    public string GetMaxWinLimit(decimal maxWinLimit)
        => FormatMaxWinLimit(maxWinLimit);

    public string GetMaxWinLimitCashMember(decimal maxWinLimit)
    {
        if (maxWinLimit == MaxWinLimitToShowNotAvailable)
        {
            return "N/A";
        }

        return maxWinLimit == 0 ? Lang.Unlimited : $"{maxWinLimit.ToString("#,##0.00")}";
    }

    public string GetWinPercent(decimal? winPercent, decimal? currentWinLimit, decimal credit, decimal maxWinLimit)
        => FormatWinPercent(credit, currentWinLimit, winPercent, maxWinLimit);

    public string GetWinPercentCashMember(decimal? winPercent, decimal maxWinLimit)
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

    public static string GetLossPercent(decimal? maxLossLimit, decimal? lossPercent)
        => FormatLossPercent(maxLossLimit, lossPercent);

    public static string GetMaxLossLimit(decimal? maxLossLimit)
        => FormatMaxLossLimit(maxLossLimit);

    public static string GetRemainingBuyInAmount(decimal? remainingBuyInAmount, decimal? maxLossLimit)
        => FormatRemainingBuyInAmount(maxLossLimit, remainingBuyInAmount);

    public static string GetRemainingTransferInAmount(decimal? remainingTransferInAmount, decimal? maxLossLimit)
        => FormatRemainingTransferInAmmount(maxLossLimit, remainingTransferInAmount);

    private static string FormatMaxWinLimit(decimal maxWinLimit)
    {
        if (maxWinLimit == MaxWinLimitToShowNotAvailable)
        {
            return "N/A";
        }

        return maxWinLimit == 0 ? Lang.Unlimited : $"{maxWinLimit.ToString("#,##0")}X";
    }

    private static string FormatWinPercent(decimal credit, decimal? currentWinLimit, decimal? winPercent, decimal maxWinLimit)
    {
        if (maxWinLimit == MaxWinLimitToShowNotAvailable || credit == 0 && currentWinLimit > 0)
        {
            return "N/A";
        }

        if (!winPercent.HasValue)
        {
            return Lang.Unlimited;
        }

        return winPercent > 0 ? $"{winPercent.Value.ToString("#,##0.##")}%" : "0%";
    }

    private static string FormatLossPercent(decimal? maxLossLimit, decimal? lossPercent)
    {
        if (!maxLossLimit.HasValue)
        {
            return "-";
        }

        if (!lossPercent.HasValue)
        {
            return Lang.Unlimited;
        }

        return lossPercent > 0 ? $"{lossPercent.Value.ToString("#,##0.##")}%" : "0%";
    }

    private static string FormatMaxLossLimit(decimal? maxLossLimit)
    {
        if (!maxLossLimit.HasValue)
        {
            return "-";
        }

        return maxLossLimit.Value == 0 ? Lang.Unlimited : maxLossLimit.Value.ToString("#,##0.##");
    }

    private static string FormatRemainingBuyInAmount(decimal? maxLossLimit, decimal? remainingBuyInAmount)
    {
        if (maxLossLimit.HasValue && maxLossLimit.Value == 0)
        {
            return Lang.Unlimited;
        }

        return remainingBuyInAmount.HasValue ? remainingBuyInAmount.Value.ToString("#,##0.##") : "-";
    }

    private static string FormatRemainingTransferInAmmount(decimal? maxLossLimit, decimal? remainingTransferInAmount)
    {
        if (maxLossLimit.HasValue && maxLossLimit.Value == 0)
        {
            return Lang.Unlimited;
        }

        return remainingTransferInAmount.HasValue ? remainingTransferInAmount.Value.ToString("#,##0.##") : "-";
    }
}