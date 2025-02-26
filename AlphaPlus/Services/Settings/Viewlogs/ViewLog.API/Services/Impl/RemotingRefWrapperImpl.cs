using AlphaPlus.Settings.ViewLogs.API.Constants;
using AlphaPlus.Core.Configurations;
using Fanex.RemotingRef;

namespace AlphaPlus.Settings.ViewLogs.API.Services.Impl
{
    public class RemotingRefWrapperImpl : RemotingRefWrapper
    {
        private readonly IRemotingRefService remotingRefService;

        private const string ProductRefId = "Product";

        private const string ProductUATRefId = "Product_UAT";

        private const string DummySportRefId = "DummySport";

        private const string DummySportUATRefId = "DummySport_UAT";

        public RemotingRefWrapperImpl(IRemotingRefService remotingRefService)
        {
            this.remotingRefService = remotingRefService;
        }

        public string GetBetType(object betTypeId, object betId = null)
        {
            // resourceId remove first character
            string resourceId = string.Empty;

            if (betId != null && betId.ToString().Length > 1)
            {
                resourceId = betId.ToString().Substring(1);
            }

            return betTypeId == null ?
                string.Empty :
                remotingRefService.GetBetTypeName(betTypeId.ToString(), LanguageCode, string.Empty, resourceId);
        }

        public string GetBetTypeName(object bettypeId)
        {
            return bettypeId != null
                ? remotingRefService.GetBetTypeName(bettypeId.ToString(), LanguageCode)
                : string.Empty;
        }

        public string GetSportName(object sportId)
        {
            var sportTypeName = remotingRefService.GetSportName(sportId.ToString(), LanguageCode);
            if (string.IsNullOrWhiteSpace(sportTypeName))
            {
                if (AppSettings.SettingStatus != 0)
                {
                    sportTypeName = remotingRefService.GetResourceDataByRefIdAndResourceId(
                    DummySportUATRefId,
                    sportId.ToString(),
                    LanguageCode,
                    string.Empty);
                }

                sportTypeName = string.IsNullOrEmpty(sportTypeName) ? remotingRefService.GetResourceDataByRefIdAndResourceId(
                    DummySportRefId,
                    sportId.ToString(),
                    LanguageCode,
                    sportId.ToString())
                    : sportTypeName;
            }

            return sportTypeName;
        }

        public string GetProductName(int productId, bool isSiteLabel777 = false)
        {
            string name = null;
            if (isSiteLabel777)
            {
                name = remotingRefService.GetResourceDataByRefIdAndResourceId($"{ProductRefId}_Saba", productId.ToString(), LanguageCode, string.Empty);

                return string.IsNullOrEmpty(name) ?
                    remotingRefService.GetResourceDataByRefIdAndResourceId(ProductRefId, productId.ToString(), LanguageCode, productId.ToString()) :
                    name;
            }

            if (AppSettings.SettingStatus != 0)
            {
                name = remotingRefService.GetResourceDataByRefIdAndResourceId(ProductUATRefId, productId.ToString(), LanguageCode, string.Empty);
            }

            return string.IsNullOrEmpty(name) ?
                remotingRefService.GetResourceDataByRefIdAndResourceId(ProductRefId, productId.ToString(), LanguageCode, productId.ToString()) :
                name;
        }

        public string GetSubProductGroupName(int subGroupId)
        {
            return remotingRefService.GetResourceDataByRefIdAndResourceId("SubGroup", subGroupId.ToString(), LanguageCode, string.Empty);
        }

        public string GetProductGroupName(int productGroupId)
        {
            return remotingRefService.GetResourceDataByRefIdAndResourceId("ProductGroup", productGroupId.ToString(), LanguageCode, string.Empty);
        }

        public string GetSportbookCommissionName(int sportType, int betType)
        {
            return remotingRefService.GetResourceDataByRefIdAndResourceId(
                "SB_Comm_SMA",
                $"{ProductGroups.SportsBook}_{GroupTypes.SportsBookComm}_{sportType}_{betType}",
                LanguageCode,
                string.Empty);
        }

        public string GetRNGGamesBetSettingName(int sportType, int betType)
        {
            return remotingRefService.GetResourceDataByRefIdAndResourceId(
                "G_GT_Sport_Bettype",
                $"{ProductGroups.RNGGames}_{GroupTypes.RNGGamesBetSetting}_{sportType}_{betType}",
                LanguageCode,
                string.Empty);
        }

        public string GetParlayMaxPayoutSportBetTypeName(int sportType, int betType)
        {
            return remotingRefService.GetResourceDataByRefIdAndResourceId(
                "G_GT_Sport_Bettype",
                $"{ProductGroups.SportsBook}_{GroupTypes.ParlayMaxPayout}_{sportType}_{betType}",
                LanguageCode,
                string.Empty);
        }

        public string GetSportsBookBetSettingName(int sportType, int betType)
        {
            var value = remotingRefService.GetResourceDataByRefIdAndResourceId(
                "G_GT_Sport_Bettype",
                $"{ProductGroups.SportsBook}_{GroupTypes.SportsBookBetSetting}_{sportType}_{betType}",
                LanguageCode,
                string.Empty);
            var valueFor777 = string.Empty;
            if (AppSettings.IsSiteLabel777)
            {
                valueFor777 = remotingRefService.GetResourceDataByRefIdAndResourceId(
                "G_GT_Sport_BT_Saba",
                $"{ProductGroups.SportsBook}_{GroupTypes.SportsBookBetSetting}_{sportType}_{betType}",
                LanguageCode,
                string.Empty);
            }

            return string.IsNullOrEmpty(valueFor777) ? value : valueFor777;
        }

        public string GetOtherGamesBetSettingName(int sportType, int betType)
        {
            return remotingRefService.GetResourceDataByRefIdAndResourceId(
                "G_GT_Sport_Bettype",
                $"{ProductGroups.OtherGames}_{GroupTypes.OtherGamesBetSetting}_{sportType}_{betType}",
                LanguageCode,
                string.Empty);
        }

        public string GetSportsBookPositionTaking(int sportType, int betType)
        {
            return remotingRefService.GetResourceDataByRefIdAndResourceId(
                "G_GT_Sport_Bettype",
                $"{ProductGroups.SportsBook}_{GroupTypes.PositionTaking}_{sportType}_{betType}",
                LanguageCode,
                string.Empty);
        }

        public string GetSportbookMemberCommissionName(int sportType, int betType)
        {
            return remotingRefService.GetResourceDataByRefIdAndResourceId(
                "SB_Comm_Member",
                $"{ProductGroups.SportsBook}_{GroupTypes.SportsBookComm}_{sportType}_{betType}",
                LanguageCode,
                string.Empty);
        }

        private static string LanguageCode
        => Thread.CurrentThread.CurrentCulture.Name;
    }
}