using AlphaPlus.Core.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.Main.Site.Features.Dashboard.BalanceInfo;

public class GetBalanceInfoCriteria(UserLevel userLevel, int custId) : CriteriaBase
{
    private static readonly Dictionary<UserLevel, string> SettingKeys = new()
    {
        [UserLevel.Agent] = "Age_CustomerSelBalanceInfo_Ag",
        [UserLevel.Master] = "Age_CustomerSelBalanceInfo_MAg",
        [UserLevel.Super] = "Age_CustomerSelBalanceInfo_SAg"
    };

    public int CustId { get; } = custId;

    public override string GetSettingKey() => SettingKeys[userLevel];

    public override bool IsValid() => CustId > 0;
}