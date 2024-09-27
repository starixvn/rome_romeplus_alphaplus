using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
using AlphaPlus.Settings.MemberInfo.Domain.Constants;
using AlphaPlus.Settings.MemberInfo.Domain.Models.WinLossLimit;
using AlphaPlus.Settings.MemberInfo.Domain.Params;
using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.Components;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests.WinLossLimit;
using AlphaPlus.Settings.MemberInfo.Domain.Features.Shared.Models;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.WinLossLimit
{
    public class WinLossLimitViewModel
    {
        private const int MaxItemToShowPager = 100;
        private const decimal MaxWinLimitToShowNotAvailable = -3.00m;
        private const int DefaultNumberOfColumns = 13;

        private readonly Dictionary<int, int> NumberOfColumnBySportType = new Dictionary<int, int> {
            {SportTypeConstants.VGaming, 14},
            { SportTypeConstants.Sabaclub, 14},
            {SportTypeConstants.BBin, 14}
        };

        public WinLossLimitViewModel(
            WinLossLimitQuery message,
            IEnumerable<WinLossLimitGroupProductModel> groupProducts,
            IEnumerable<CustInfo> masterList,
            IEnumerable<CustInfo> agentList,
            WinLossLimitPage winLossLimitPage,
            WinLossLimitParam param)
        {
            ViewerLevel = message.ActorLevel;
            UserSelectedId = $"{param.ProductId}-{param.SportType}";
            Username = message.Username ?? string.Empty;
            PlayerId = message.PlayerId ?? string.Empty;
            MasterId = param.MasterId;
            AgentId = param.AgentId;

            GroupProducts = groupProducts;
            MasterList = masterList;
            AgentList = agentList;

            PageSize = message.PageSize;
            PageIndex = message.PageIndex;
            WinLossLimitPage = winLossLimitPage;

            SportType = param.SportType;
            BetType = param.BetType;
        }

        public UserLevel ViewerLevel { get; }

        public string UserSelectedId { get; }

        public IEnumerable<WinLossLimitGroupProductModel> GroupProducts { get; }

        public int MasterId { get; }

        public int AgentId { get; }

        public string Username { get; }

        public string PlayerId { get; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int SportType { get; }

        public bool IsSabaclub => SportType == SportTypeConstants.Sabaclub || SportType == SportTypeConstants.VGaming;

        public bool IsBBin => SportType == SportTypeConstants.BBin;

        public int BetType { get; }

        public IEnumerable<CustInfo> MasterList { get; }

        public IEnumerable<CustInfo> AgentList { get; }

        public WinLossLimitPage WinLossLimitPage { get; }

        public PagerViewModel PagerViewModel =>
            new(PageIndex, PageSize, WinLossLimitPage.NumberOfRows);

        public bool HasData => WinLossLimitPage.NumberOfRows > 0;

        public bool ShowPager => WinLossLimitPage.NumberOfRows >= MaxItemToShowPager;

        public int NumberOfColumns => NumberOfColumnBySportType.ContainsKey(SportType) ? NumberOfColumnBySportType[SportType] : DefaultNumberOfColumns;

        public static string GetPlayerIdText(string playerId)
            => string.IsNullOrWhiteSpace(playerId) ? "-" : playerId;

        public static string GetStatusText(string statusKey) => Resources.ResourceManager.GetString(statusKey);

        public static string GetCurrentWinAnmout(decimal? currentWinAnmout)
            => currentWinAnmout.HasValue ? currentWinAnmout.Value.ToString("#,##0.00") : Unlimited;

        public string GetMaxWinLimit(decimal maxWinLimit)
            => FormatMaxWinLimit(maxWinLimit);

        public string GetWinPercent(decimal? winPercent, decimal? currentWinLimit, decimal credit, decimal maxWinLimit)
            => FormatWinPercent(credit, currentWinLimit, winPercent, maxWinLimit);

        public static string GetLossPercent(decimal? maxLossLimit, decimal? lossPercent)
            => FormatLossPercent(maxLossLimit, lossPercent);

        public static string GetMaxLossLimit(decimal? maxLossLimit)
            => FormatMaxLossLimit(maxLossLimit);

        public static string GetRemainingBuyInAmount(decimal? remainingBuyInAmount, decimal? maxLossLimit)
            => FormatRemainingBuyInAmount(maxLossLimit, remainingBuyInAmount);

        public static string GetRemainingTransferInAmount(decimal? remainingTransferInAmount, decimal? maxLossLimit)
            => FormatRemainingTransferInAmmount(maxLossLimit, remainingTransferInAmount);

        private static string Unlimited
            => Lang.Unlimited;

        private static string FormatMaxWinLimit(decimal maxWinLimit)
        {
            if (maxWinLimit == MaxWinLimitToShowNotAvailable)
            {
                return "N/A";
            }

            return maxWinLimit == 0 ? Unlimited : $"{maxWinLimit.ToString("#,##0")}X";
        }

        private static string FormatWinPercent(decimal credit, decimal? currentWinLimit, decimal? winPercent, decimal maxWinLimit)
        {
            if (maxWinLimit == MaxWinLimitToShowNotAvailable || credit == 0 && currentWinLimit > 0)
            {
                return "N/A";
            }

            if (!winPercent.HasValue)
            {
                return Unlimited;
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
                return Unlimited;
            }

            return lossPercent > 0 ? $"{lossPercent.Value.ToString("#,##0.##")}%" : "0%";
        }

        private static string FormatMaxLossLimit(decimal? maxLossLimit)
        {
            if (!maxLossLimit.HasValue)
            {
                return "-";
            }

            return maxLossLimit.Value == 0 ? Unlimited : maxLossLimit.Value.ToString("#,##0.##");
        }

        private static string FormatRemainingBuyInAmount(decimal? maxLossLimit, decimal? remainingBuyInAmount)
        {
            if (maxLossLimit.HasValue && maxLossLimit.Value == 0)
            {
                return Unlimited;
            }

            return remainingBuyInAmount.HasValue ? remainingBuyInAmount.Value.ToString("#,##0.##") : "-";
        }

        private static string FormatRemainingTransferInAmmount(decimal? maxLossLimit, decimal? remainingTransferInAmount)
        {
            if (maxLossLimit.HasValue && maxLossLimit.Value == 0)
            {
                return Unlimited;
            }

            return remainingTransferInAmount.HasValue ? remainingTransferInAmount.Value.ToString("#,##0.##") : "-";
        }
    }
}