namespace AlphaPlus.Settings.MemberInfo.API.Criteria.WinLossLimit;

using Core.Criteria;
using Core.Helpers;

public class ResetCurrentSportsbookWinAmountCriteria(int custId,
    int subAccId,
    string subAccName,
    int userId,
    string ip,
    int settingStatus,
    int siteDbGroupId) : SiteDbBaseCommand(siteDbGroupId)
{
    public int CustId { get; } = custId;

    public int SubAccId { get; } = subAccId;

    public string SubAccName { get; } = subAccName;

    public int UserId { get; } = userId;

    public string IP { get; } = ip;

    public int SettingStatus { get; } = settingStatus;

    public override string GetSettingKey()
        => SpNameHelpers.SiteDbPrefix("Acc_CustWinLimitBetReset_CashMember", SiteDbGroupId);

    public override bool IsValid() => CustId > 0;
}