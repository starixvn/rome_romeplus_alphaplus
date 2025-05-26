using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Transfer.API.Criteria.TransferYesterdayBalance;

public class SelectMemberWinLimitCriteria(string custIds, byte settingStatus = 0) : CriteriaBase
{
    public string CustIds { get; } = custIds;

    public byte SettingStatus { get; } = settingStatus;

    public override bool IsValid() => !string.IsNullOrEmpty(CustIds);

    public override string GetSettingKey() => "Acc_CustPotentialWinLimit_SelAutoResetMembers";
}