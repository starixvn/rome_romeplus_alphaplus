namespace AlphaPlus.Settings.ViewLogs.API.Services
{
    public interface RemotingRefWrapper
    {
        string GetBetType(object betTypeId, object betId = null);

        string GetBetTypeName(object bettypeId);

        string GetSportName(object sportId);

        string GetProductName(int productId, bool isSiteLabel777 = false);

        string GetSubProductGroupName(int subGroupId);

        string GetProductGroupName(int productGroupId);

        string GetSportbookCommissionName(int sportType, int betType);

        string GetSportbookMemberCommissionName(int sportType, int betType);

        string GetRNGGamesBetSettingName(int sportType, int betType);

        string GetParlayMaxPayoutSportBetTypeName(int sportType, int betType);

        string GetSportsBookBetSettingName(int sportType, int betType);

        string GetSportsBookPositionTaking(int sportType, int betType);

        string GetOtherGamesBetSettingName(int sportType, int betType);

        string GetPaymentMethodName(int methodId);

        string GetProviderName(int providerId);
    }
}