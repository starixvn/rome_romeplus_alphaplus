using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.Shared.Criteria;

public class GetEnableJackpotProductCriteria(bool isTest) : CriteriaBase
{
    public bool IsTest { get; } = isTest;

    public override string GetSettingKey() => "Acc_GameLaunchSel";

    public override bool IsValid() => true;
}