namespace AlphaPlus.Settings.MemberInfo.API.Criteria.CreditBalance;

public class GetSuperClosedReasonCriteria(
    int custId,
    string userName,
    bool? enabled,
    bool? closed,
    bool? suspended,
    byte levelType,
    int pageIndex,
    int pageSize) : GetMasterClosedReasonCriteria(custId, userName, enabled, closed, suspended, levelType, pageIndex, pageSize)
{
    public override string GetSettingKey() => "Age_CustomerSelCreditBalClosedReason_SAg";
}
