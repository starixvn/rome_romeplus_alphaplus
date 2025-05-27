using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Transfer.API.Criteria.TransferYesterdayBalance;

public class SelectMemberWinLimitCriteria(string custIds) : CriteriaBase
{
    [SpParam(SqlParamName ="ListCustId")]
    public string CustIds { get; } = custIds;

    public override bool IsValid() => !string.IsNullOrEmpty(CustIds);

    public override string GetSettingKey() => "Acc_CustPotentialWinLimit_SelAutoResetMembers";
}